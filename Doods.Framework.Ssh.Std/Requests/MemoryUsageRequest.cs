using System;
using System.Collections.Generic;
using System.Text;

namespace Doods.Framework.Ssh.Std.Requests
{
    public class MemoryUsageRequest : SshRequestBase
    {
        public const string RequestString = "cat /proc/meminfo";
        public MemoryUsageRequest() : base(RequestString)
        {

        }
    }
}
