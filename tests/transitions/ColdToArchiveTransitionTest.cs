using Newtonsoft.Json;
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
    public class ColdToArchiveTransitionTest
    {
        [TestMethod]
        public void WorksWithoutCompressing()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Cold, CompressOnArchive: false);

            ColdToArchiveTransition transition = new();
            Batch batch = transition.Build(werk);

            Util.WorkOffBatch(batch);

            string archiveMetaFile = Path.Combine(werk.GetDirectoryFor(WerkState.Archived), werkbank.Config.DirNameMeta, werkbank.Config.FileNameMetaJson);

            Assert.IsFalse(werk.Moving);
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

            Assert.IsFalse(werk.Moving);
            Assert.IsFalse(Directory.Exists(werk.GetDirectoryFor(WerkState.Cold)));
            Assert.IsTrue(Directory.Exists(werk.GetDirectoryFor(WerkState.Archived)));
            Assert.IsTrue(File.Exists(Path.Combine(werk.GetDirectoryFor(WerkState.Archived), werk.Name + ".zip")));
            Assert.IsFalse(File.Exists(Path.Combine(werk.GetDirectoryFor(WerkState.Archived), "my-content.txt")));
            Assert.AreEqual(JsonConvert.SerializeObject(werk), File.ReadAllText(archiveMetaFile));
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
