namespace Doods.Framework.Ssh.Std.Requests
{
    public class AptUpdateRequest : SshRequestBase
    {
        public const string RequestString = "apt update";
        public AptUpdateRequest(bool withSudo) : base(RequestString)
        {
            UseSudo = withSudo;
        }
    }
}