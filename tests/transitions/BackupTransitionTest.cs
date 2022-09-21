using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.models;
using werkbank.repositories;
using werkbank.transitions;

namespace tests.transitions
{
    [TestClass]
    public class BackupTransitionTest
    {
        [TestMethod]
        public void Works()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Hot);

            BackupTransition transition = new();
            Batch batch = transition.Build(werk);

            Util.WorkOffBatch(batch);

            Assert.IsFalse(werk.Moving);
            Assert.IsTrue(Directory.Exists(werk.GetDirectoryFor(WerkState.Hot)));
            Assert.IsTrue(File.Exists(Path.Combine(werk.GetDirectoryFor(WerkState.Hot), "my-content.txt")));
            Assert.IsTrue(File.Exists(Path.Combine(werk.GetDirectoryFor(WerkState.Hot), werkbank.Config.DirNameMeta, werkbank.Config.FileNameMetaJson)));
            Assert.IsTrue(Directory.Exists(werk.GetDirectoryFor(WerkState.Cold)));
            Assert.IsTrue(File.Exists(Path.Combine(werk.GetDirectoryFor(WerkState.Cold), "my-content.txt")));
            Assert.IsFalse(File.Exists(Path.Combine(werk.GetDirectoryFor(WerkState.Cold), werkbank.Config.DirNameMeta, werkbank.Config.FileNameMetaJson)));
            Assert.IsFalse(File.Exists(Path.Combine(werk.GetDirectoryFor(WerkState.Cold), werkbank.Config.DirNameMeta)));
        }

        [TestMethod]
        [ExpectedException(typeof(werkbank.exceptions.UnexpectedWerkStateException))]
        public void ThrowsExceptionIfWerkIsInColdVault()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Cold);

            BackupTransition transition = new();
            Batch batch = transition.Build(werk);

            Util.WorkOffBatch(batch);
        }

        [TestMethod]
        [ExpectedException(typeof(werkbank.exceptions.UnexpectedWerkStateException))]
        public void ThrowsExceptionIfWerkIsInArchiveVault()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Archived);

            BackupTransition transition = new();
            Batch batch = transition.Build(werk);

            Util.WorkOffBatch(batch);
        }
    }
}
