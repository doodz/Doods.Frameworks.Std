using Doods.Framework.ApiClientBase.Std.Classes;
using Doods.Framework.ApiClientBase.Std.Models;
using Doods.Framework.Std;
using RestSharp;

namespace Doods.Framework.Http.Std
{
    public class HttpServiceBase : APIServiceBase
    {
        public ILogger Logger { get; }
        private RestClient _client;
        public HttpServiceBase(ILogger logger)
        {
            Logger = logger;
        }

        protected virtual IRestClient GetHttpClient()
        {
            
           return _client ?? (_client = new RestClientBase(Connection));
        }
    }
}
