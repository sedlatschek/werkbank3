using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using IniParser;
using IniParser.Model;
using Newtonsoft.Json;
using werkbank.converters;
using werkbank.environments;
using werkbank.repositories;
using werkbank.services;
using werkbank.transitions;
using werkbank.operations;

namespace werkbank.models
{
    /// <summary>
    /// State a Werk can take
    /// </summary>
    public enum WerkState
    {
        Hot,
        Cold,
        Archived
    }

    /// <summary>
    /// Holds a timestamp to a state
    /// </summary>
    public class WerkStateTimestamp
    {
        [JsonProperty("ts")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("env"), JsonConverter(typeof(NullableEnvironmentConverter))]
        public environments.Environment? Environment { get; set; }

        [JsonProperty("state")]
        public WerkState State { get; set; }
    }

    /// <summary>
    /// A Werk is a folder containing a piece of work
    /// </summary>
    public class Werk
    {
        [JsonProperty("version")]
        public readonly int Version = 1;

        [JsonProperty("id")]
        public readonly Guid Id;

        [JsonProperty("name")]
        public readonly string Name;

        [JsonProperty("title")]
        public string Title;

        [JsonProperty("desc")]
        public string? Description;

        [JsonProperty("state")]
        public WerkState State = WerkState.Hot;

        [JsonProperty("env"), JsonConverter(typeof(EnvironmentConverter))]
        public environments.Environment Environment;

        [JsonProperty("compressOnArchive")]
        public bool CompressOnArchive = true;

        [JsonProperty("created")]
        public DateTime CreatedAt;

        [JsonProperty("transitionType")]
        public TransitionType? TransitionType;

        [JsonProperty("history")]
        public readonly List<WerkStateTimestamp> History = new();

        [JsonIgnore]
        public DateTime LastModified { get; set; }

        public Werk(Guid Id, string Name, string Title, environments.Environment Environment, DateTime? CreatedAt = null)
        {
            this.Id = Id;
            this.Name = Name;
            this.Title = Title;
            this.Environment = Environment;
            this.CreatedAt = CreatedAt ?? DateTime.Now;
        }

        [JsonIgnore]
        public Batch? CurrentBatch;

        [JsonIgnore]
        public bool HasIcon
        {
            get
            {
                if (Environment == null)
                {
                    return false;
                }
                return File.Exists(IconFile);
            }
        }

        /// <summary>
        /// Whether or not the werk uses Git.
        /// </summary>
        [JsonIgnore]
        public bool HasGit => (State == WerkState.Hot && Directory.Exists(Path.Combine(CurrentDirectory, Config.DirNameGit)))
                    || (State != WerkState.Hot && File.Exists(Path.Combine(CurrentDirectory, Config.FileNameGitZip)));

        [JsonIgnore]
        public string IconFile => Path.Combine(CurrentMetaDirectory, Config.FileNameWerkIcon);

        /// <summary>
        /// The path of the directory the werk currently resides.
        /// </summary>
        [JsonIgnore]
        public string CurrentDirectory => GetDirectoryFor(State, Environment, Name);

        /// <summary>
        /// The path to the current meta directory (.werk) of the werk.
        /// </summary>
        [JsonIgnore]
        public string CurrentMetaDirectory => Path.Combine(CurrentDirectory, Config.DirNameMeta);

        /// <summary>
        /// The path to the current werk file (.werk/werk.json).
        /// </summary>
        [JsonIgnore]
        public string CurrentWerkJson => Path.Combine(CurrentMetaDirectory, Config.FileNameMetaJson);

        /// <summary>
        /// Save the meta file of the work to a given path. If no path is provided, use the default path.
        /// </summary>
        /// <param name="FilePath"></param>
        public void Save(string? FilePath = null)
        {
            string file = FilePath ?? Path.Combine(CurrentDirectory, Config.DirNameMeta, Config.FileNameMetaJson);
            File.WriteAllText(
                file,
                JsonConvert.SerializeObject(this)
            );
        }

        /// <summary>
        /// Get the directory the werk would have for the given environment.
        /// </summary>
        /// <param name="Environment"></param>
        /// <returns></returns>
        public string GetDirectoryFor(environments.Environment Environment)
        {
            return GetDirectoryFor(State, Environment, Name);
        }

        /// <summary>
        /// Get the directory the werk would have for the given werk state.
        /// </summary>
        /// <param name="State"></param>
        /// <returns></returns>
        public string GetDirectoryFor(WerkState State)
        {
            return GetDirectoryFor(State, Environment, Name);
        }

        /// <summary>
        /// Get the directory the werk would have for the given werk state and environment.
        /// </summary>
        /// <param name="State"></param>
        /// <param name="Environment"></param>
        /// <returns></returns>
        public string GetDirectoryFor(WerkState State, environments.Environment Environment)
        {
            return GetDirectoryFor(State, Environment, Name);
        }

        /// <summary>
        /// Get the directory a werk would have with a given werk state, environment and name.
        /// </summary>
        /// <param name="State"></param>
        /// <param name="Environment"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static string GetDirectoryFor(WerkState State, environments.Environment Environment, string Name)
        {
            return Path.Combine(VaultRepository.GetDirectory(State), Environment.Directory, Name);
        }

        /// <summary>
        /// Open the current directory of the werk in the file explorer.
        /// </summary>
        public void OpenInFileExplorer()
        {
            System.Diagnostics.Process.Start("explorer.exe", CurrentDirectory);
        }

        /// <summary>
        /// Open the current directory of the werk in VS Code.
        /// </summary>
        public void OpenInVsCode()
        {
            System.Diagnostics.Process.Start(Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData),
                @"Programs\Microsoft VS Code\Code.exe"),
                CurrentDirectory
            );
        }

        /// <summary>
        /// Retrieve the remote url from the local git repository config. Will be null when git config does not exist or has no remote configured.
        /// </summary>
        /// <returns></returns>
        public string? GetGitRemoteUrl()
        {
            if (!HasGit)
            {
                return null;
            }

            string? url = null;

            if (State == WerkState.Hot)
            {
                FileIniDataParser parser = new();
                IniData gitConfig = parser.ReadFile(Path.Combine(CurrentDirectory, Config.DirNameGit, "config"));
                url = gitConfig["remote \"origin\""]["url"];
            }
            else
            {
                string zipPath = Path.Combine(CurrentDirectory, Config.FileNameGitZip);
                using ZipInputStream zip = new(File.OpenRead(zipPath));
                using FileStream filestream = new(zipPath, FileMode.Open, FileAccess.Read);
                ZipFile zipfile = new(filestream);
                ZipEntry item;
                while ((item = zip.GetNextEntry()) != null)
                {
                    if (item.Name == "config")
                    {
                        using StreamReader s = new(zipfile.GetInputStream(item));
                        FileIniDataParser parser = new();
                        IniData gitConfig = parser.ReadData(s);
                        url = gitConfig["remote \"origin\""]["url"];
                        break;
                    }
                }
            }

            return url;
        }

        /// <summary>
        /// Add a state to the history.
        /// </summary>
        public void UpdateHistory()
        {
            History.Add(new WerkStateTimestamp()
            {
                Timestamp = DateTime.Now,
                State = this.State,
                Environment = this.Environment
            });
        }
    }
}
