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
}