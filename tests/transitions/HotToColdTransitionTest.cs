using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using werkbank.models;
using werkbank.operations;
using werkbank.repositories;
using werkbank.services;
using werkbank.transitions;

namespace tests.transitions
{
    [TestClass]
    public class HotToColdTransitionTest
    {
        [TestCleanup]
        public void Cleanup()
        {
            Util.ClearDummyWerke();
        }

        [TestMethod]
        public void Works()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Hot);

            HotToColdTransition transition = new();
            Batch batch = transition.Build(werk);

            Util.WorkOffBatch(batch);

            string coldMetaFile = Path.Combine(werk.GetDirectoryFor(WerkState.Cold), werkbank.Config.DirNameMeta, werkbank.Config.FileNameMetaJson);

            Assert.IsNull(werk.TransitionType);
            Assert.IsFalse(Directory.Exists(werk.GetDirectoryFor(WerkState.Hot)));
            Assert.IsTrue(Directory.Exists(werk.GetDirectoryFor(WerkState.Cold)));
            Assert.IsTrue(File.Exists(coldMetaFile));
            Assert.IsTrue(File.Exists(Path.Combine(werk.GetDirectoryFor(WerkState.Cold), "my-content.txt")));
            Assert.AreEqual(JsonConvert.SerializeObject(werk), File.ReadAllText(coldMetaFile));
        }

        [TestMethod]
        public void HiddenStillHidden()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[3], WerkState.Hot);

            Directory.CreateDirectory(Path.Combine(werk.CurrentDirectory, "s1"));
            DirectoryInfo subDir2 = Directory.CreateDirectory(Path.Combine(werk.CurrentDirectory, "s2"));
            string file1 = Path.Combine(werk.CurrentDirectory, "f1.txt");
            File.WriteAllText(file1, "hi");

            Hide.Perform(subDir2.FullName);
            Hide.Perform(file1);

            HotToColdTransition transition = new();
            Batch batch = transition.Build(werk);
            Util.WorkOffBatch(batch);

            Assert.IsNull(werk.TransitionType);
            Assert.IsFalse(Directory.Exists(werk.GetDirectoryFor(WerkState.Hot)));
            Assert.IsTrue(Directory.Exists(werk.GetDirectoryFor(WerkState.Cold)));
            Assert.IsFalse(new DirectoryInfo(Path.Combine(werk.GetDirectoryFor(WerkState.Cold), "s1")).Attributes.HasFlag(FileAttributes.Hidden));
            Assert.IsTrue(new DirectoryInfo(Path.Combine(werk.GetDirectoryFor(WerkState.Cold), "s2")).Attributes.HasFlag(FileAttributes.Hidden));
            Assert.IsTrue(File.GetAttributes(Path.Combine(werk.GetDirectoryFor(WerkState.Cold), "f1.txt")).HasFlag(FileAttributes.Hidden));
        }

        [TestMethod]
        [ExpectedException(typeof(werkbank.exceptions.UnexpectedWerkStateException))]
        public void ThrowsExceptionIfWerkIsInColdVault()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Cold);

            HotToColdTransition transition = new();
            Batch batch = transition.Build(werk);

            Util.WorkOffBatch(batch);
        }

        [TestMethod]
        [ExpectedException(typeof(werkbank.exceptions.UnexpectedWerkStateException))]
        public void ThrowsExceptionIfWerkIsInArchiveVault()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Archived);

            HotToColdTransition transition = new();
            Batch batch = transition.Build(werk);

            Util.WorkOffBatch(batch);
        }
    }
}
