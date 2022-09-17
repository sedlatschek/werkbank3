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
    public class WriteTest
    {
        [TestMethod]
        public void PerformWorks()
        {
            string file = Util.GetTempPath() + ".txt";
            Write.Perform(new OperationWriteOptions(file, "hi"));

            Assert.IsTrue(File.Exists(file));
            Assert.AreEqual("hi", File.ReadAllText(file));
        }

        [TestMethod]
        public void VerifyWorks()
        {
            string file = Util.GetTempPath() + ".txt";
            OperationWriteOptions options = new(file, "hi");
            Write.Perform(options);

            Assert.IsTrue(Write.Verify(options));

            File.WriteAllText(file, "ho");
            Assert.IsFalse(Write.Verify(options));

            File.Delete(file);
            Assert.IsFalse(Write.Verify(options));
        }
    }
}
