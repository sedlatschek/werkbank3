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
        public void TestPerformWorks()
        {
            string source = Util.GetTempPath() + ".txt";
            File.WriteAllText(source, "hi");
            string dest = Util.GetTempPath() + ".txt";

            OperationMoveFileOptions options = new(source, dest);
            MoveFile.Perform(options);

            Assert.IsFalse(File.Exists(source));
            Assert.IsTrue(File.Exists(dest));
            Assert.AreEqual("hi", File.ReadAllText(dest));
        }

        [TestMethod]
        public void TestVerifyWorks()
        {
            string source = Util.GetTempPath() + ".txt";
            File.WriteAllText(source, "hi");
            string dest = Util.GetTempPath() + ".txt";

            OperationMoveFileOptions options = new(source, dest);
            MoveFile.Perform(options);

            Assert.IsFalse(File.Exists(source));
            Assert.IsTrue(File.Exists(dest));
            Assert.AreEqual("hi", File.ReadAllText(dest));

            Assert.IsTrue(MoveFile.Verify(options));

            File.WriteAllText(source, "hi");
            Assert.IsFalse(MoveFile.Verify(options));

            File.Delete(source);
            Assert.IsTrue(MoveFile.Verify(options));

            File.Delete(dest);
            Assert.IsFalse(MoveFile.Verify(options));
        }
    }
}
