using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tests
{
    [TestClass]
    public class ConfigTest
    {
        [TestMethod]
        public void EnvironmentIsDetected()
        {
            Assert.IsTrue(werkbank.Config.IsTestEnvironment);
        }
    }
}
