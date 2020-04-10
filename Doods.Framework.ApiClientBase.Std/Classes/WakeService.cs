using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Doods.Framework.ApiClientBase.Std.Classes
{
    public class WakeService : IWakeService
    {
        public Task Wake(byte[] mac)
        {
            return IPAddress.Broadcast.SendWolAsync(mac, 40000);
        }
    }

  
}