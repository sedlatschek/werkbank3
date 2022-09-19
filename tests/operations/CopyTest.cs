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
    public class CopyTest
    {
        [TestMethod]
        public void PerformWorksForDir()
        {
            string source = Util.GetTempPath();
            Directory.CreateDirectory(source);
            File.WriteAllText(Path.Combine(source, "hi.txt"), "hi");
            DirectoryInfo subDir = Directory.CreateDirectory(Path.Combine(source, "subdir"));
            File.WriteAllText(Path.Combine(subDir.FullName, "ho.txt"), "ho");
            string dest = Util.GetTempPath();

            Copy.Perform(source, dest);

            string hi = Path.Combine(dest, "hi.txt");
            string ho = Path.Combine(dest, "subdir", "ho.txt");

            Assert.IsTrue(File.Exists(hi));
            Assert.AreEqual(File.ReadAllText(hi), "hi");
            Assert.IsTrue(File.Exists(ho));
            Assert.AreEqual(File.ReadAllText(ho), "ho");
        }

        [TestMethod]
        public void PerformWorksForFile()
        {
            string source = Util.GetTempPath() + ".txt";
            File.WriteAllText(source, "hi");
            string dest = Util.GetTempPath() + ".txt";

            Copy.Perform(source, dest);

            Assert.IsTrue(File.Exists(dest));
            Assert.AreEqual("hi", File.ReadAllText(dest));
        }

        [TestMethod]
        public void VerifyWorks()
        {
            string path1 = Util.GetTempPath();
            string path2 = Util.GetTempPath();

            Directory.CreateDirectory(path1);
            Directory.CreateDirectory(path2);

            File.WriteAllText(Path.Combine(path1, "hi.txt"), "hi");
            File.WriteAllText(Path.Combine(path2, "hi.txt"), "hi");

            DirectoryInfo subDir1 = Directory.CreateDirectory(Path.Combine(path1, "subdir"));
            DirectoryInfo subDir2 = Directory.CreateDirectory(Path.Combine(path2, "subdir"));

            File.WriteAllText(Path.Combine(subDir1.FullName, "ho.txt"), "ho");
            File.WriteAllText(Path.Combine(subDir2.FullName, "ho.txt"), "ho");

            Assert.IsTrue(Copy.Verify(path1, path2));

            File.WriteAllText(Path.Combine(subDir2.FullName, "ho.txt"), "I changed this");

            Assert.IsFalse(Copy.Verify(path1, path2));

            File.WriteAllText(Path.Combine(subDir1.FullName, "ho.txt"), "I changed this");

            Assert.IsTrue(Copy.Verify(path1, path2));

            Directory.CreateDirectory(Path.Combine(path1, "somedir"));

            Assert.IsFalse(Copy.Verify(path1, path2));

            Directory.Delete(Path.Combine(path1, "somedir"));

            Assert.IsTrue(Copy.Verify(path1, path2));

            Directory.CreateDirectory(Path.Combine(path2, "somedir"));

            Assert.IsFalse(Copy.Verify(path1, path2));

            Directory.Delete(Path.Combine(path2, "somedir"));

            Assert.IsTrue(Copy.Verify(path1, path2));

            File.Delete(Path.Combine(path1, "hi.txt"));

            Assert.IsFalse(Copy.Verify(path1, path2));

            File.Delete(Path.Combine(path2, "hi.txt"));

            Assert.IsTrue(Copy.Verify(path1, path2));

            File.Delete(Path.Combine(subDir2.FullName, "ho.txt"));

            Assert.IsFalse(Copy.Verify(path1, path2));
        }

        [TestMethod]
        public void VerifyWorksForFile()
        {
            string source = Util.GetTempPath() + ".txt";
            File.WriteAllText(source, "hi");
            string dest = Util.GetTempPath() + ".txt";

            Copy.Perform(source, dest);

            Assert.IsTrue(Copy.Verify(source, dest));

            File.Delete(dest);
            Assert.IsFalse(Copy.Verify(source, dest));

            File.WriteAllText(dest, "hi");
            Assert.IsTrue(Copy.Verify(source, dest));

            File.WriteAllText(dest, "ho");
            Assert.IsFalse(Copy.Verify(source, dest));
        }
    }
}
