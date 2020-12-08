using System;
using System.Collections.Generic;
using System.Text;
using Doods.Framework.Ssh.Std.Queries;

namespace Doods.Framework.Ssh.Std.Requests
{
    public class InstallPackagesRequest : SshRequestBase
    {
        public const string RequestString = "apt-get install -y ";
        public InstallPackagesRequest(IEnumerable<string> packages) : base($"{RequestString} {string.Join(" ", packages)}")
        {
            NeedGroup = new List<string>() { "root", "sudo" };
            UseSudo = true;
        }
    }
}
