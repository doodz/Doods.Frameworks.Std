using System;
using Doods.Framework.Ssh.Std.Interfaces;

namespace Doods.Framework.Ssh.Std.Serializers
{
    public class SshSerializer : IDeserializer
    {
        private readonly DoodsSshRequestSerializer _serializer;


        public SshSerializer()
        {
            _serializer = new DoodsSshRequestSerializer();
        }

        public SshSerializer(SshSerializerSettings settings)
        {
            _serializer = DoodsSshRequestSerializer.CreateDefault(settings);
        }

        public T Deserialize<T>(IApiResponse apiResponse)
        {
            return Deserialize<T>(apiResponse.Content);
        }

        public T DeserializeError<T>(IApiResponse apiResponse)
        {
            return Deserialize<T>(apiResponse.ErrorMessage);
        }

        public T Deserialize<T>(string content)
        {
            try
            {
                return _serializer.Deserialize<T>(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Logger.Error($"Couldn't deserialize json: {json}. Error: {ex}");
                throw;
            }
        }
    }
}