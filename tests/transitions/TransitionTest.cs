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
    public class TransitionTest
    {
        [TestMethod]
        [ExpectedException(typeof(werkbank.exceptions.WerkIsAlreadyTransitioningException))]
        public void ThrowsExceptionIfMultipleTransitionsAreBuiltForOneWerk()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Hot);

            BackupTransition backupTransition = new();
            backupTransition.Build(werk);

            HotToColdTransition hotToColdTransition = new();
            hotToColdTransition.Build(werk);
        }
    }
}
