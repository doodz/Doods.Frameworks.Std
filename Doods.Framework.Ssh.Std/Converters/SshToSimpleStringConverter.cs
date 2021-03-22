using System;
using System.Collections.Generic;
using Doods.Framework.Ssh.Std.Serializers;

namespace Doods.Framework.Ssh.Std.Converters
{
    public class SshToSimpleStringConverter : SshConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(IEnumerable<string>)) return true;

            return false;
        }

        public override object Read(string content, Type objectType)
        {
            var lines = content.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
            return lines;
        }
    }
}