using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using werkbank;
using werkbank.controls;
using werkbank.models;
using werkbank.operations;
using werkbank.repositories;

namespace tests.controls
{
    [TestClass]
    public class WerkListTest
    {
        [TestCleanup]
        public void Cleanup()
        {
            Util.ClearDummyWerke();
        }

        [TestMethod]
        public void GatherWorks()
        {
            ImageList IconList = new();
            WerkList werkList = new(IconList, WerkState.Hot);

            Util.CreateDummyWerk(EnvironmentRepository.ByHandleOrDie("csharp"), WerkState.Hot, "My C# Project");
            Util.CreateDummyWerk(EnvironmentRepository.ByHandleOrDie("delphi7"), WerkState.Hot, "My Delphi 7 Project");
            Util.CreateDummyWerk(EnvironmentRepository.ByHandleOrDie("delphi7"), WerkState.Cold, "Cold werk that should not be picked up");

            werkList.Gather();

            List<Werk> werke = werkList.List.Objects.Cast<Werk>().ToList();

            Assert.IsNotNull(werke);
            Assert.AreEqual(2, werke.Count);

            Assert.AreEqual("My C# Project", werke[0].Title);
            Assert.AreEqual("My Delphi 7 Project", werke[1].Title);
        }

        [TestMethod]
        [ExpectedException(typeof(werkbank.exceptions.UnexpectedWerkEnvironmentException))]
        public void WerkInWrongEnvDirThrowsException()
        {
            ImageList IconList = new();
            WerkList werkList = new(IconList, WerkState.Cold);

            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.ByHandleOrDie("csharp"), WerkState.Cold, "My Painting");

            File.WriteAllText(
                werk.CurrentWerkJson,
                JsonConvert.SerializeObject(werk).Replace("\"env\":\"csharp\"", "\"env\":\"delphi7\"")
            );

            werkList.Gather();
        }

        [TestMethod]
        [ExpectedException(typeof(werkbank.exceptions.UnexpectedWerkStateException))]
        public void WerkInWrongVaultDirThrowsException()
        {
            ImageList IconList = new();
            WerkList werkList = new(IconList, WerkState.Cold);

            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Cold, "My Painting");
            werk.State = WerkState.Hot;

            string werkJson = Path.Combine(werk.GetDirectoryFor(WerkState.Cold, werk.Environment), Config.DirNameMeta, Config.FileNameMetaJson);
            File.WriteAllText(werkJson, JsonConvert.SerializeObject(werk));

            werkList.Gather();
        }

        [TestMethod]
        [ExpectedException(typeof(werkbank.exceptions.UnexpectedWerkDirectoryNameException))]
        public void WerkTitleWerkDirectoryMismatchThrowsException()
        {
            ImageList IconList = new();
            WerkList werkList = new(IconList, WerkState.Archived);

            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.Environments[0], WerkState.Archived, "oxo");

            File.WriteAllText(
                werk.CurrentWerkJson,
                JsonConvert.SerializeObject(werk).Replace("\"name\":\"oxo\"", "\"name\":\"xox\"")
            );

            werkList.Gather();
        }
    }
}
