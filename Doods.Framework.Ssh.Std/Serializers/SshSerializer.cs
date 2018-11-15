using System;
using System.Collections.Generic;
using System.Text;
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

        public T Deserialize<T>(ISshResponse response)
        {
            return Deserialize<T>(response.Content);
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