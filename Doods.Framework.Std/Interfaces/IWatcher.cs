using System;
using System.Collections.Generic;

namespace Doods.Framework.Std
{
    public interface IWatcher : IDisposable
    {
        string Key { get; }

        Dictionary<string, object> Descriptions { get; }
    }
}