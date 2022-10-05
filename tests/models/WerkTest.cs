using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.models;
using werkbank.operations;
using werkbank.repositories;

namespace tests.models
{
    [TestClass]
    public class WerkTest
    {
        [TestCleanup]
        public void Cleanup()
        {
            Util.ClearDummyWerke();
        }

        [TestMethod]
        public void HasGitWorksForHotWerke()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.ByHandleOrDie("audio"), WerkState.Hot);

            Assert.IsTrue(Directory.Exists(werk.CurrentDirectory));
            Assert.IsFalse(werk.HasGit);

            Directory.CreateDirectory(Path.Combine(werk.CurrentDirectory, ".git"));

            Assert.IsTrue(werk.HasGit);
        }

        [TestMethod]
        public void HasGitWorksForColdWerke()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.ByHandleOrDie("cpp"), WerkState.Cold);

            Assert.IsTrue(Directory.Exists(werk.CurrentDirectory));
            Assert.IsFalse(werk.HasGit);

            File.WriteAllText(Path.Combine(werk.CurrentDirectory, "git.zip"), "I am not really a zip :/");

            Assert.IsTrue(werk.HasGit);
        }

        [TestMethod]
        public void HasGitWorksForArchivedWerke()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.ByHandleOrDie("cpp"), WerkState.Archived);

            Assert.IsTrue(Directory.Exists(werk.CurrentDirectory));
            Assert.IsFalse(werk.HasGit);

            File.WriteAllText(Path.Combine(werk.CurrentDirectory, "git.zip"), "I am not really a zip :/");

            Assert.IsTrue(werk.HasGit);
        }

        [TestMethod]
        public void GetGitRemoteUrlWorksForHotWerke()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.ByHandleOrDie("audio"), WerkState.Hot);

            string gitDir = Path.Combine(werk.CurrentDirectory, ".git");
            string gitConfig = Path.Combine(gitDir, "config");

            Directory.CreateDirectory(gitDir);
            File.WriteAllText(gitConfig,
                "[core]\r\n" +
                "\trepositoryformatversion = 0\r\n" +
                "\tfilemode = false\r\n" +
                "\tbare = false\r\n" +
                "\tlogallrefupdates = true\r\n" +
                "\tsymlinks = false\r\n" +
                "\tignorecase = true\r\n" +
                "[remote \"origin\"]\r\n" +
                "\turl = https://github.com/sedlatschek/werkbank3.git\r\n" +
                "\tfetch = +refs/heads/*:refs/remotes/origin/*\r\n" +
                "[branch \"main\"]\r\n" +
                "\tremote = origin\r\n" +
                "\tmerge = refs/heads/main\r\n" +
                "[lfs]\r\n" +
                "\trepositoryformatversion = 0\r\n");

            Assert.IsTrue(werk.HasGit);
            Assert.AreEqual("https://github.com/sedlatschek/werkbank3.git", werk.GetGitRemoteUrl());

            Directory.Delete(gitDir, true);
            Assert.IsNull(werk.GetGitRemoteUrl());
        }

        [TestMethod]
        public void GetGitRemoteUrlReturnsNullWhenNoRemoteIsConfigured()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.ByHandleOrDie("audio"), WerkState.Hot);

            string gitDir = Path.Combine(werk.CurrentDirectory, ".git");
            string gitConfig = Path.Combine(gitDir, "config");

            Directory.CreateDirectory(gitDir);
            File.WriteAllText(gitConfig,
                "[core]\r\n" +
                "\trepositoryformatversion = 0\r\n" +
                "\tfilemode = false\r\n" +
                "\tbare = false\r\n" +
                "\tlogallrefupdates = true\r\n" +
                "\tsymlinks = false\r\n" +
                "\tignorecase = true\r\n" +
                "[branch \"main\"]\r\n" +
                "\tremote = origin\r\n" +
                "\tmerge = refs/heads/main\r\n" +
                "[lfs]\r\n" +
                "\trepositoryformatversion = 0\r\n");

            Assert.IsTrue(werk.HasGit);
            Assert.IsNull(werk.GetGitRemoteUrl());
        }

        [TestMethod]
        public void GetGitRemoteUrlWorksForColdWerke()
        {
            Werk werk = Util.CreateDummyWerk(EnvironmentRepository.ByHandleOrDie("cad"), WerkState.Cold);

            string tmpGit = Directory.CreateDirectory(Util.GetTempPath()).FullName;
            string gitZip = Path.Combine(werk.CurrentDirectory, "git.zip");

            File.WriteAllText(Path.Combine(tmpGit, "config"),
                "[core]\r\n" +
                "\trepositoryformatversion = 0\r\n" +
                "\tfilemode = false\r\n" +
                "\tbare = false\r\n" +
                "\tlogallrefupdates = true\r\n" +
                "\tsymlinks = false\r\n" +
                "\tignorecase = true\r\n" +
                "[remote \"origin\"]\r\n" +
                "\turl = https://github.com/sedlatschek/werkbank3.git\r\n" +
                "\tfetch = +refs/heads/*:refs/remotes/origin/*\r\n" +
                "[branch \"main\"]\r\n" +
                "\tremote = origin\r\n" +
                "\tmerge = refs/heads/main\r\n" +
                "[lfs]\r\n" +
                "\trepositoryformatversion = 0\r\n");

            ZipFile.CreateFromDirectory(tmpGit, gitZip);

            Assert.IsTrue(werk.HasGit);
            Assert.AreEqual("https://github.com/sedlatschek/werkbank3.git", werk.GetGitRemoteUrl());

            File.Delete(gitZip);
            Assert.IsNull(werk.GetGitRemoteUrl());
        }
    }
}
