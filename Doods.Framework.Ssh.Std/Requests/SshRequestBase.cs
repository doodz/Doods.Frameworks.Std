using Doods.Framework.Ssh.Std.Interfaces;
using Doods.Framework.Ssh.Std.Serializers;

namespace Doods.Framework.Ssh.Std.Queries
{
    public class SshRequestBase : ISshRequest
    {
        public string CommandText { get; }
        public IDeserializer Handler { get; }

        public SshRequestBase(string commandText)
        {
            CommandText = commandText;
            Handler = new SshSerializer();
        }
    }
}