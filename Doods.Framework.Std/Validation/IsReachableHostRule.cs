using System;
using System.Net;
using System.Net.NetworkInformation;

namespace Doods.Framework.Std.Validation
{
    public class IsReachableHostRule<T> : IValidationRule<T>
    {
        public Func<string, string> ValueFormater { get; set; }
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            var p = new Ping();
            var ip = value as string;
            try
            {
                var request =
                    (HttpWebRequest) WebRequest.Create(ValueFormater.Invoke(ip));
                var response = (HttpWebResponse) request.GetResponse();
                return response.StatusCode == HttpStatusCode.OK;

                //var ipa = IPAddress.Parse(ip);
                //var rep = p.Send(ipa, 1000);
                //if (rep.Status != IPStatus.Success)
                //    return false;
            }
            catch (Exception ex)
            {
                ValidationMessage = ex.Message;
                return false;
            }
        }
    }
}