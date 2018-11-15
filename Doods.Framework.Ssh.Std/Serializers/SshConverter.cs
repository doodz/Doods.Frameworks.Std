using System;

namespace Doods.Framework.Ssh.Std.Serializers
{
    public abstract class SshConverter
    {
        public abstract bool CanConvert(Type objectType);
        public abstract object Read(string reader, Type objectType);
    }
    public abstract class SshConverter<T> : SshConverter
    {
        public sealed override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }
    }
}