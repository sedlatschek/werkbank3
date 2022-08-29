using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace werkbank.services
{
    public class Settings
    {
        public static readonly Settings Properties = Init();

        [JsonProperty("archiveCompressionLevel")]
        public int ArchiveCompressionLevel { get; set; }

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
                        return settings;
                    }
                }
            }
            return new Settings()
            {
                ArchiveCompressionLevel = 9
            };
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
