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
