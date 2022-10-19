using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using werkbank.operations;
using werkbank.services;

namespace tests.services
{
    [TestClass]
    public class FileServiceTest
    {
        [TestMethod]
        public void PathExistsWorksWithDir()
        {
            string path = Util.GetTempPath();
            Directory.CreateDirectory(path);

            Assert.IsTrue(FileService.PathExists(path, out FileService.PathType? pathType));
            Assert.AreEqual(FileService.PathType.Directory, pathType);
        }

        [TestMethod]
        public void PathExistsWorksWithFile()
        {
            string path = Util.GetTempPath() + ".txt";
            File.WriteAllText(path, "hi");

            Assert.IsTrue(FileService.PathExists(path, out FileService.PathType? pathType));
            Assert.AreEqual(FileService.PathType.File, pathType);
        }

        [TestMethod]
        public void PathExistsWorksWithFileWithoutExtension()
        {
            string path = Util.GetTempPath();
            File.WriteAllText(path, "hi");

            Assert.IsTrue(FileService.PathExists(path, out FileService.PathType? pathType));
            Assert.AreEqual(FileService.PathType.File, pathType);
        }

        [TestMethod]
        public void PathExistsWorksWithNonExistingPath()
        {
            string path = Util.GetTempPath();
            Assert.IsFalse(FileService.PathExists(path, out FileService.PathType? _));
        }

        [TestMethod]
        public void ShellDeleteFileWorks()
        {
            string file = Util.GetTempPath() + ".txt";
            File.WriteAllText(file, "hi");

            Assert.IsTrue(File.Exists(file));

            FileService.ShellDeleteFile(file);

            Assert.IsFalse(File.Exists(file));
        }

        [TestMethod]
        public void ShellDeleteDirectoryWorks()
        {
            string dir = Util.GetTempPath();
            Directory.CreateDirectory(dir);

            Assert.IsTrue(Directory.Exists(dir));

            FileService.ShellDeleteDirectory(dir);

            Assert.IsFalse(Directory.Exists(dir));
        }

        [TestMethod]
        [ExpectedException(typeof(ExternalException))]
        public void ShellDeleteDirectoryThrowsExceptionIfPathDoesNotExist()
        {
            string dir = Util.GetTempPath();
            FileService.ShellDeleteDirectory(dir);
        }

        [TestMethod]
        [ExpectedException(typeof(ExternalException))]
        public void ShellDeleteFileThrowsExceptionIfPathDoesNotExist()
        {
            string file = Util.GetTempPath();
            FileService.ShellDeleteFile(file);
        }

        [TestMethod]
        public void GetHiddenPathsWorks()
        {
            string dir = Directory.CreateDirectory(Util.GetTempPath()).FullName;
            string subDir1 = Directory.CreateDirectory(Path.Combine(dir, "subDir1")).FullName;
            string subDir1File = Path.Combine(subDir1, "file.txt");
            string subDir2 = Directory.CreateDirectory(Path.Combine(dir, "subDir2")).FullName;
            string subDir2File = Path.Combine(subDir2, "file.txt");
            string subDir3 = Directory.CreateDirectory(Path.Combine(subDir2, "subDir3")).FullName;
            string subDir3File = Path.Combine(subDir3, "file.txt");

            File.WriteAllText(subDir1File, "hi");
            File.WriteAllText(subDir2File, "hi");
            File.WriteAllText(subDir3File, "hi");

            Hide.Perform(subDir3);
            Hide.Perform(subDir2File);

            List<string> hiddenPaths = FileService.GetHiddenPaths(dir);

            Assert.AreEqual(2, hiddenPaths.Count);
            Assert.IsTrue(hiddenPaths.Contains(subDir2File));
            Assert.IsTrue(hiddenPaths.Contains(subDir3));
        }
    }
}
