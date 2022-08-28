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
        public void TestPerformWorksForDir()
        {
            string dir = Util.GetTempPath();
            Directory.CreateDirectory(dir);

            Hide.Perform(new OperationHideOptions(dir));

            Assert.IsTrue(File.GetAttributes(dir).HasFlag(FileAttributes.Hidden));

            Unhide.Perform(new OperationUnhideOptions(dir));

            Assert.IsFalse(File.GetAttributes(dir).HasFlag(FileAttributes.Hidden));
        }

        [TestMethod]
        public void TestPerformWorksForFile()
        {
            string file = Util.GetTempPath() + ".txt";
            File.WriteAllText(file, "haha");

            Hide.Perform(new OperationHideOptions(file));

            Assert.IsTrue(File.GetAttributes(file).HasFlag(FileAttributes.Hidden));

            Unhide.Perform(new OperationUnhideOptions(file));

            Assert.IsFalse(File.GetAttributes(file).HasFlag(FileAttributes.Hidden));
        }

        [TestMethod]
        public void TestVerifyWorksForDir()
        {
            string dir = Util.GetTempPath();
            Directory.CreateDirectory(dir);

            FileAttributes attributes = File.GetAttributes(dir);
            File.SetAttributes(dir, attributes |= FileAttributes.Hidden);

            Assert.IsTrue(Hide.Verify(new OperationHideOptions(dir)));
            Assert.IsFalse(Unhide.Verify(new OperationUnhideOptions(dir)));

            File.SetAttributes(dir, attributes &= ~FileAttributes.Hidden);

            Assert.IsFalse(Hide.Verify(new OperationHideOptions(dir)));
            Assert.IsTrue(Unhide.Verify(new OperationUnhideOptions(dir)));
        }

        [TestMethod]
        public void TestVerifyWorksForFile()
        {
            string file = Util.GetTempPath() + ".txt";
            File.WriteAllText(file, "haha");

            FileAttributes attributes = File.GetAttributes(file);
            File.SetAttributes(file, attributes |= FileAttributes.Hidden);

            Assert.IsTrue(Hide.Verify(new OperationHideOptions(file)));
            Assert.IsFalse(Unhide.Verify(new OperationUnhideOptions(file)));

            File.SetAttributes(file, attributes &= ~FileAttributes.Hidden);

            Assert.IsFalse(Hide.Verify(new OperationHideOptions(file)));
            Assert.IsTrue(Unhide.Verify(new OperationUnhideOptions(file)));
        }
    }
}
