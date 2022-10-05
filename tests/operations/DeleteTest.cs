using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.operations;
using werkbank.services;

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
        public void PerformWorksWithSymbolicDirLink()
        {
            if (!WinApiService.IsUserAnAdmin())
            {
                Assert.Inconclusive("This test only works on administrative privileges.");
                return;
            }

            string source = Util.GetTempPath();
            string dest = Directory.CreateDirectory(Util.GetTempPath()).FullName;

            WinApiService.CreateSymbolicLink(source, dest, WinApiService.SYMBOLIC_LINK_FLAG.Directory);

            // delete dest to break the symlink
            Directory.Delete(dest);

            Delete.Perform(source);

            Assert.IsFalse(Directory.Exists(source));
        }

        [TestMethod]
        public void PerformWorksWithNestedSymbolicDirLink()
        {
            if (!WinApiService.IsUserAnAdmin())
            {
                Assert.Inconclusive("This test only works on administrative privileges.");
                return;
            }

            string source = Util.GetTempPath();
            string subDir = Directory.CreateDirectory(Path.Combine(source, "xoxo", "hi")).FullName;
            string link = Path.Combine(subDir, "ho");
            string dest = Directory.CreateDirectory(Util.GetTempPath()).FullName;

            WinApiService.CreateSymbolicLink(link, dest, WinApiService.SYMBOLIC_LINK_FLAG.Directory);

            // delete dest to break the symlink
            Directory.Delete(dest);

            Delete.Perform(source);

            Assert.IsFalse(Directory.Exists(source));
        }

        [TestMethod]
        public void PerformWorksWithSymbolicFileLink()
        {
            if (!WinApiService.IsUserAnAdmin())
            {
                Assert.Inconclusive("This test only works on administrative privileges.");
                return;
            }

            string source = Util.GetTempPath();
            string dest = Directory.CreateDirectory(Util.GetTempPath()).FullName;

            WinApiService.CreateSymbolicLink(source, dest, WinApiService.SYMBOLIC_LINK_FLAG.File);

            // delete dest to break the symlink
            Directory.Delete(dest);

            Delete.Perform(source);

            Assert.IsFalse(File.Exists(source));
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
