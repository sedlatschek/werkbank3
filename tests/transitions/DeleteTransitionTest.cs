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
    public class DeleteTransitionTest
    {
        [TestCleanup]
        public void Cleanup()
        {
            Util.ClearDummyWerke();
        }

        [TestMethod]
        public void Works()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.ByHandleOrDie("cad"), WerkState.Hot);

            DeleteTransition transition = new();
            Batch batch = transition.Build(werk);

            Util.WorkOffBatch(batch);

            Assert.IsFalse(Directory.Exists(werk.CurrentDirectory));
        }
    }
}
