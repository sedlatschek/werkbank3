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
    public class EnvironmentTransitionTest
    {
        [TestMethod]
        public void Works()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Hot);

            EnvironmentTransition transition = new();
            Batch batch = transition.Build(werk, EnvironmentRepository.Environments[1]);

            Util.WorkOffBatch(batch);

            Assert.IsNull(werk.TransitionType);

            Assert.IsFalse(Directory.Exists(werk.GetDirectoryFor(EnvironmentRepository.Environments[0])));
            Assert.IsTrue(Directory.Exists(werk.GetDirectoryFor(EnvironmentRepository.Environments[1])));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionIfTargetEnvironmentIsNotGiven()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Archived);

            EnvironmentTransition transition = new();
            transition.Build(werk, null);
        }

        [TestMethod]
        [ExpectedException(typeof(werkbank.exceptions.InvalidTargetEnvironmentException))]
        public void ThrowsExceptionIfTargetEnvironmentIsAlreadySetForWerk()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Hot);

            EnvironmentTransition transition = new();
            transition.Build(werk, EnvironmentRepository.Environments[0]);
        }
    }
}
