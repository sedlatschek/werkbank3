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
    public class MoveFileTest
    {
        [TestMethod]
        public void PerformWorks()
        {
            string source = Util.GetTempPath() + ".txt";
            File.WriteAllText(source, "hi");
            string dest = Util.GetTempPath() + ".txt";

            MoveFile.Perform(source, dest);

            Assert.IsFalse(File.Exists(source));
            Assert.IsTrue(File.Exists(dest));
            Assert.AreEqual("hi", File.ReadAllText(dest));
        }

        [TestMethod]
        public void VerifyWorks()
        {
            string source = Util.GetTempPath() + ".txt";
            File.WriteAllText(source, "hi");
            string dest = Util.GetTempPath() + ".txt";

            MoveFile.Perform(source, dest);

            Assert.IsFalse(File.Exists(source));
            Assert.IsTrue(File.Exists(dest));
            Assert.AreEqual("hi", File.ReadAllText(dest));

            Assert.IsTrue(MoveFile.Verify(source, dest));

            File.WriteAllText(source, "hi");
            Assert.IsFalse(MoveFile.Verify(source, dest));

            File.Delete(source);
            Assert.IsTrue(MoveFile.Verify(source, dest));

            File.Delete(dest);
            Assert.IsFalse(MoveFile.Verify(source, dest));
        }
    }
}
