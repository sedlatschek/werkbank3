using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.operations;

namespace tests.operations
{
    [TestClass]
    public class DeleteTest
    {
        [TestMethod]
        public void TestPerformWorks()
        {
            string temp = Util.GetTempPath();

            string dir = Path.Combine(temp, "dir");
            Directory.CreateDirectory(dir);
            string hi = Path.Combine(temp, "hi.txt");
            File.WriteAllText(hi, "hi");

            Assert.IsTrue(Directory.Exists(dir));
            Assert.IsTrue(File.Exists(hi));

            Delete.Perform(new OperationDeleteOptions(dir));

            Assert.IsFalse(Directory.Exists(dir));

            Delete.Perform(new OperationDeleteOptions(hi));

            Assert.IsFalse(File.Exists(hi));
        }

        [TestMethod]
        public void TestVerifyWorks()
        {
            string temp = Util.GetTempPath();

            string dir = Path.Combine(temp, "dir");
            Directory.CreateDirectory(dir);
            string hi = Path.Combine(temp, "hi.txt");
            File.WriteAllText(hi, "hi");

            Assert.IsFalse(Delete.Verify(new OperationDeleteOptions(dir)));
            Assert.IsFalse(Delete.Verify(new OperationDeleteOptions(hi)));

            Directory.Delete(dir);
            Assert.IsTrue(Delete.Verify(new OperationDeleteOptions(dir)));

            File.Delete(hi);
            Assert.IsTrue(Delete.Verify(new OperationDeleteOptions(hi)));
        }
    }
}
