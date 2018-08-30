using Doods.Framework.ApiClientBase.Std.Authentication;

namespace Doods.Framework.ApiClientBase.Std.Models
{

    public interface IConnection
    {
        int Port { get; }
        string Host { get; }

        Credentials Credentials { get; }
        ConnectionType ConnectionType { get; }
    }

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

    public class HttpConnection: ConnectionBase
    {
        public HttpConnection(string host, int port) : base(host, port,new Credentials())
        {
            ConnectionType = ConnectionType.Http;
        }
    }

    public class SshConnection : ConnectionBase
    {
        public SshConnection(string host, int port, string username, string password):base(host,port,new Credentials(username,password))
        {
            ConnectionType = ConnectionType.Ssh;
        }


        public SshConnection(string host, string username, string password):this(host, 22, username, password)
        {
           
        }
    }
}
