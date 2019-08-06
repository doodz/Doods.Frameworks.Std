namespace Doods.Framework.Ssh.Std.Requests
{
    public class InterfacesRequest : SshRequestBase
    {
        public const string RequestString = "ls -1 /sys/class/net";
        public InterfacesRequest() : base(RequestString)
        {

        }
    }
}