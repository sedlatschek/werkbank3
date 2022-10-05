using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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
    }
}
