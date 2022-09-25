using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using werkbank.exceptions;
using werkbank.repositories;

namespace werkbank.converters
{
    public class NullableEnvironmentConverter : JsonConverter<environments.Environment>
    {
        public override environments.Environment? ReadJson(JsonReader reader, Type objectType, environments.Environment? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value != null)
            {
                string handle = (string)reader.Value;
                return EnvironmentRepository.ByHandle(handle);
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, environments.Environment? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteValue(value.Handle);
            }
        }
    }
}
