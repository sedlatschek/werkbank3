using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using werkbank.models;
using werkbank.repositories;
using werkbank.transitions;

namespace tests.transitions
{
    [TestClass]
    public class ArchiveToColdTransitionTest
    {
        [TestMethod]
        public void WorksWithoutCompressing()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Archived, CompressOnArchive: false);

            ArchiveToColdTransition transition = new();
            Batch batch = transition.Build(werk);

            Util.WorkOffBatch(batch);

            string coldMetaFile = Path.Combine(werk.GetDirectoryFor(WerkState.Cold), werkbank.Config.DirNameMeta, werkbank.Config.FileNameMetaJson);

            Assert.IsFalse(werk.Moving);
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

            Assert.IsFalse(werk.Moving);
            Assert.IsFalse(Directory.Exists(werk.GetDirectoryFor(WerkState.Archived)));
            Assert.IsTrue(Directory.Exists(werk.GetDirectoryFor(WerkState.Cold)));
            Assert.IsFalse(File.Exists(Path.Combine(werk.GetDirectoryFor(WerkState.Cold), werk.Name + ".zip")));
            Assert.IsTrue(File.Exists(Path.Combine(werk.GetDirectoryFor(WerkState.Cold), "my-content.txt")));
            Assert.AreEqual(JsonConvert.SerializeObject(werk), File.ReadAllText(coldMetaFile));
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
