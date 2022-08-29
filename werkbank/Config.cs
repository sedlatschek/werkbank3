using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace werkbank
{
    public static class Config
    {
        public const string AppHandle = "werkbank3";
        public const string DirNameWerk = ".werk";
        public const string FileNameWerkJson = "werk.json";
        public const string FileNameWerkIcon = "icon.png";
        public const string FileNameSettings = "settings.json";
        public const string FileNameQueue = "queue.json";
        
        public static readonly string DirAppData = Directory.CreateDirectory(Debugger.IsAttached ? "tmp" : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppHandle)).FullName;
        public static readonly string FileSettings = Path.Combine(DirAppData, FileNameSettings);
        public static readonly string FileQueue = Path.Combine(DirAppData, FileNameQueue);
    }
}
