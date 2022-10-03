using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO.Compression;
using werkbank.operations;
using werkbank.models;
using werkbank.repositories;
using werkbank.transitions;

namespace tests.operations
{
    [TestClass]
    public class ZipTest
    {
        [TestMethod]
        public void PerformWorks()
        {
            string source = Util.GetTempPath();
            Directory.CreateDirectory(source);
            File.WriteAllText(Path.Combine(source, "hi.txt"), "hi");
            DirectoryInfo subDir = Directory.CreateDirectory(Path.Combine(source, "subdir"));
            DirectoryInfo subSubDir = Directory.CreateDirectory(Path.Combine(subDir.FullName, "x"));
            File.WriteAllText(Path.Combine(subDir.FullName, "ho.txt"), "ho");
            File.WriteAllText(Path.Combine(subSubDir.FullName, "x.txt"), "XX");

            string zip = Util.GetTempPath() + ".zip";

            Zip.Perform(source, zip);
            Assert.IsTrue(File.Exists(zip));

            List<string> contents = GetZipContents(zip);

            Assert.AreEqual(5, contents.Count); // 2 dirs + 3 files = 5 entries
            Assert.IsTrue(contents.Contains("hi.txt"));
            Assert.IsTrue(contents.Contains("subdir/ho.txt"));
            Assert.IsTrue(contents.Contains("subdir/x/x.txt"));
        }

        private static List<string> GetZipContents(string FilePath)
        {
            List<string> contents = new();
            using (var archive = ZipFile.OpenRead(FilePath))
            {
                foreach (var entry in archive.Entries)
                {
                    contents.Add(entry.FullName);
                }
            }
            return contents;
        }

        [TestMethod]
        public void VerifyWorks()
        {
            DirectoryInfo source = Directory.CreateDirectory(Util.GetTempPath());
            DirectoryInfo subDir = Directory.CreateDirectory(Path.Combine(source.FullName, "subdir"));
            DirectoryInfo subSubDir = Directory.CreateDirectory(Path.Combine(subDir.FullName, "x"));
            DirectoryInfo subSubDir2 = Directory.CreateDirectory(Path.Combine(subDir.FullName, "y"));
            DirectoryInfo subSubDir3 = Directory.CreateDirectory(Path.Combine(subDir.FullName, "z"));
            File.WriteAllText(Path.Combine(subSubDir.FullName, "ha.txt"), "ha");
            File.WriteAllText(Path.Combine(subSubDir2.FullName, "he.txt"), "he");
            File.WriteAllText(Path.Combine(subSubDir3.FullName, "hi.txt"), "hi");

            string zip = Util.GetTempPath() + ".zip";
            Zip.Perform(source.FullName, zip);

            Assert.IsTrue(File.Exists(zip));
            Assert.IsTrue(Zip.Verify(source.FullName, zip));
        }

        [TestMethod]
        [ExpectedException(typeof(werkbank.exceptions.VerificationException))]
        public void VerifyThrowsExceptionIfDirectoryIsDeleted()
        {
            DirectoryInfo source = Directory.CreateDirectory(Util.GetTempPath());
            DirectoryInfo subDir = Directory.CreateDirectory(Path.Combine(source.FullName, "subdir"));
            DirectoryInfo subSubDir = Directory.CreateDirectory(Path.Combine(subDir.FullName, "x"));
            DirectoryInfo subSubDir2 = Directory.CreateDirectory(Path.Combine(subDir.FullName, "y"));
            DirectoryInfo subSubDir3 = Directory.CreateDirectory(Path.Combine(subDir.FullName, "z"));
            File.WriteAllText(Path.Combine(subSubDir.FullName, "ha.txt"), "ha");
            File.WriteAllText(Path.Combine(subSubDir2.FullName, "he.txt"), "he");
            File.WriteAllText(Path.Combine(subSubDir3.FullName, "hi.txt"), "hi");

            string zip = Util.GetTempPath() + ".zip";
            Zip.Perform(source.FullName, zip);

            Assert.IsTrue(File.Exists(zip));

            subSubDir2.Delete(true);

            Zip.Verify(source.FullName, zip);
        }

        [TestMethod]
        [ExpectedException(typeof(werkbank.exceptions.VerificationException))]
        public void VerifyThrowsExceptionIfFileIsManipulated()
        {
            DirectoryInfo source = Directory.CreateDirectory(Util.GetTempPath());
            DirectoryInfo subDir = Directory.CreateDirectory(Path.Combine(source.FullName, "subdir"));
            DirectoryInfo subSubDir = Directory.CreateDirectory(Path.Combine(subDir.FullName, "x"));
            DirectoryInfo subSubDir2 = Directory.CreateDirectory(Path.Combine(subDir.FullName, "y"));
            DirectoryInfo subSubDir3 = Directory.CreateDirectory(Path.Combine(subDir.FullName, "z"));
            File.WriteAllText(Path.Combine(subSubDir.FullName, "ha.txt"), "ha");
            File.WriteAllText(Path.Combine(subSubDir2.FullName, "he.txt"), "he");
            File.WriteAllText(Path.Combine(subSubDir3.FullName, "hi.txt"), "hi");

            string zip = Util.GetTempPath() + ".zip";
            Zip.Perform(source.FullName, zip);

            Assert.IsTrue(File.Exists(zip));

            File.WriteAllText(Path.Combine(subSubDir3.FullName, "hi.txt"), "hahaha");

            Zip.Verify(source.FullName, zip);
        }

        [TestMethod]
        [ExpectedException(typeof(werkbank.exceptions.VerificationException))]
        public void VerifyThrowsExceptionIfFileIsDeleted()
        {
            DirectoryInfo source = Directory.CreateDirectory(Util.GetTempPath());
            DirectoryInfo subDir = Directory.CreateDirectory(Path.Combine(source.FullName, "subdir"));
            DirectoryInfo subSubDir = Directory.CreateDirectory(Path.Combine(subDir.FullName, "x"));
            DirectoryInfo subSubDir2 = Directory.CreateDirectory(Path.Combine(subDir.FullName, "y"));
            DirectoryInfo subSubDir3 = Directory.CreateDirectory(Path.Combine(subDir.FullName, "z"));
            File.WriteAllText(Path.Combine(subSubDir.FullName, "ha.txt"), "ha");
            File.WriteAllText(Path.Combine(subSubDir2.FullName, "he.txt"), "he");
            File.WriteAllText(Path.Combine(subSubDir3.FullName, "hi.txt"), "hi");

            string zip = Util.GetTempPath() + ".zip";
            Zip.Perform(source.FullName, zip);

            Assert.IsTrue(File.Exists(zip));

            File.Delete(Path.Combine(subSubDir.FullName, "ha.txt"));

            Zip.Verify(source.FullName, zip);
        }

        [TestMethod]
        [ExpectedException(typeof(werkbank.exceptions.VerificationException))]
        public void VerifyThrowsExceptionIfFileIsMissing()
        {
            DirectoryInfo source = Directory.CreateDirectory(Util.GetTempPath());
            DirectoryInfo subDir = Directory.CreateDirectory(Path.Combine(source.FullName, "subdir"));
            DirectoryInfo subSubDir = Directory.CreateDirectory(Path.Combine(subDir.FullName, "x"));
            DirectoryInfo subSubDir2 = Directory.CreateDirectory(Path.Combine(subDir.FullName, "y"));
            DirectoryInfo subSubDir3 = Directory.CreateDirectory(Path.Combine(subDir.FullName, "z"));
            File.WriteAllText(Path.Combine(subSubDir.FullName, "ha.txt"), "ha");
            File.WriteAllText(Path.Combine(subSubDir2.FullName, "he.txt"), "he");
            File.WriteAllText(Path.Combine(subSubDir3.FullName, "hi.txt"), "hi");

            string zip = Util.GetTempPath() + ".zip";
            Zip.Perform(source.FullName, zip);

            Assert.IsTrue(File.Exists(zip));

            File.WriteAllText(Path.Combine(subDir.FullName, "i-am-new.txt"), "NEWNEWNEW");

            Zip.Verify(source.FullName, zip);
        }

        [TestMethod]
        [ExpectedException(typeof(werkbank.exceptions.VerificationException))]
        public void VerifyThrowsExceptionIfDirIsMissing()
        {
            DirectoryInfo source = Directory.CreateDirectory(Util.GetTempPath());
            DirectoryInfo subDir = Directory.CreateDirectory(Path.Combine(source.FullName, "subdir"));
            DirectoryInfo subSubDir = Directory.CreateDirectory(Path.Combine(subDir.FullName, "x"));
            DirectoryInfo subSubDir2 = Directory.CreateDirectory(Path.Combine(subDir.FullName, "y"));
            DirectoryInfo subSubDir3 = Directory.CreateDirectory(Path.Combine(subDir.FullName, "z"));
            File.WriteAllText(Path.Combine(subSubDir.FullName, "ha.txt"), "ha");
            File.WriteAllText(Path.Combine(subSubDir2.FullName, "he.txt"), "he");
            File.WriteAllText(Path.Combine(subSubDir3.FullName, "hi.txt"), "hi");

            string zip = Util.GetTempPath() + ".zip";
            Zip.Perform(source.FullName, zip);

            Assert.IsTrue(File.Exists(zip));

            Directory.CreateDirectory(Path.Combine(subSubDir.FullName, "d"));

            Zip.Verify(source.FullName, zip);
        }
    }
}
