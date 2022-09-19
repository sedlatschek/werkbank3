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
    public class HotToColdTransitionTest
    {
        [TestMethod]
        public void Works()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Hot);

            HotToColdTransition transition = new();
            Batch batch = transition.Build(werk);

            Util.WorkOffBatch(batch);

            Assert.IsFalse(Directory.Exists(werk.GetDirectoryFor(WerkState.Hot)));
            Assert.IsTrue(Directory.Exists(werk.GetDirectoryFor(WerkState.Cold)));
        }
    }
}
