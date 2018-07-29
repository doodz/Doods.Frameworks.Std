using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doods.Framework.Std
{
    public interface ITimeWatcher
    {
        string[] Traces { get; }
        void Start();

        void Stop();

        IWatcher StartWatcher(string name);

        Task<IWatcher> StartWatcherAsync(string name);

        void StopWatcher(string key, Dictionary<string, object> descriptions);
    }
}