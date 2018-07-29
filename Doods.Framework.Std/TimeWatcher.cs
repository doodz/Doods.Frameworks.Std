using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Doods.Framework.Std.Extensions;

namespace Doods.Framework.Std
{
    public class TimeWatcher : ITimeWatcher, IDisposable
    {
        private readonly Dictionary<string, TimeDescription> _cache;
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        private int _count;

        public TimeWatcher()
        {
            _cache = new Dictionary<string, TimeDescription>();
            Start();
        }

        public void Dispose()
        {
            if (_lock.CurrentCount != 1)
                _lock.Release();
        }

        public void Start()
        {
            if (_cache.IsNotEmpty())
            {
                Stop();
                _cache.Clear();
            }
        }

        public void Stop()
        {
            if (_count > 0 && _cache.IsNotEmpty())
                foreach (var kvp in _cache)
                    kvp.Value.Watcher.Stop();
            _count = 0;
        }

        public IWatcher StartWatcher(string name)
        {
            return StartWatcherInternal(name);
        }

        public async Task<IWatcher> StartWatcherAsync(string name)
        {
            using (this)
            {
                await _lock.WaitAsync();
                var watcher = StartWatcherInternal(name);
                return watcher;
            }
        }

        public void StopWatcher(string key, Dictionary<string, object> descriptions)
        {
            if (!_cache.ContainsKey(key)) return;

            var time = _cache[key];
            time.Watcher.Stop();

            _cache[key] = new TimeDescription(time.Watcher, time.Level, descriptions);
            _count--;
        }

        public string[] Traces => GetTraces();

        private IWatcher StartWatcherInternal(string name)
        {
            var key = FormatKey(name, DateTime.Now.Ticks);
            if (_cache.ContainsKey(key)) return null;

            _cache.Add(key, new TimeDescription(Stopwatch.StartNew(), _count));
            _count++;

            return new Watcher(this, key);
        }

        private static string FormatKey(string name, long tick)
        {
            return $"{name}:{tick}";
        }

        private string[] GetTraces()
        {
            var rslt = _cache.Select(kvp => $"{kvp.Value.FormatLevel()}{kvp.Key.Split(':')[0]}: {kvp.Value.Format()}");
            return rslt.ToArray();
        }

        private class Watcher : IWatcher
        {
            private readonly ITimeWatcher _timeWatcher;

            public Watcher(ITimeWatcher timeWatcher, string key)
            {
                _timeWatcher = timeWatcher;
                Key = key;
                Descriptions = new Dictionary<string, object>();
            }

            public Dictionary<string, object> Descriptions { get; }

            public string Key { get; }

            public void Dispose()
            {
                _timeWatcher.StopWatcher(Key, Descriptions);
            }
        }

        private class TimeDescription
        {
            public TimeDescription(Stopwatch watcher, int level, Dictionary<string, object> descriptions = null)
            {
                Watcher = watcher;
                Level = level;
                Descriptions = descriptions;
            }

            public Stopwatch Watcher { get; }

            public int Level { get; }

            public Dictionary<string, object> Descriptions { get; }

            public string Format()
            {
                var r = new List<string>();
                if (Descriptions.IsNotEmpty())
                    r.AddRange(Descriptions.Select(d => $"{d.Key}={d.Value}"));

                r.Add($"ms={Watcher.ElapsedMilliseconds}");

                return r.Join(", ");
            }

            public string FormatLevel()
            {
                var str = "";
                if (Level > 0)
                {
                    var i = 1;
                    do
                    {
                        str += "   ";
                    } while (i++ < Level);

                    str += "> ";
                }

                return str;
            }
        }
    }
}