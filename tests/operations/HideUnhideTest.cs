using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using werkbank.operations;

namespace tests.operations
{
    [TestClass]
    public class HideUnhideTest
    {
        [TestMethod]
        public void PerformWorksForDir()
        {
            string dir = Util.GetTempPath();
            Directory.CreateDirectory(dir);

            Hide.Perform(dir);

            Assert.IsTrue(File.GetAttributes(dir).HasFlag(FileAttributes.Hidden));

            Unhide.Perform(dir);

            Assert.IsFalse(File.GetAttributes(dir).HasFlag(FileAttributes.Hidden));
        }

        [TestMethod]
        public void PerformWorksForFile()
        {
            string file = Util.GetTempPath() + ".txt";
            File.WriteAllText(file, "haha");

            Hide.Perform(file);

            Assert.IsTrue(File.GetAttributes(file).HasFlag(FileAttributes.Hidden));

            Unhide.Perform(file);

            Assert.IsFalse(File.GetAttributes(file).HasFlag(FileAttributes.Hidden));
        }

        [TestMethod]
        public void VerifyWorksForDir()
        {
            string dir = Util.GetTempPath();
            Directory.CreateDirectory(dir);

            FileAttributes attributes = File.GetAttributes(dir);
            File.SetAttributes(dir, attributes |= FileAttributes.Hidden);

            Assert.IsTrue(Hide.Verify(dir));
            Assert.IsFalse(Unhide.Verify(dir));

            File.SetAttributes(dir, attributes &= ~FileAttributes.Hidden);

            Assert.IsFalse(Hide.Verify(dir));
            Assert.IsTrue(Unhide.Verify(dir));
        }

        [TestMethod]
        public void VerifyWorksForFile()
        {
            string file = Util.GetTempPath() + ".txt";
            File.WriteAllText(file, "haha");

            FileAttributes attributes = File.GetAttributes(file);
            File.SetAttributes(file, attributes |= FileAttributes.Hidden);

            Assert.IsTrue(Hide.Verify(file));
            Assert.IsFalse(Unhide.Verify(file));

            File.SetAttributes(file, attributes &= ~FileAttributes.Hidden);

            Assert.IsFalse(Hide.Verify(file));
            Assert.IsTrue(Unhide.Verify(file));
        }
    }
}
