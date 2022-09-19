using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.operations;

namespace tests.operations
{
    [TestClass]
    public class CreateDirectoryTest
    {
        [TestMethod]
        public void PerformWorks()
        {
            string dest = Util.GetTempPath();
            CreateDirectory.Perform(dest);
            Assert.IsTrue(Directory.Exists(dest));
        }

        [TestMethod]
        public void VerifyWorks()
        {
            string dest = Util.GetTempPath();

            Assert.IsFalse(Directory.Exists(dest));
            Assert.IsFalse(CreateDirectory.Verify(dest));

            CreateDirectory.Perform(dest);
            Assert.IsTrue(Directory.Exists(dest));
            Assert.IsTrue(CreateDirectory.Verify(dest));

            Directory.Delete(dest);

            Assert.IsFalse(Directory.Exists(dest));
            Assert.IsFalse(CreateDirectory.Verify(dest));
        }
    }
}
