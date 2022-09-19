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
        public void PerformWorks()
        {
            string temp = Util.GetTempPath();

            string dir = Path.Combine(temp, "dir");
            Directory.CreateDirectory(dir);
            string hi = Path.Combine(temp, "hi.txt");
            File.WriteAllText(hi, "hi");

            Assert.IsTrue(Directory.Exists(dir));
            Assert.IsTrue(File.Exists(hi));

            Delete.Perform(dir);

            Assert.IsFalse(Directory.Exists(dir));

            Delete.Perform(hi);

            Assert.IsFalse(File.Exists(hi));
        }

        [TestMethod]
        public void VerifyWorks()
        {
            string temp = Util.GetTempPath();

            string dir = Path.Combine(temp, "dir");
            Directory.CreateDirectory(dir);
            string hi = Path.Combine(temp, "hi.txt");
            File.WriteAllText(hi, "hi");

            Assert.IsFalse(Delete.Verify(dir));
            Assert.IsFalse(Delete.Verify(hi));

            Directory.Delete(dir);
            Assert.IsTrue(Delete.Verify(dir));

            File.Delete(hi);
            Assert.IsTrue(Delete.Verify(hi));
        }
    }
}
