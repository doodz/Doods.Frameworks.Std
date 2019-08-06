
using Doods.Framework.Ssh.Std.Queries;

namespace Doods.Framework.Ssh.Std.Requests
{
    public class UpgradeAllRequest : SshRequestBase
    {
        public const string RequestString = "apt-get upgrade -y";
        public UpgradeAllRequest() : base(RequestString)
        {
            UseSudo = true;
        }
    }
}