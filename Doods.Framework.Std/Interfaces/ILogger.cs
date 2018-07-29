using System;
using System.Collections.Generic;

namespace Doods.Framework.Std
{
    public interface ILogger
    {
        void Error(string msg);

        void Error(Exception e, string type = null);

        void Warning(string msg);

        void Warning(Exception e);

        void Info(string msg);

        void Debug(string msg);

        void Event(string type, Dictionary<string, string> properties = null, Dictionary<string, double> measures = null);
    }
}
