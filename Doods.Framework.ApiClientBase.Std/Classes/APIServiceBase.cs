using System;
using System.Collections.Generic;
using System.Text;
using Doods.Framework.ApiClientBase.Std.Interfaces;
using Doods.Framework.ApiClientBase.Std.Models;

namespace Doods.Framework.ApiClientBase.Std.Classes
{
    public abstract class APIServiceBase
    {
        protected IConnection Connection;
     
        public bool CanConnect()
        {
            return Connection != null;
        }
    }
}
