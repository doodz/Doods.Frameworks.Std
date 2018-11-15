using Doods.Framework.ApiClientBase.Std.Authentication;

namespace Doods.Framework.ApiClientBase.Std.Models
{
    public abstract class ConnectionBase: IConnection
{
        /// <summary>Gets connection port.</summary>
        public int Port { get; private set; }
        /// <summary>Gets connection host.</summary>
        public string Host { get; private set; }
        public Credentials Credentials { get; private set; }
        public ConnectionType ConnectionType { get; protected set; }
        protected ConnectionBase(string host, int port, Credentials credentials)
        {
            Host = host;
            Port = port;
            Credentials = credentials;
        }
    }
}
