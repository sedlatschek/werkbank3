using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.exceptions;
using werkbank.repositories;

namespace werkbank.converters
{
    public class EnvironmentConverter : JsonConverter<environments.Environment>
    {
        public override environments.Environment ReadJson(JsonReader reader, Type objectType, environments.Environment? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value != null)
            {
                string handle = (string)reader.Value;
                return EnvironmentRepository.ByHandle(handle);
            }
            throw new WerkDeserializationException("Can not deserialize environment from null");
        }

        public override void WriteJson(JsonWriter writer, environments.Environment? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                throw new WerkSerializationException("Can not serialize environment without value");

            }
            writer.WriteValue(value.Handle);
        }
    }
}
