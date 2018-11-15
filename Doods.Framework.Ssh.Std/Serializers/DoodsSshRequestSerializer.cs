using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Doods.Framework.Std.Utilities;

namespace Doods.Framework.Ssh.Std.Serializers
{
    public class DoodsSshRequestSerializer
    {
        internal IContractResolver _contractResolver;
        internal Collection<SshConverter> _converters;

        public DoodsSshRequestSerializer()
        {
            _contractResolver = DefaultContractResolver.Instance;
        }

        public virtual Collection<SshConverter> Converters
        {
            get
            {
                if (_converters == null) _converters = new Collection<SshConverter>();

                return _converters;
            }
        }

        public virtual IContractResolver ContractResolver
        {
            get => _contractResolver;
            set => _contractResolver = value ?? DefaultContractResolver.Instance;
        }

        public static DoodsSshRequestSerializer CreateDefault(SshSerializerSettings settings)
        {
            var serializer = CreateDefault();
            if (settings != null) ApplySerializerSettings(serializer, settings);

            return serializer;
        }

        private static void ApplySerializerSettings(DoodsSshRequestSerializer serializer,
            SshSerializerSettings settings)
        {
            if (!CollectionUtils.IsNullOrEmpty(settings.Converters))
                for (var i = 0; i < settings.Converters.Count; i++)
                    serializer.Converters.Insert(i, settings.Converters[i]);
        }

        public static DoodsSshRequestSerializer CreateDefault()
        {
            return new DoodsSshRequestSerializer();
        }

        public T Deserialize<T>(string value)
        {
            return (T) Deserialize(value, typeof(T));
        }

        public object Deserialize(string value, Type objectType)
        {
            return DeserializeInternal(value, objectType);
        }

        internal virtual object DeserializeInternal(string reader, Type objectType)
        {
            ValidationUtils.ArgumentNotNull(reader, nameof(reader));


            var serializerReader = new DoodsSshRequestSerializerInternalReader(this);
            var value = serializerReader.Deserialize(reader, objectType);


            return value;
        }


        internal SshConverter GetMatchingConverter(Type type)
        {
            return GetMatchingConverter(_converters, type);
        }

        internal static SshConverter GetMatchingConverter(IList<SshConverter> converters, Type objectType)
        {
#if DEBUG
            ValidationUtils.ArgumentNotNull(objectType, nameof(objectType));
#endif

            if (converters != null)
                for (var i = 0; i < converters.Count; i++)
                {
                    var converter = converters[i];

                    if (converter.CanConvert(objectType)) return converter;
                }

            return null;
        }
    }
}