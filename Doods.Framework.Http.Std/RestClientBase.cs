using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Doods.Framework.ApiClientBase.Std.Exceptions;
using Doods.Framework.ApiClientBase.Std.Interfaces;
using Doods.Framework.Http.Std.Authentication;
using Doods.Framework.Http.Std.Serializers;
using RestSharp;

namespace Doods.Framework.Http.Std
{
    public abstract class RestClientBase : RestClient
    {
        //TODO logger
        //private readonly ILog _logger =
        //    LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected readonly IConnection Connection;

        public RestClientBase(IConnection connection) : base(connection.Host)
        {
            Connection = connection;

            var serializer = new NewtonsoftJsonSerializer();
            AddHandler("application/json", () => serializer);
            AddHandler("text/json", () => serializer);
            AddHandler("text/plain", () => serializer);
            AddHandler("text/x-json", () => serializer);
            AddHandler("text/javascript", () => serializer);
            AddHandler("*+json", () => serializer);

            var auth = new Authenticator(connection.Credentials);
            Authenticator = auth.CreatedAuthenticator;
            FollowRedirects = false;
            CookieContainer = new CookieContainer();
        }

        public async Task<IRestResponse> ExecuteAsync(IRestRequest request)
        {
            //_logger.Info($"Calling ExecuteTaskAsync. BaseUrl: {BaseUrl} Resource: {request.Resource} Parameters: {string.Join(", ", request.Parameters)}");
            AddHeaders(request);
            var response = await base.ExecuteAsync(request);
            response = await RedirectIfNeededAndGetResponseAsync(response, request).ConfigureAwait(false);
            CheckResponseStatusCode(response);
            return response;
        }

        public async Task<IRestResponse<T>> ExecuteAsync<T>(IRestRequest request)
        {
            //_logger.Info($"Calling ExecuteTaskAsync. BaseUrl: {BaseUrl} Resource: {request.Resource} Parameters: {string.Join(", ", request.Parameters)}");
            AddHeaders(request);
            var response = await base.ExecuteAsync<T>(request).ConfigureAwait(false);
            response = await RedirectIfNeededAndGetResponseAsync(response, request);
            CheckResponseStatusCode(response);
            return response;
        }

        protected abstract void AddHeaders(IRestRequest request);

        protected abstract string DeserializeError(IRestResponse response);

        private void CheckResponseStatusCode(IRestResponse response)
        {
            if (response.ErrorException != null)
                throw response.ErrorException;

            if (response.StatusCode == HttpStatusCode.Unauthorized) throw new AuthorizationException();

            if (response.StatusCode == HttpStatusCode.Forbidden) throw new ForbiddenException(response.ErrorMessage);

            if ((int)response.StatusCode >= 400)
            {
                var errorMessage = response.ErrorMessage;
                var friendly = false;
                if (response.Content != null)
                    try
                    {
                        errorMessage = DeserializeError(response);
                        friendly = true;
                    }
                    catch
                    {
                    }

                //_logger.Error($"Error in request: {response.Content}");

                throw new RequestFailedException(errorMessage, friendly);
            }
        }


        private async Task<IRestResponse<T>> RedirectIfNeededAndGetResponseAsync<T>(IRestResponse<T> response,
            IRestRequest request)
        {
            while (response.StatusCode == HttpStatusCode.Redirect)
            {
                var newLocation = GetNewLocationFromHeader(response);

                if (newLocation == null)
                    return response;

                request.Resource = RemoveBaseUrl(newLocation);
                response = await base.ExecuteAsync<T>(request).ConfigureAwait(false);
            }

            return response;
        }

        private async Task<IRestResponse> RedirectIfNeededAndGetResponseAsync(IRestResponse response,
            IRestRequest request)
        {
            while (response.StatusCode == HttpStatusCode.Redirect)
            {
                var newLocation = GetNewLocationFromHeader(response);

                if (newLocation == null)
                    return response;

                request.Resource = RemoveBaseUrl(newLocation);
                response = await base.ExecuteAsync(request).ConfigureAwait(false);
            }

            return response;
        }


        private static Parameter GetNewLocationFromHeader(IRestResponse response)
        {
            return response.Headers
                .FirstOrDefault(x => x.Type == ParameterType.HttpHeader &&
                                     x.Name.Equals(HttpResponseHeader.Location.ToString(),
                                         StringComparison.InvariantCultureIgnoreCase));
        }

        private string RemoveBaseUrl(Parameter newLocation)
        {
            return newLocation.Value.ToString().Replace(Connection.Host.ToString(), string.Empty);
        }
    }
}