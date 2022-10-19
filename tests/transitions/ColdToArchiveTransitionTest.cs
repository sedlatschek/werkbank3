using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.models;
using werkbank.operations;
using werkbank.repositories;
using werkbank.transitions;

namespace tests.transitions
{
    [TestClass]
    public class ColdToArchiveTransitionTest
    {
        [TestCleanup]
        public void Cleanup()
        {
            Util.ClearDummyWerke();
        }

        [TestMethod]
        public void WorksWithoutCompressing()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Cold, CompressOnArchive: false);

            ColdToArchiveTransition transition = new();
            Batch batch = transition.Build(werk);

            Util.WorkOffBatch(batch);

            string archiveMetaFile = Path.Combine(werk.GetDirectoryFor(WerkState.Archived), werkbank.Config.DirNameMeta, werkbank.Config.FileNameMetaJson);

            Assert.IsNull(werk.TransitionType);
            Assert.IsFalse(Directory.Exists(werk.GetDirectoryFor(WerkState.Cold)));
            Assert.IsTrue(Directory.Exists(werk.GetDirectoryFor(WerkState.Archived)));
            Assert.IsTrue(File.Exists(Path.Combine(werk.GetDirectoryFor(WerkState.Archived), "my-content.txt")));
            Assert.AreEqual(JsonConvert.SerializeObject(werk), File.ReadAllText(archiveMetaFile));
        }

        [TestMethod]
        public void WorksWithCompressing()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Cold, CompressOnArchive: true);

            ColdToArchiveTransition transition = new();
            Batch batch = transition.Build(werk);

            Util.WorkOffBatch(batch);

            string archiveMetaFile = Path.Combine(werk.GetDirectoryFor(WerkState.Archived), werkbank.Config.DirNameMeta, werkbank.Config.FileNameMetaJson);

            Assert.IsNull(werk.TransitionType);
            Assert.IsFalse(Directory.Exists(werk.GetDirectoryFor(WerkState.Cold)));
            Assert.IsTrue(Directory.Exists(werk.GetDirectoryFor(WerkState.Archived)));
            Assert.IsTrue(File.Exists(Path.Combine(werk.GetDirectoryFor(WerkState.Archived), werk.Name + ".zip")));
            Assert.IsFalse(File.Exists(Path.Combine(werk.GetDirectoryFor(WerkState.Archived), "my-content.txt")));
            Assert.AreEqual(JsonConvert.SerializeObject(werk), File.ReadAllText(archiveMetaFile));
        }

        [TestMethod]
        public void HiddenStillHiddenWithoutCompressing()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[6], WerkState.Cold, CompressOnArchive: false);

            Directory.CreateDirectory(Path.Combine(werk.CurrentDirectory, "s1"));
            DirectoryInfo subDir2 = Directory.CreateDirectory(Path.Combine(werk.CurrentDirectory, "s2"));
            string file1 = Path.Combine(werk.CurrentDirectory, "f1.txt");
            File.WriteAllText(file1, "hi");

            Hide.Perform(subDir2.FullName);
            Hide.Perform(file1);

            ColdToArchiveTransition transition = new();
            Batch batch = transition.Build(werk);
            Util.WorkOffBatch(batch);

            Assert.IsNull(werk.TransitionType);
            Assert.IsFalse(Directory.Exists(werk.GetDirectoryFor(WerkState.Cold)));
            Assert.IsTrue(Directory.Exists(werk.GetDirectoryFor(WerkState.Archived)));
            Assert.IsFalse(new DirectoryInfo(Path.Combine(werk.GetDirectoryFor(WerkState.Archived), "s1")).Attributes.HasFlag(FileAttributes.Hidden));
            Assert.IsTrue(new DirectoryInfo(Path.Combine(werk.GetDirectoryFor(WerkState.Archived), "s2")).Attributes.HasFlag(FileAttributes.Hidden));
            Assert.IsTrue(File.GetAttributes(Path.Combine(werk.GetDirectoryFor(WerkState.Archived), "f1.txt")).HasFlag(FileAttributes.Hidden));
        }

        [TestMethod]
        public void HiddenStillHiddenWithCompressing()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[6], WerkState.Cold, CompressOnArchive: true);

            Directory.CreateDirectory(Path.Combine(werk.CurrentDirectory, "s1"));
            DirectoryInfo subDir2 = Directory.CreateDirectory(Path.Combine(werk.CurrentDirectory, "s2"));
            string file1 = Path.Combine(werk.CurrentDirectory, "f1.txt");
            File.WriteAllText(file1, "hi");

            Hide.Perform(subDir2.FullName);
            Hide.Perform(file1);

            ColdToArchiveTransition transition = new();
            Batch batch = transition.Build(werk);
            Util.WorkOffBatch(batch);

            Assert.IsNull(werk.TransitionType);
            Assert.IsFalse(Directory.Exists(werk.GetDirectoryFor(WerkState.Cold)));
            Assert.IsTrue(Directory.Exists(werk.GetDirectoryFor(WerkState.Archived)));

            string hiddenJson = Path.Combine(werk.GetDirectoryFor(WerkState.Archived), werkbank.Config.DirNameMeta, werkbank.Config.FileNameHiddenJson);

            Assert.IsTrue(File.Exists(hiddenJson));

            string content = File.ReadAllText(hiddenJson);

            Assert.IsTrue(content.Contains(subDir2.FullName.Replace(werk.GetDirectoryFor(WerkState.Cold) + "\\", "")));
            Assert.IsTrue(content.Contains(file1.Replace(werk.GetDirectoryFor(WerkState.Cold) + "\\", "")));
        }

        [TestMethod]
        [ExpectedException(typeof(werkbank.exceptions.UnexpectedWerkStateException))]
        public void ThrowsExceptionIfWerkIsInHotVault()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Hot);

            ColdToArchiveTransition transition = new();
            Batch batch = transition.Build(werk);

            Util.WorkOffBatch(batch);
        }

        [TestMethod]
        [ExpectedException(typeof(werkbank.exceptions.UnexpectedWerkStateException))]
        public void ThrowsExceptionIfWerkIsInArchiveVault()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Archived);

            ColdToArchiveTransition transition = new();
            Batch batch = transition.Build(werk);

            Util.WorkOffBatch(batch);
        }
    }
}
