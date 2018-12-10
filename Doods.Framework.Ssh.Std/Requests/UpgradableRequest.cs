namespace Doods.Framework.Ssh.Std.Queries
{
    public class UpgradableRequest : SshRequestBase
    {
        public const string RequestString = "apt list --upgradable";
        public UpgradableRequest():base(RequestString)
        {
            
        }
       
       
    }
}