using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Doods.Framework.ApiClientBase.Std.Classes;
using Doods.Framework.ApiClientBase.Std.Exceptions;
using Doods.Framework.ApiClientBase.Std.Interfaces;
using Doods.Framework.Ssh.Std.Enums;
using Doods.Framework.Ssh.Std.Extensions;
using Doods.Framework.Ssh.Std.Interfaces;
using Doods.Framework.Std;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace Doods.Framework.Ssh.Std
{
    public class SshServiceBase : APIServiceBase, IDisposable, IClientSsh
    {
        protected const int TimeoutInSecond = 60;
        private readonly object _lockObj;


        private ShellStream _shell;


        //private ConnectionInfo _connectionInfo;
        protected SshServiceBase(ILogger logger)
        {
            _lockObj = new object();
            Logger = logger;
        }

        public SemaphoreSlim ReadLock { get; } = new SemaphoreSlim(1, 1);
        public ILogger Logger { get; }

        public SshClient Client { get; private set; }


        public Task<string> RunCommandAsync(SshCommand cmd, CancellationToken token)
        {
            if (Client == null) return null;
            try
            {
                using (CancellationTokenSource.CreateLinkedTokenSource(token))
                {
                    //await _readLock.WaitAsync(token);
                    //var request = _client.CreateCommand(cmdStr);


                    var ret = Task.Run(() => Client.RunCommand(cmd.CommandText).Result, token);


                    //var res = Task.Factory.FromAsync(cmd.BeginExecute, cmd.EndExecute, null);
                    return ret;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

                throw ex;
            }

            //return sshCommand.Result;
        }


        public Task<ISshApiResponse<T>> ExecuteTaskAsync<T>(ISshRequest request)
        {
            return ExecuteTaskAsync<T>(request, CancellationToken.None);
        }

        public Task<ISshApiResponse<T>> ExecuteTaskAsync<T>(ISshRequest request, CancellationToken token)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            var taskCompletionSource = new TaskCompletionSource<ISshApiResponse<T>>();
            try
            {
                var async = ExecuteAsync(request,
                    (Action<ISshApiResponse<T>, SshRequestAsyncHandle>) ((response, _) =>
                    {
                        if (token.IsCancellationRequested)
                            taskCompletionSource.TrySetCanceled();
                        else
                            taskCompletionSource.TrySetResult(response);
                    }));
                var registration = token.Register(() =>
                {
                    async.Abort();
                    taskCompletionSource.TrySetCanceled();
                });
                taskCompletionSource.Task.ContinueWith(t => registration.Dispose(),
                    token);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                taskCompletionSource.TrySetException(ex);
            }

            return taskCompletionSource.Task;
        }

        public SshCommand RunQuerry(string cmd)
        {
            return Client.RunCommand(cmd);
        }


        public async Task<bool> ConnectAsync()
        {
            var res = await Task.Run(() => Connect());
            return IsConnected();
            //await Task.Factory.StartNew(Connect);
        }

        public bool Connect()
        {
            lock (_lockObj)
            {
                if (Client == null)
                {
                    GetSshClient();
                }
                else
                {
                    if (Client.IsConnected)
                        Client.Disconnect();
                    Client = null;
                    GetSshClient();
                }

                try
                {
                    Client.Connect();
                }
                catch (Exception ex)
                {
                    Logger.Debug(ex.Message);
                    //TODO THE Renci.SshNet.Common.SshOperationTimeoutException 
                    throw;
                }

                return Client.IsConnected;
            }
        }

        public bool IsConnected()
        {
            lock (_lockObj)
            {
                return IsAuthenticated();
            }
        }


        public bool IsAuthenticated()
        {
            lock (_lockObj)
            {
                if (Client == null) return false;
                return Client.IsConnected;
            }
        }


        public ShellStream CreateShell()
        {
            return _shell = Client.CreateShellStream(nameof(SshServiceBase), 0, 0, 0, 0, 1024);
        }

        public ScpClient GetScpClient()
        {
            var test =
                new PasswordConnectionInfo(Connection.Host, Connection.Port, Connection.Credentials.Login,
                        Connection.Credentials.Password)
                    {Timeout = TimeSpan.FromSeconds(10)};
            return new ScpClient(test);
        }

        /// <summary>
        ///     Try to connect to client
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="throwException"></param>
        /// <returns>true if connection succeeded</returns>
        /// <exception cref="T:Exception"></exception>
        /// <exception cref="T:DoodsApiConnectionExceptionn">SSH session could not be established.</exception>
        /// <exception cref="T:DoodsApiAuthenticationException">Authentication of SSH session failed.</exception>
        public bool TestConnection(IConnection connection, bool throwException)
        {
            var testConnectionResult = false;
            SshClient client = null;
            try
            {
                var test =
                    new PasswordConnectionInfo(connection.Host, connection.Port, connection.Credentials.Login,
                            connection.Credentials.Password)
                        {Timeout = TimeSpan.FromSeconds(10)};
                client = new SshClient(test);

                //client.HostKeyReceived += (sender, e) =>
                //{
                //    if (true)
                //    {
                //        e.CanTrust = false;
                //    }

                //};

                client.Connect();

                // InvalidOperationException
                // ObjectDisposedException
                // SocketException
                // SshConnectionException
                // SshAuthenticationException
                // ProxyException
                testConnectionResult = client.IsConnected;
            }
            catch (SshConnectionException ex)
            {
                if (throwException)
                    throw new DoodsApiConnectionException(ex.Message);
            }
            catch (SshAuthenticationException ex)
            {
                if (throwException)
                    throw new DoodsApiAuthenticationException(ex.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (throwException)
                    throw;
            }
            finally
            {
                client?.Dispose();
            }


            return testConnectionResult;
        }

        public void Dispose()
        {
            lock (_lockObj)
            {
                Client?.Dispose();
                Client = null;
            }
        }

        public Task<IEnumerable<byte[]>> GetFilesAsync(IEnumerable<string> filesPath)
        {
            var lst = new List<byte[]>();
            var scp = GetScpClient();
            if (!scp.IsConnected) scp.Connect();
            scp.RemotePathTransformation = RemotePathTransformation.ShellQuote;

            foreach (var filepath in filesPath)
                using (var ms = new MemoryStream())
                {
                    try
                    {
                        scp.Download(filepath.Trim(), ms);
                        var byteArray = ms.ToArray();
                        lst.Add(byteArray);
                    }
                    catch 
                    {
                      
                    }
                  
                }
            scp.Dispose();
            return Task.FromResult(lst.AsEnumerable());
        }

        public Task<byte[]> GetFileAsync(string filePath)
        {
            var scp = GetScpClient();
            if (!scp.IsConnected) scp.Connect();
            scp.RemotePathTransformation = RemotePathTransformation.ShellQuote;
            using (var ms = new MemoryStream())
            {
                try
                {

                    scp.Download(filePath.Trim(), ms);
                    var byteArray = ms.ToArray();
                    return Task.FromResult(byteArray);
                }
                finally
                {
                    scp.Dispose();
                }
              
            }

            return Task.FromResult(default(byte[]));
        }

        protected virtual SshClient GetSshClient()
        {
            var test =
                new PasswordConnectionInfo(Connection.Host, Connection.Port, Connection.Credentials.Login,
                    Connection.Credentials.Password) {Timeout = TimeSpan.FromSeconds(10)};
            return Client ?? (Client = new SshClient(test));
        }

        public SshRequestAsyncHandle ExecuteAsync<T>(ISshRequest request,
            Action<ISshApiResponse<T>, SshRequestAsyncHandle> callback)
        {
            return ExecuteAsync(request,
                (response, asyncHandle) =>
                    DeserializeResponse(request, callback, response, asyncHandle));
        }


        private void DeserializeResponse<T>(ISshRequest request,
            Action<ISshApiResponse<T>, SshRequestAsyncHandle> callback, ISshApiResponse apiResponse,
            SshRequestAsyncHandle asyncHandle)
        {
            ISshApiResponse<T> sshApiResponse1;
            try
            {
                sshApiResponse1 = Deserialize<T>(request, apiResponse);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                var restResponse2 = new SshApiResponse<T>();
                restResponse2.Request = request;
                restResponse2.ResponseStatus = ResponseStatus.Error;
                restResponse2.ErrorMessage = ex.Message;
                restResponse2.ErrorException = ex;
                sshApiResponse1 = restResponse2;
            }

            callback(sshApiResponse1, asyncHandle);
        }


        private ISshApiResponse<T> Deserialize<T>(ISshRequest request, ISshApiResponse raw)
        {
            //request.OnBeforeDeserialization(raw);
            var restResponse = (ISshApiResponse<T>) new SshApiResponse<T>();
            try
            {
                restResponse = raw.ToAsyncResponse<T>();
                restResponse.Request = request;
                if (restResponse.ErrorException == null)
                {
                    var handler = request.Handler;
                    if (handler != null) restResponse.Data = handler.Deserialize<T>(raw);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                restResponse.ResponseStatus = ResponseStatus.Error;
                restResponse.ErrorMessage = ex.Message;
                restResponse.ErrorException = ex;
            }

            return restResponse;
        }

        public SshRequestAsyncHandle ExecuteAsync(ISshRequest request,
            Action<ISshApiResponse, SshRequestAsyncHandle> callback)
        {
            return ExecuteAsync(request, callback,
                DoAsGetAsync);
        }


        private SshRequestAsyncHandle ExecuteAsync(ISshRequest request,
            Action<ISshApiResponse, SshRequestAsyncHandle> callback,
            Func<SshClient, ISshRequest, Action<SshApiResponse>, SshCommand> getSshRequest)
        {
            //ISsh ssh= this.ConfigureSsh(request);
            var asyncHandle = new SshRequestAsyncHandle();
            var action =
                (Action<SshApiResponse>) (r => ProcessResponse(request, r, asyncHandle, callback));
            //if (this.UseSynchronizationContext && SynchronizationContext.Current != null)
            //{
            //    SynchronizationContext ctx = SynchronizationContext.Current;
            //    Action<HttpResponse> cb = action;
            //    action = (Action<HttpResponse>)(resp => ctx.Post((SendOrPostCallback)(s => cb(resp)), (object)null));
            //}
            asyncHandle.SshRequest = getSshRequest(Client, request, action);
            return asyncHandle;
        }

        private void ProcessResponse(ISshRequest request, SshApiResponse sshApiResponse,
            SshRequestAsyncHandle asyncHandle, Action<ISshApiResponse, SshRequestAsyncHandle> callback)
        {
            // SshApiResponse restResponse = ConvertToRestResponse(request, httpResponse);
            //var sshApiResponse = httpResponse;
            sshApiResponse.Request = request;

            callback(sshApiResponse, asyncHandle);
        }


        //SshCommand cmd,
        private SshCommand DoAsGetAsync(SshClient ssh, ISshRequest request, Action<SshApiResponse> responseCb)
        {
            var cmd = ssh.CreateCommand(request.CommandText);

            var asyncResult =
                cmd.BeginExecute(result => { ResponseCallback(result, responseCb, cmd); },
                    cmd);
            var b = asyncResult.IsCompleted;
            return cmd;
        }

        private void ResponseCallback(IAsyncResult result, Action<SshApiResponse> callback, SshCommand sshCommand)
        {
            var response = new SshApiResponse
            {
                ResponseStatus = ResponseStatus.None
            };
            //var str = string.Empty;
            try
            {
                //WaitHandle.WaitAny(new[] {result.AsyncWaitHandle});


                //var cmd = (SshCommand) result.AsyncState;

                //str = cmd.EndExecute(result);

                //var b = result.IsCompleted;

                //apiResponse.Content = str;
                using (sshCommand)
                {
                    ExtractResponseData(response, sshCommand);
                    PopulateErrorForIncompleteResponse(response, sshCommand);
                }
            }
            catch (Exception e)
            {
                response = ResponseCallbackError(e);
            }

            callback(response);
        }

        private void ExecuteCallback(SshApiResponse apiResponse, Action<SshApiResponse> callback)
        {
            callback(apiResponse);
        }

        private SshApiResponse ResponseCallbackError(Exception e)
        {
            return CreateErrorResponse(e);
        }

        private SshApiResponse CreateErrorResponse(Exception ex)
        {
            var sshResponse = new SshApiResponse();
            //WebException webException;
            //if ((webException = ex as WebException) != null && webException.Status == WebExceptionStatus.RequestCanceled)
            //{
            //    httpResponse.ResponseStatus = this.timeoutState.TimedOut ? ResponseStatus.TimedOut : ResponseStatus.Aborted;
            //    return httpResponse;
            //}
            sshResponse.ErrorMessage = ex.Message;
            sshResponse.ErrorException = ex;
            sshResponse.ResponseStatus = ResponseStatus.Error;
            return sshResponse;
        }

        private void ExtractResponseData(SshApiResponse apiResponse, SshCommand sshCommand)
        {
            apiResponse.Content = sshCommand.Result;
            apiResponse.ResponseStatus = ResponseStatus.Completed;
            apiResponse.StatusCode = sshCommand.ExitStatus;
        }

        private void PopulateErrorForIncompleteResponse(SshApiResponse apiResponse, SshCommand sshCommand)
        {
            if (sshCommand.ExitStatus > 0)
            {
                apiResponse.ResponseStatus = ResponseStatus.Error;
                apiResponse.ErrorMessage = sshCommand.Error;
                apiResponse.ExitStatus = sshCommand.ExitStatus;
            }

            //apiResponse.ErrorException = (Exception)apiResponse.ResponseStatus.ToWebException();
            //apiResponse.ErrorMessage = apiResponse.ErrorException.Message;
        }
    }
}