using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.services;

namespace tests.services
{
    [TestClass]
    public class FileServiceTest
    {
        [TestMethod]
        public void WorksWithDir()
        {
            string path = Util.GetTempPath();
            Directory.CreateDirectory(path);

            Assert.IsTrue(FileService.PathExists(path, out FileService.PathType? pathType));
            Assert.AreEqual(FileService.PathType.Directory, pathType);
        }

        [TestMethod]
        public void WorksWithFile()
        {
            string path = Util.GetTempPath() + ".txt";
            File.WriteAllText(path, "hi");

            Assert.IsTrue(FileService.PathExists(path, out FileService.PathType? pathType));
            Assert.AreEqual(FileService.PathType.File, pathType);
        }

        [TestMethod]
        public void WorksWithFileWithoutExtension()
        {
            string path = Util.GetTempPath();
            File.WriteAllText(path, "hi");

            Assert.IsTrue(FileService.PathExists(path, out FileService.PathType? pathType));
            Assert.AreEqual(FileService.PathType.File, pathType);
        }

        [TestMethod]
        public void WorksWithNonExistingPath()
        {
            string path = Util.GetTempPath();
            Assert.IsFalse(FileService.PathExists(path, out FileService.PathType? pathType));
        }
    }
}
