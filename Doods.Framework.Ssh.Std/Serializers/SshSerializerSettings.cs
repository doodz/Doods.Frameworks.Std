using System.Collections.Generic;

namespace Doods.Framework.Ssh.Std.Serializers
{
    public class SshSerializerSettings
    {
        public IList<ISshConverter> Converters { get; set; }

        public SshSerializerSettings()
        {
            Converters = new List<ISshConverter>();
        }
    }
}