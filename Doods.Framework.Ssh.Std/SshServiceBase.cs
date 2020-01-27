using System;
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


        public Task<ISshResponse<T>> ExecuteTaskAsync<T>(ISshRequest request)
        {
            return ExecuteTaskAsync<T>(request, CancellationToken.None);
        }

        public Task<ISshResponse<T>> ExecuteTaskAsync<T>(ISshRequest request, CancellationToken token)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            var taskCompletionSource = new TaskCompletionSource<ISshResponse<T>>();
            try
            {
                var async = ExecuteAsync(request,
                    (Action<ISshResponse<T>, SshRequestAsyncHandle>) ((response, _) =>
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

        protected virtual SshClient GetSshClient()
        {
            var test =
                new PasswordConnectionInfo(Connection.Host, Connection.Port, Connection.Credentials.Login,
                    Connection.Credentials.Password) {Timeout = TimeSpan.FromSeconds(10)};
            return Client ?? (Client = new SshClient(test));
        }

        public SshRequestAsyncHandle ExecuteAsync<T>(ISshRequest request,
            Action<ISshResponse<T>, SshRequestAsyncHandle> callback)
        {
            return ExecuteAsync(request,
                (response, asyncHandle) =>
                    DeserializeResponse(request, callback, response, asyncHandle));
        }


        private void DeserializeResponse<T>(ISshRequest request,
            Action<ISshResponse<T>, SshRequestAsyncHandle> callback, ISshResponse response,
            SshRequestAsyncHandle asyncHandle)
        {
            ISshResponse<T> sshResponse1;
            try
            {
                sshResponse1 = Deserialize<T>(request, response);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                var restResponse2 = new SshResponse<T>();
                restResponse2.Request = request;
                restResponse2.ResponseStatus = ResponseStatus.Error;
                restResponse2.ErrorMessage = ex.Message;
                restResponse2.ErrorException = ex;
                sshResponse1 = restResponse2;
            }

            callback(sshResponse1, asyncHandle);
        }


        private ISshResponse<T> Deserialize<T>(ISshRequest request, ISshResponse raw)
        {
            //request.OnBeforeDeserialization(raw);
            var restResponse = (ISshResponse<T>) new SshResponse<T>();
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
            Action<ISshResponse, SshRequestAsyncHandle> callback)
        {
            return ExecuteAsync(request, callback,
                DoAsGetAsync);
        }


        private SshRequestAsyncHandle ExecuteAsync(ISshRequest request,
            Action<ISshResponse, SshRequestAsyncHandle> callback,
            Func<SshClient, ISshRequest, Action<SshResponse>, SshCommand> getSshRequest)
        {
            //ISsh ssh= this.ConfigureSsh(request);
            var asyncHandle = new SshRequestAsyncHandle();
            var action =
                (Action<SshResponse>) (r => ProcessResponse(request, r, asyncHandle, callback));
            //if (this.UseSynchronizationContext && SynchronizationContext.Current != null)
            //{
            //    SynchronizationContext ctx = SynchronizationContext.Current;
            //    Action<HttpResponse> cb = action;
            //    action = (Action<HttpResponse>)(resp => ctx.Post((SendOrPostCallback)(s => cb(resp)), (object)null));
            //}
            asyncHandle.SshRequest = getSshRequest(Client, request, action);
            return asyncHandle;
        }

        private void ProcessResponse(ISshRequest request, SshResponse sshResponse,
            SshRequestAsyncHandle asyncHandle, Action<ISshResponse, SshRequestAsyncHandle> callback)
        {
            // SshResponse restResponse = ConvertToRestResponse(request, httpResponse);
            //var sshResponse = httpResponse;
            sshResponse.Request = request;

            callback(sshResponse, asyncHandle);
        }


        //SshCommand cmd,
        private SshCommand DoAsGetAsync(SshClient ssh, ISshRequest request, Action<SshResponse> responseCb)
        {
            var cmd = ssh.CreateCommand(request.CommandText);

            var asyncResult =
                cmd.BeginExecute(result => { ResponseCallback(result, responseCb, cmd); },
                    cmd);
            var b = asyncResult.IsCompleted;
            return cmd;
        }

        private void ResponseCallback(IAsyncResult result, Action<SshResponse> callback, SshCommand sshCommand)
        {
            var response = new SshResponse
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

                //response.Content = str;
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

        private void ExecuteCallback(SshResponse response, Action<SshResponse> callback)
        {
            callback(response);
        }

        private SshResponse ResponseCallbackError(Exception e)
        {
            return CreateErrorResponse(e);
        }

        private SshResponse CreateErrorResponse(Exception ex)
        {
            var sshResponse = new SshResponse();
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

        private void ExtractResponseData(SshResponse response, SshCommand sshCommand)
        {
            response.Content = sshCommand.Result;
            response.ResponseStatus = ResponseStatus.Completed;
            response.StatusCode = sshCommand.ExitStatus;
        }

        private void PopulateErrorForIncompleteResponse(SshResponse response, SshCommand sshCommand)
        {
            if (sshCommand.ExitStatus > 0)
            {
                response.ResponseStatus = ResponseStatus.Error;
                response.ErrorMessage = sshCommand.Error;
                response.ExitStatus = sshCommand.ExitStatus;
            }

            //response.ErrorException = (Exception)response.ResponseStatus.ToWebException();
            //response.ErrorMessage = response.ErrorException.Message;
        }
    }
}