namespace Doods.Framework.Ssh.Std.Queries
{
    public class CpuInfoRequest : SshRequestBase
    {
        public const string RequestString = "lscpu";
        public CpuInfoRequest():base(RequestString)
        {
            
        }
       
       
    }
}