using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using werkbank.exceptions;

namespace werkbank.operations
{
    public static class Copy
    {
        public static bool Perform(string? SourcePath, string? DestinationPath)
        {
            if (SourcePath == null || DestinationPath == null)
            {
                throw new OperationParametersMissingException();
            }

            if (Directory.Exists(DestinationPath))
            {
                Directory.Delete(DestinationPath, true);
            }
            else if (File.Exists(DestinationPath))
            {
                File.Delete(DestinationPath);
            }


            CopyDirOrFile(SourcePath, DestinationPath);
            return true;
        }

        /// <summary>
        /// Copy a given file or directory to a given destination.
        /// </summary>
        /// <param name="SourcePath"></param>
        /// <param name="DestinationPath"></param>
        private static void CopyDirOrFile(string SourcePath, string DestinationPath)
        {
            FileAttributes attributes = File.GetAttributes(SourcePath);

            if (attributes.HasFlag(FileAttributes.Directory))
            {
                Directory.CreateDirectory(DestinationPath);

                DirectoryInfo dir = new(SourcePath);
                DirectoryInfo[] dirs = dir.GetDirectories();

                foreach (FileInfo file in dir.GetFiles())
                {
                    string targetFilePath = Path.Combine(DestinationPath, file.Name);
                    file.CopyTo(targetFilePath);
                }

                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(DestinationPath, subDir.Name);
                    CopyDirOrFile(subDir.FullName, newDestinationDir);
                }
            }
            else
            {
                FileInfo file = new(SourcePath);
                file.CopyTo(DestinationPath);
            }
        }

        public static bool Verify(string? SourcePath, string? DestinationPath)
        {
            if (SourcePath == null || DestinationPath == null)
            {
                throw new OperationParametersMissingException();
            }
            return IsEqual(SourcePath, DestinationPath) && IsEqual(DestinationPath, SourcePath);
        }

        /// <summary>
        /// Determine whether two paths have the same contents.
        /// </summary>
        /// <param name="SourcePath"></param>
        /// <param name="DestinationPath"></param>
        /// <returns></returns>
        private static bool IsEqual(string SourcePath, string DestinationPath)
        {
            FileAttributes sourceAttributes = File.GetAttributes(SourcePath);
            FileAttributes destinationAttributes = File.GetAttributes(SourcePath);

            if (sourceAttributes.HasFlag(FileAttributes.Directory) != destinationAttributes.HasFlag(FileAttributes.Directory))
            {
                return false;
            }

            if (sourceAttributes.HasFlag(FileAttributes.Directory))
            {
                if (!Directory.Exists(DestinationPath))
                {
                    return false;
                }

                DirectoryInfo dir = new(SourcePath);
                DirectoryInfo[] dirs = dir.GetDirectories();

                foreach (FileInfo file in dir.GetFiles())
                {
                    string targetFilePath = Path.Combine(DestinationPath, file.Name);
                    if (!File.Exists(targetFilePath) || GetMD5(targetFilePath) != GetMD5(file.FullName))
                    {
                        return false;
                    }
                }

                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(DestinationPath, subDir.Name);
                    if (!IsEqual(subDir.FullName, newDestinationDir))
                    {
                        return false;
                    }
                }
            }
            else
            {
                return File.Exists(SourcePath) && File.Exists(DestinationPath) && GetMD5(SourcePath) == GetMD5(DestinationPath);
            }

            return true;
        }

        /// <summary>
        /// Get the MD5 hash of a file.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        private static string GetMD5(string FilePath)
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
