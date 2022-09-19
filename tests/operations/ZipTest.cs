using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO.Compression;
using werkbank.operations;

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

            Assert.AreEqual(3, contents.Count);
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
            string source = Util.GetTempPath();
            Directory.CreateDirectory(source);
            File.WriteAllText(Path.Combine(source, "hi.txt"), "hi");
            DirectoryInfo subDir = Directory.CreateDirectory(Path.Combine(source, "subdir"));
            DirectoryInfo subSubDir = Directory.CreateDirectory(Path.Combine(subDir.FullName, "x"));
            string ho = Path.Combine(subDir.FullName, "ho.txt");
            File.WriteAllText(ho, "ho");
            string x = Path.Combine(subSubDir.FullName, "x.txt");
            File.WriteAllText(x, "XX");

            string zip = Util.GetTempPath() + ".zip";
            Zip.Perform(source, zip);

            Assert.IsTrue(File.Exists(zip));
            Assert.IsTrue(Zip.Verify(source, zip));

            string y = Path.Combine(subDir.FullName, "y");
            File.WriteAllText(y, "hoy");
            Assert.IsFalse(Zip.Verify(source, zip));

            File.Delete(y);
            Assert.IsTrue(Zip.Verify(source, zip));

            File.WriteAllText(ho, "This text was changed!");
            Assert.IsFalse(Zip.Verify(source, zip));

            File.WriteAllText(ho, "ho");
            Assert.IsTrue(Zip.Verify(source, zip));

            File.Delete(x);
            Assert.IsFalse(Zip.Verify(source, zip));
        }
    }
}
