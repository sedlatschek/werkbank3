using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slugify;
using werkbank.models;
using Newtonsoft.Json;
using werkbank.services;
using werkbank.transitions;
using werkbank.operations;

namespace tests
{
    public static class Util
    {
        private static readonly Random random = new();

        /// <summary>
        /// Get a random path in the temp directory.
        /// </summary>
        /// <returns></returns>
        public static string GetTempPath()
        {
            return Path.Combine(Path.GetTempPath(), werkbank.Config.DirNameTests, Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Get a random alphabetic string of a given length.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Create a dummy werk with all its directories and optional content.
        /// </summary>
        /// <param name="Environment"></param>
        /// <param name="State"></param>
        /// <param name="Title"></param>
        /// <param name="CreateContent"></param>
        /// <returns></returns>
        public static Werk CreateDummyWerk(werkbank.environments.Environment Environment, WerkState State = WerkState.Hot, string? Title = null, bool CreateContent = true, bool CompressOnArchive = true)
        {
            SlugHelper slugHelper = new();
            string title = Title ?? GetRandomString(8);
            string name = slugHelper.GenerateSlug(title);

            Werk werk = new(Guid.NewGuid(), name, title, Environment)
            {
                State = State,
                CompressOnArchive = CompressOnArchive
            };

            Directory.CreateDirectory(werk.CurrentMetaDirectory);
            File.WriteAllText(werk.CurrentWerkJson, JsonConvert.SerializeObject(werk));

            if (CreateContent)
            {
                if (State == WerkState.Archived && CompressOnArchive)
                {
                    string tmpDir = GetTempPath();
                    Directory.CreateDirectory(tmpDir);
                    File.WriteAllText(Path.Combine(tmpDir, "my-content.txt"), "just some content\n");
                    File.Copy("../../../../logo.svg", Path.Combine(tmpDir, "logo.svg"));
                    Zip.Perform(tmpDir, Path.Combine(werk.CurrentDirectory, FileService.ReplaceInvalidCharsFromPath(werk.Name) + ".zip"));
                    Directory.Delete(tmpDir, true);
                }
                else
                {
                    File.WriteAllText(Path.Combine(werk.CurrentDirectory, "my-content.txt"), "just some content\n");
                    File.Copy("../../../../logo.svg", Path.Combine(werk.CurrentDirectory, "logo.svg"));
                }
            }

            return werk;
        }

        /// <summary>
        /// Cleanse all dummy werke.
        /// </summary>
        public static void ClearDummyWerke()
        {
            string[] dirs = new string[]
            {
                Settings.Properties.DirHotVault,
                Settings.Properties.DirColdVault,
                Settings.Properties.DirArchiveVault,
            };

            foreach (string dir in dirs)
            {
                if (Directory.Exists(dir))
                {
                    Directory.Delete(dir, true);
                }
            }
        }

        /// <summary>
        /// Work off a batch of operations.
        /// </summary>
        /// <param name="Batch"></param>
        public static void WorkOffBatch(Batch Batch)
        {
            Console.WriteLine("WorkOffBatch: ");
            foreach (Operation op in Batch.Operations)
            {
                Console.WriteLine("  " + op.Type.ToString());
                op.Run();
                if (op.Error != null)
                {
                    throw op.Error;
                }
            }

            if (Batch.Done && Batch.Werk != null)
            {
                Transition.For(Batch.TransitionType).Finish(Batch);
            }
            Batch.Untie();
        }
    }
}
