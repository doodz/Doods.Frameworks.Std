using System.Collections.Generic;

namespace Doods.Framework.Ssh.Std.Serializers
{
    public class SshSerializerSettings
    {
        public SshSerializerSettings()
        {
            Converters = new List<ISshConverter>();
        }

        public IList<ISshConverter> Converters { get; set; }
    }
}