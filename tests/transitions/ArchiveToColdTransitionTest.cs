using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using werkbank.models;
using werkbank.operations;
using werkbank.repositories;
using werkbank.transitions;

namespace tests.transitions
{
    [TestClass]
    public class ArchiveToColdTransitionTest
    {
        /*
        [TestCleanup]
        public void Cleanup()
        {
            Util.ClearDummyWerke();
        }
        */

        [TestMethod]
        public void WorksWithoutCompressing()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Archived, CompressOnArchive: false);

            ArchiveToColdTransition transition = new();
            Batch batch = transition.Build(werk);

            Util.WorkOffBatch(batch);

            string coldMetaFile = Path.Combine(werk.GetDirectoryFor(WerkState.Cold), werkbank.Config.DirNameMeta, werkbank.Config.FileNameMetaJson);

            Assert.IsNull(werk.TransitionType);
            Assert.IsFalse(Directory.Exists(werk.GetDirectoryFor(WerkState.Archived)));
            Assert.IsTrue(Directory.Exists(werk.GetDirectoryFor(WerkState.Cold)));
            Assert.IsTrue(File.Exists(Path.Combine(werk.GetDirectoryFor(WerkState.Cold), "my-content.txt")));
            Assert.AreEqual(JsonConvert.SerializeObject(werk), File.ReadAllText(coldMetaFile));
        }

        [TestMethod]
        public void WorksWithCompressing()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Archived, CompressOnArchive: true);

            ArchiveToColdTransition transition = new();
            Batch batch = transition.Build(werk);

            Util.WorkOffBatch(batch);

            string coldMetaFile = Path.Combine(werk.GetDirectoryFor(WerkState.Cold), werkbank.Config.DirNameMeta, werkbank.Config.FileNameMetaJson);

            Assert.IsNull(werk.TransitionType);
            Assert.IsFalse(Directory.Exists(werk.GetDirectoryFor(WerkState.Archived)));
            Assert.IsTrue(Directory.Exists(werk.GetDirectoryFor(WerkState.Cold)));
            Assert.IsFalse(File.Exists(Path.Combine(werk.GetDirectoryFor(WerkState.Cold), werk.Name + ".zip")));
            Assert.IsTrue(File.Exists(Path.Combine(werk.GetDirectoryFor(WerkState.Cold), "my-content.txt")));
            Assert.AreEqual(JsonConvert.SerializeObject(werk), File.ReadAllText(coldMetaFile));
        }

        [TestMethod]
        public void HiddenStillHiddenWithCompressing()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[7], WerkState.Archived, CreateContent: false, CompressOnArchive: true);

            DirectoryInfo dummyDir = Directory.CreateDirectory(Util.GetTempPath());
            Directory.CreateDirectory(Path.Combine(dummyDir.FullName, "s1"));
            DirectoryInfo subDir2 = Directory.CreateDirectory(Path.Combine(dummyDir.FullName, "s2"));
            string file1 = Path.Combine(dummyDir.FullName, "f1.txt");
            File.WriteAllText(file1, "hi");
            Hide.Perform(subDir2.FullName);
            Hide.Perform(file1);
            Zip.Perform(dummyDir.FullName, Path.Combine(werk.CurrentDirectory, werk.Name + ".zip"));
                        
            File.WriteAllText(
                Path.Combine(werk.CurrentMetaDirectory, werkbank.Config.FileNameHiddenJson),
                JsonConvert.SerializeObject(new List<string>()
                {
                    "s2",
                    "f1.txt"
                })
            );

            ArchiveToColdTransition transition = new();
            Batch batch = transition.Build(werk);
            Util.WorkOffBatch(batch);

            Assert.IsNull(werk.TransitionType);
            Assert.IsFalse(Directory.Exists(werk.GetDirectoryFor(WerkState.Archived)));
            Assert.IsTrue(Directory.Exists(werk.GetDirectoryFor(WerkState.Cold)));
            Assert.IsFalse(new DirectoryInfo(Path.Combine(werk.GetDirectoryFor(WerkState.Cold), "s1")).Attributes.HasFlag(FileAttributes.Hidden));
            Assert.IsTrue(new DirectoryInfo(Path.Combine(werk.GetDirectoryFor(WerkState.Cold), "s2")).Attributes.HasFlag(FileAttributes.Hidden));
            Assert.IsTrue(File.GetAttributes(Path.Combine(werk.GetDirectoryFor(WerkState.Cold), "f1.txt")).HasFlag(FileAttributes.Hidden));
            Assert.IsFalse(File.Exists(Path.Combine(werk.GetDirectoryFor(WerkState.Cold), werkbank.Config.DirNameMeta, werkbank.Config.FileNameHiddenJson)));
        }

        [TestMethod]
        public void HiddenStillHiddenWithoutCompressing()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[7], WerkState.Archived, CompressOnArchive: false);

            Directory.CreateDirectory(Path.Combine(werk.CurrentDirectory, "s1"));
            DirectoryInfo subDir2 = Directory.CreateDirectory(Path.Combine(werk.CurrentDirectory, "s2"));
            string file1 = Path.Combine(werk.CurrentDirectory, "f1.txt");
            File.WriteAllText(file1, "hi");
            Hide.Perform(subDir2.FullName);
            Hide.Perform(file1);

            ArchiveToColdTransition transition = new();
            Batch batch = transition.Build(werk);
            Util.WorkOffBatch(batch);

            Assert.IsNull(werk.TransitionType);
            Assert.IsFalse(Directory.Exists(werk.GetDirectoryFor(WerkState.Archived)));
            Assert.IsTrue(Directory.Exists(werk.GetDirectoryFor(WerkState.Cold)));
            Assert.IsFalse(new DirectoryInfo(Path.Combine(werk.GetDirectoryFor(WerkState.Cold), "s1")).Attributes.HasFlag(FileAttributes.Hidden));
            Assert.IsTrue(new DirectoryInfo(Path.Combine(werk.GetDirectoryFor(WerkState.Cold), "s2")).Attributes.HasFlag(FileAttributes.Hidden));
            Assert.IsTrue(File.GetAttributes(Path.Combine(werk.GetDirectoryFor(WerkState.Cold), "f1.txt")).HasFlag(FileAttributes.Hidden));
            Assert.IsFalse(File.Exists(Path.Combine(werk.GetDirectoryFor(WerkState.Cold), werkbank.Config.DirNameMeta, werkbank.Config.FileNameHiddenJson)));
        }

        [TestMethod]
        [ExpectedException(typeof(werkbank.exceptions.UnexpectedWerkStateException))]
        public void ThrowsExceptionIfWerkIsInHotVault()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Hot);

            ArchiveToColdTransition transition = new();
            Batch batch = transition.Build(werk);

            Util.WorkOffBatch(batch);
        }

        [TestMethod]
        [ExpectedException(typeof(werkbank.exceptions.UnexpectedWerkStateException))]
        public void ThrowsExceptionIfWerkIsInColdVault()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Cold);

            ArchiveToColdTransition transition = new();
            Batch batch = transition.Build(werk);

            Util.WorkOffBatch(batch);
        }
    }
}
