namespace Doods.Framework.Ssh.Std.Requests
{


    public class CpuInfoRequest : SshRequestBase
    {
        public const string RequestString = "lscpu";
        public CpuInfoRequest() : base(RequestString)
        {

        }


    }
}