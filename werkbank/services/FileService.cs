using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace werkbank.services
{
    public static class FileService
    {
        public enum PathType
        {
            Directory,
            File
        }

        /// <summary>
        /// Determine whether or not a path exists and output its type.
        /// </summary>
        /// <param name="FileOrDirPath"></param>
        /// <param name="PathType"></param>
        /// <returns></returns>
        public static bool PathExists(string FileOrDirPath, out PathType? PathType)
        {
            PathType = null;
            if (Directory.Exists(FileOrDirPath))
            {
                PathType = FileService.PathType.Directory;
                return true;
            }
            if (File.Exists(FileOrDirPath))
            {
                PathType = FileService.PathType.File;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determine whether or not a path exists.
        /// </summary>
        /// <param name="FileOrDirPath"></param>
        /// <returns></returns>
        public static bool PathExists(string FileOrDirPath)
        {
            return PathExists(FileOrDirPath, out PathType? _);
        }

        /// <summary>
        /// Get the MD5 hash of a file.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static string GetMD5(string FilePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(FilePath))
                {
                    return Encoding.Default.GetString(md5.ComputeHash(stream));
                }
            }
        }
    }
}
