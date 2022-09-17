using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace werkbank
{
    public static class Config
    {
        /// <summary>
        /// Whether or not the current instance is executed as part of testing.
        /// </summary>
        public static readonly bool IsTestEnvironment = AppDomain.CurrentDomain.GetAssemblies().Any(a => a.FullName != null && a.FullName.Contains("Microsoft.VisualStudio.TestPlatform"));

        /// <summary>
        /// Whether or not the current instance is executed as part of debugging.
        /// </summary>
        public static readonly bool IsDebugEnvironment = Debugger.IsAttached;

        public const string AppHandle = "werkbank3";
        public const string DirNameWerk = ".werk";
        public const string DirNameTests = "werkbank_tests";
        public const string FileNameWerkJson = "werk.json";
        public const string FileNameWerkIcon = "icon.png";
        public const string FileNameSettings = "settings.json";
        public const string FileNameQueue = "queue.json";

        public static readonly string DirAppData = Directory.CreateDirectory(GetAppDataPath()).FullName;
        public static readonly string FileSettings = Path.Combine(DirAppData, FileNameSettings);
        public static readonly string FileQueue = Path.Combine(DirAppData, FileNameQueue);

        public static readonly string DirDefaultHotVault = Path.Combine(DirAppData, "hot");
        public static readonly string DirDefaultColdVault = Path.Combine(DirAppData, "cold");
        public static readonly string DirDefaultArchiveVault = Path.Combine(DirAppData, "archive");


        /// <summary>
        /// Get the application data path for the current environment.
        /// </summary>
        /// <returns></returns>
        private static string GetAppDataPath()
        {
            if (IsDebugEnvironment)
            {
                Console.WriteLine("Initialize for debugging...");
                return "../../../../tmp";
            }
            if (IsTestEnvironment)
            {
                Console.WriteLine("Initialize for testing...");
                return Path.Combine(Path.GetTempPath(), DirNameTests, Guid.NewGuid().ToString());
            }
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppHandle);
        }
    }
}
