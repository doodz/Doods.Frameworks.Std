using Doods.Framework.Ssh.Std.Interfaces;
using Doods.Framework.Ssh.Std.Serializers;

namespace Doods.Framework.Ssh.Std.Requests
{
    public class SshRequestBase : ISshRequest
    {

        private readonly string _commandText;
        public string CommandText => GetCommandText();
        public IDeserializer Handler
        {
            get => _SshSerializer;
        }

        protected SshSerializer _SshSerializer;
        protected bool UseSudo;
        public SshRequestBase(string commandText)
        {
            _commandText = commandText;
            _SshSerializer = new SshSerializer();
        }

        public SshRequestBase(string commandText, SshSerializerSettings settings )
        {
            _commandText = commandText;
            _SshSerializer = new SshSerializer(settings);
        }


        private string GetCommandText()
        {
            return UseSudo ? $"sudo {_commandText}" : _commandText;
        }

       

    }
}