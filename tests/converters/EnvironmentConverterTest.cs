using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.converters;
using werkbank.repositories;

namespace tests.converters
{
    internal class Dummy
    {
        [JsonProperty("env"), JsonConverter(typeof(EnvironmentConverter))]
        public werkbank.environments.Environment Environment = EnvironmentRepository.ByHandleOrDie("csharp");
    }

    [TestClass]
    public class EnvironmentConverterTest
    {
        [TestMethod]
        public void SerializationWorks()
        {
            Dummy dummy = new();

            Assert.AreEqual("csharp", dummy.Environment.Handle);
            string text = JsonConvert.SerializeObject(dummy);
            Assert.AreEqual("{\"env\":\"csharp\"}", text);
        }

        [TestMethod]
        public void DeserializationWorks()
        {
            string text = "{\"env\":\"csharp\"}";
            Dummy? dummy = JsonConvert.DeserializeObject<Dummy>(text);

            Assert.IsNotNull(dummy);
            Assert.AreEqual("csharp", dummy.Environment.Handle);
        }
    }
}
