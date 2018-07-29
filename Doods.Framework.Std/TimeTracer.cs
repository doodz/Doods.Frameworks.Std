using System;
using Doods.Framework.Std.Extensions;

namespace Doods.Framework.Std
{
    public class TimeTracer : IDisposable
    {
        private readonly ILogger _logger;
        private readonly Action<ILogger, string> _logging;

        public TimeTracer(ILogger logger, ITimeWatcher timer = null, Action<ILogger, string> logging = null)
        {
            Timer = timer ?? new TimeWatcher();
            _logger = logger;
            _logging = logging ?? ((l, s) => l.Info(s));
        }

        public ITimeWatcher Timer { get; }

        public void Dispose()
        {
            if (Timer != null)
            {
                _logging(_logger, $"{Timer?.Traces.Join("\n")}");
            }
        }
    }
}