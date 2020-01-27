using System;

namespace Doods.Framework.ApiClientBase.Std.Exceptions
{
    public class AuthorizationException : Exception
    {
        public override string Message => "Unauthorized";
    }
}