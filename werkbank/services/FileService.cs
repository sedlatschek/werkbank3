using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

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

        /// <summary>
        /// Replaces invalid characters from a filesystem path with an underscore.
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static string ReplaceInvalidCharsFromPath(string FileName)
        {
            return string.Join("_", FileName.Split(Path.GetInvalidFileNameChars()));
        }

        /// <summary>
        /// Determine whether or not a path is actually a symlink.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsSymbolic(string path)
        {
            FileInfo pathInfo = new(path);
            return pathInfo.Attributes.HasFlag(FileAttributes.ReparsePoint);
        }

        /// <summary>
        /// Execute a command in shell.
        /// </summary>
        /// <param name="Command"></param>
        /// <exception cref="ExternalException"></exception>
        private static void Shell(string Command)
        {
            System.Diagnostics.Process process = new();
            System.Diagnostics.ProcessStartInfo startInfo = new()
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = "/C " + Command
            };
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new ExternalException("Shell exited with code " + process.ExitCode.ToString());
            }
        }

        /// <summary>
        /// Delete a file using the shell command rm.
        /// </summary>
        /// <param name="FilePath"></param>
        public static void ShellDeleteFile(string FilePath)
        {
            Shell("rm \"" + FilePath + "\"");
        }

        /// <summary>
        /// Delete a directory using the shell command rmdir.
        /// </summary>
        /// <param name="DirectoryPath"></param>
        public static void ShellDeleteDirectory(string DirectoryPath)
        {
            Shell("rmdir \"" + DirectoryPath + "\"");
        }

        /// <summary>
        /// Get the hidden paths in a given directory.
        /// </summary>
        /// <param name="Directory"></param>
        /// <returns></returns>
        public static List<string> GetHiddenPaths(DirectoryInfo Directory)
        {
            List<string> paths = new();

            foreach (FileInfo file in Directory.GetFiles())
            {
                if (file.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    paths.Add(file.FullName);
                }
            }

            foreach (DirectoryInfo subDir in Directory.GetDirectories())
            {
                if (subDir.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    paths.Add(subDir.FullName);
                }
                else
                {
                    paths.AddRange(GetHiddenPaths(subDir));
                }
            }

            return paths;
        }

        /// <summary>
        /// Get the hidden paths in a given directory.
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static List<string> GetHiddenPaths(string Path)
        {
            return GetHiddenPaths(new DirectoryInfo(Path));
        }
    }
}
