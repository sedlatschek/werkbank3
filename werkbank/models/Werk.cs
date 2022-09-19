﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.converters;
using werkbank.environments;
using werkbank.repositories;
using werkbank.services;
using werkbank.transitions;
using static System.Windows.Forms.AxHost;

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
        public readonly string Title;

        [JsonProperty("state")]
        public WerkState State = WerkState.Hot;

        [JsonProperty("env"), JsonConverter(typeof(EnvironmentConverter))]
        public readonly environments.Environment Environment;

        [JsonProperty("compressOnArchive")]
        public bool CompressOnArchive = true;

        [JsonProperty("created")]
        public readonly DateTime CreatedAt;

        [JsonProperty("moving")]
        public bool Moving = false;

        [JsonProperty("history")]
        public readonly List<WerkStateTimestamp> History = new();

        [JsonIgnore]
        public DateTime LastModified { get; set; }

        public Werk(string Name, string Title, environments.Environment Environment)
        {
            this.Name = Name;
            this.Title = Title;
            this.Environment = Environment;
            CreatedAt = DateTime.Now;
        }

        [JsonIgnore]
        public Batch? CurrentBatch;

        [JsonIgnore]
        public bool HasIcon
        {
            get
            {
                if (Environment == null)
                    return false;
                return File.Exists(IconFile);
            }
        }

        [JsonIgnore]
        public string IconFile
        {
            get
            {
                return Path.Combine(CurrentMetaDirectory, Config.FileNameWerkIcon);
            }
        }

        /// <summary>
        /// The path of the directory the werk currently resides.
        /// </summary>
        [JsonIgnore]
        public string CurrentDirectory
        {
            get
            {
                return GetDirectoryFor(State, Environment, Name);
            }
        }

        /// <summary>
        /// The path to the current meta directory (.werk) of the werk.
        /// </summary>
        [JsonIgnore]
        public string CurrentMetaDirectory
        {
            get
            {
                return Path.Combine(CurrentDirectory, Config.DirNameWerk);
            }
        }

        /// <summary>
        /// The path to the current werk file (.werk/werk.json).
        /// </summary>
        [JsonIgnore]
        public string CurrentWerkJson
        {
            get
            {
                return Path.Combine(CurrentMetaDirectory, Config.FileNameWerkJson);
            }
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
        public void OpenExplorer()
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
    }
}