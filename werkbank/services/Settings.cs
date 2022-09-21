using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace werkbank.services
{
    public class Settings
    {
        public static readonly Settings Properties = Init();

        [JsonProperty("autoStart")]
        public bool AutoStart { get; set; } = false;

        [JsonProperty("launchMinimized")]
        public bool LaunchMinimized { get; set; } = false;

        [JsonProperty("gatherAtLaunch")]
        public bool GatherAtLaunch { get; set; } = true;

        [JsonProperty("dirHotVault")]
        public string DirHotVault { get; set; }

        [JsonProperty("dirColdVault")]
        public string DirColdVault { get; set; }

        [JsonProperty("dirArchiveVault")]
        public string DirArchiveVault { get; set; }

        [JsonProperty("archiveCompressionLevel")]
        public int ArchiveCompressionLevel { get; set; } = 9;

        [JsonProperty("queueTickInterval")]
        public int QueueTickInterval { get; set; } = 1000;

        [JsonProperty("operationRetryTimeout")]
        public int OperationRetryTimeout { get; set; } = 60000;

        /// <summary>
        /// Retrieve the latest settings if possible or create new settings.
        /// </summary>
        /// <returns></returns>
        private static Settings Init()
        {
            if (File.Exists(Config.FileSettings))
            {
                string str = File.ReadAllText(Config.FileSettings);
                if (!string.IsNullOrEmpty(str))
                {
                    Settings? settings = JsonConvert.DeserializeObject<Settings>(str);
                    if (settings != null)
                    {
                        return ApplyDefaultsOnEmpty(settings);
                    }
                }
            }
            return ApplyDefaultsOnEmpty(new Settings());
        }

        public Settings()
        {
            DirHotVault = Config.DirDefaultHotVault;
            DirColdVault = Config.DirDefaultColdVault;
            DirArchiveVault = Config.DirDefaultArchiveVault;
        }

        private static Settings ApplyDefaultsOnEmpty(Settings Settings)
        {
            if (string.IsNullOrEmpty(Settings.DirHotVault))
            {
                Settings.DirHotVault = Config.DirDefaultHotVault;
            }
            if (string.IsNullOrEmpty(Settings.DirColdVault))
            {
                Settings.DirColdVault = Config.DirDefaultColdVault;
            }
            if (string.IsNullOrEmpty(Settings.DirArchiveVault))
            {
                Settings.DirArchiveVault = Config.DirDefaultArchiveVault;
            }
            if (Settings.ArchiveCompressionLevel < 0 || Settings.ArchiveCompressionLevel > 9)
            {
                Settings.ArchiveCompressionLevel = 9;
            }
            if (Settings.OperationRetryTimeout < 0)
            {
                Settings.OperationRetryTimeout = 60000;
            }
            if (Settings.QueueTickInterval < 0)
            {
                Settings.OperationRetryTimeout = 1000;
            }
            return Settings;
        }

        /// <summary>
        /// Write the settings onto the hard drive.
        /// </summary>
        /// <param name="Settings"></param>
        public static void Save()
        {
            File.WriteAllText(Config.FileSettings, JsonConvert.SerializeObject(Properties));
        }
    }
}
