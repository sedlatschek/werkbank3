using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO.Compression;
using werkbank.operations;
using ICSharpCode.SharpZipLib.GZip;

namespace tests.operations
{
    [TestClass]
    public class UnzipTest
    {
        [TestMethod]
        public void PerformWorks()
        {
            string source = Util.GetTempPath();
            string dest = Util.GetTempPath();
            Directory.CreateDirectory(source);
            string hi = Path.Combine(source, "hi.txt");
            File.WriteAllText(hi, "hi");
            DirectoryInfo subDir = Directory.CreateDirectory(Path.Combine(source, "subdir"));
            DirectoryInfo subSubDir = Directory.CreateDirectory(Path.Combine(subDir.FullName, "x"));
            string ho = Path.Combine(subDir.FullName, "ho.txt");
            File.WriteAllText(ho, "ho");
            string x = Path.Combine(subSubDir.FullName, "x.txt");
            File.WriteAllText(x, "XX");

            string zip = Util.GetTempPath() + ".zip";

            // we trust the zip operation, as it is tested independetly
            Zip.Perform(new OperationZipOptions(source, zip));
            Assert.IsTrue(File.Exists(zip));
            Assert.IsTrue(Zip.Verify(new OperationZipOptions(source, zip)));

            Unzip.Perform(new OperationUnzipOptions(zip, dest));

            Assert.IsTrue(File.Exists(hi));
            Assert.IsTrue(File.Exists(ho));
            Assert.IsTrue(File.Exists(x));
        }

        [TestMethod]
        public void VerifyWorks()
        {
            string source = Util.GetTempPath();
            string dest = Util.GetTempPath();
            Directory.CreateDirectory(source);
            string hi = Path.Combine(source, "hi.txt");
            File.WriteAllText(hi, "hi");
            DirectoryInfo subDir = Directory.CreateDirectory(Path.Combine(source, "subdir"));
            DirectoryInfo subSubDir = Directory.CreateDirectory(Path.Combine(subDir.FullName, "x"));
            string ho = Path.Combine(subDir.FullName, "ho.txt");
            File.WriteAllText(ho, "ho");
            string x = Path.Combine(subSubDir.FullName, "x.txt");
            File.WriteAllText(x, "XX");

            string zip = Util.GetTempPath() + ".zip";

            // we trust the zip operation, as it is tested independetly
            Zip.Perform(new OperationZipOptions(source, zip));
            Assert.IsTrue(File.Exists(zip));
            Assert.IsTrue(Zip.Verify(new OperationZipOptions(source, zip)));

            Unzip.Perform(new OperationUnzipOptions(zip, dest));
            Assert.IsTrue(Unzip.Verify(new OperationUnzipOptions(zip, dest)));

            string destHi = Path.Combine(dest, "hi.txt");
            File.Delete(destHi);
            Assert.IsFalse(Unzip.Verify(new OperationUnzipOptions(zip, dest)));

            File.WriteAllText(destHi, "hi");
            Assert.IsTrue(Unzip.Verify(new OperationUnzipOptions(zip, dest)));

            File.WriteAllText(destHi, "This text was changed!");
            Assert.IsFalse(Unzip.Verify(new OperationUnzipOptions(zip, dest)));
        }
    }
}
