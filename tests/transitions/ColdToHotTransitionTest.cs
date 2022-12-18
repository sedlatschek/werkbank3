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
    public class ColdToHotTransitionTest
    {
        [TestCleanup]
        public void Cleanup()
        {
            Util.ClearDummyWerke();
        }

        [TestMethod]
        public void Works()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Cold);

            ColdToHotTransition transition = new();
            Batch batch = transition.Build(werk);

            Util.WorkOffBatch(batch);

            string hotMetaFile = Path.Combine(werk.GetDirectoryFor(WerkState.Hot), werkbank.Config.DirNameMeta, werkbank.Config.FileNameMetaJson);

            Assert.IsNull(werk.TransitionType);
            Assert.IsFalse(Directory.Exists(werk.GetDirectoryFor(WerkState.Cold)));
            Assert.IsTrue(Directory.Exists(werk.GetDirectoryFor(WerkState.Hot)));
            Assert.IsTrue(File.Exists(hotMetaFile));
            Assert.IsTrue(File.Exists(Path.Combine(werk.GetDirectoryFor(WerkState.Hot), "my-content.txt")));
            Assert.AreEqual(JsonConvert.SerializeObject(werk), File.ReadAllText(hotMetaFile));
        }

        [TestMethod]
        public void HiddenStillHidden()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[5], WerkState.Cold);

            Directory.CreateDirectory(Path.Combine(werk.CurrentDirectory, "s1"));
            DirectoryInfo subDir2 = Directory.CreateDirectory(Path.Combine(werk.CurrentDirectory, "s2"));
            string file1 = Path.Combine(werk.CurrentDirectory, "f1.txt");
            File.WriteAllText(file1, "hi");

            Hide.Perform(subDir2.FullName);
            Hide.Perform(file1);

            ColdToHotTransition transition = new();
            Batch batch = transition.Build(werk);
            Util.WorkOffBatch(batch);

            Assert.IsNull(werk.TransitionType);
            Assert.IsFalse(Directory.Exists(werk.GetDirectoryFor(WerkState.Cold)));
            Assert.IsTrue(Directory.Exists(werk.GetDirectoryFor(WerkState.Hot)));
            Assert.IsFalse(new DirectoryInfo(Path.Combine(werk.GetDirectoryFor(WerkState.Hot), "s1")).Attributes.HasFlag(FileAttributes.Hidden));
            Assert.IsTrue(new DirectoryInfo(Path.Combine(werk.GetDirectoryFor(WerkState.Hot), "s2")).Attributes.HasFlag(FileAttributes.Hidden));
            Assert.IsTrue(File.GetAttributes(Path.Combine(werk.GetDirectoryFor(WerkState.Hot), "f1.txt")).HasFlag(FileAttributes.Hidden));
        }

        [TestMethod]
        public void GitDirIsHidden()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[4], WerkState.Hot);
            DirectoryInfo subDir2 = Directory.CreateDirectory(Path.Combine(werk.CurrentDirectory, ".git"));
            string file1 = Path.Combine(subDir2.FullName, "f1.txt");
            File.WriteAllText(file1, "hi");

            HotToColdTransition hotToCold = new();
            Util.WorkOffBatch(hotToCold.Build(werk));

            Assert.IsNull(werk.TransitionType);
            Assert.IsFalse(Directory.Exists(werk.GetDirectoryFor(WerkState.Hot)));
            Assert.IsTrue(Directory.Exists(werk.GetDirectoryFor(WerkState.Cold)));
            Assert.IsFalse(Directory.Exists(Path.Combine(werk.GetDirectoryFor(WerkState.Cold), ".git")));
            Assert.IsTrue(File.Exists(Path.Combine(werk.GetDirectoryFor(WerkState.Cold), "git.zip")));

            ColdToHotTransition coldToHot = new();
            Util.WorkOffBatch(coldToHot.Build(werk));

            Assert.IsNull(werk.TransitionType);
            Assert.IsFalse(Directory.Exists(werk.GetDirectoryFor(WerkState.Cold)));
            Assert.IsTrue(Directory.Exists(werk.GetDirectoryFor(WerkState.Hot)));
            Assert.IsTrue(new DirectoryInfo(Path.Combine(werk.GetDirectoryFor(WerkState.Hot), ".git")).Attributes.HasFlag(FileAttributes.Hidden));
        }

        [TestMethod]
        public void WerkDirIsHidden()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Hot);

            HotToColdTransition hotToCold = new();
            Util.WorkOffBatch(hotToCold.Build(werk));

            Assert.IsNull(werk.TransitionType);
            Assert.IsFalse(Directory.Exists(werk.GetDirectoryFor(WerkState.Hot)));
            Assert.IsTrue(Directory.Exists(werk.GetDirectoryFor(WerkState.Cold)));

            ColdToHotTransition coldToHot = new();
            Util.WorkOffBatch(coldToHot.Build(werk));

            Assert.IsNull(werk.TransitionType);
            Assert.IsFalse(Directory.Exists(werk.GetDirectoryFor(WerkState.Cold)));
            Assert.IsTrue(Directory.Exists(werk.GetDirectoryFor(WerkState.Hot)));
            Assert.IsTrue(new DirectoryInfo(Path.Combine(werk.GetDirectoryFor(WerkState.Hot), werkbank.Config.DirNameMeta)).Attributes.HasFlag(FileAttributes.Hidden));
        }

        [TestMethod]
        [ExpectedException(typeof(werkbank.exceptions.UnexpectedWerkStateException))]
        public void ThrowsExceptionIfWerkIsInHotVault()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Hot);

            ColdToHotTransition transition = new();
            Batch batch = transition.Build(werk);

            Util.WorkOffBatch(batch);
        }

        [TestMethod]
        [ExpectedException(typeof(werkbank.exceptions.UnexpectedWerkStateException))]
        public void ThrowsExceptionIfWerkIsInArchiveVault()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Archived);

            ColdToHotTransition transition = new();
            Batch batch = transition.Build(werk);

            Util.WorkOffBatch(batch);
        }
    }
}
