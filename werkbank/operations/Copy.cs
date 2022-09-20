using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using werkbank.exceptions;
using werkbank.services;

namespace werkbank.operations
{
    public static class Copy
    {
        public static bool Perform(string? SourcePath, string? DestinationPath)
        {
            return Perform(SourcePath, DestinationPath, null);
        }

        public static bool Perform(string? SourcePath, string? DestinationPath, IEnumerable<string>? IgnoreList)
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

            CopyDirOrFile(SourcePath, DestinationPath, IgnoreList ?? new List<string>());
            return true;
        }

        /// <summary>
        /// Copy a given file or directory to a given destination.
        /// </summary>
        /// <param name="SourcePath"></param>
        /// <param name="DestinationPath"></param>
        /// <param name="IgnoreList"></param>
        private static void CopyDirOrFile(string SourcePath, string DestinationPath, IEnumerable<string> IgnoreList)
        {
            if (IgnoreList.Contains(SourcePath))
            {
                return;
            }

            FileAttributes attributes = File.GetAttributes(SourcePath);

            if (attributes.HasFlag(FileAttributes.Directory))
            {
                Directory.CreateDirectory(DestinationPath);

                DirectoryInfo dir = new(SourcePath);
                DirectoryInfo[] dirs = dir.GetDirectories();

                foreach (FileInfo file in dir.GetFiles())
                {
                    if (IgnoreList.Contains(file.FullName))
                    {
                        continue;
                    }

                    string targetFilePath = Path.Combine(DestinationPath, file.Name);
                    file.CopyTo(targetFilePath);
                }

                foreach (DirectoryInfo subDir in dirs)
                {
                    if (IgnoreList.Contains(subDir.FullName))
                    {
                        continue;
                    }

                    string newDestinationDir = Path.Combine(DestinationPath, subDir.Name);
                    CopyDirOrFile(subDir.FullName, newDestinationDir, IgnoreList);
                }
            }
            else if (!IgnoreList.Contains(SourcePath))
            {
                FileInfo file = new(SourcePath);
                file.CopyTo(DestinationPath);
            }
        }

        public static bool Verify(string? SourcePath, string? DestinationPath)
        {
            return Verify(SourcePath, DestinationPath, null);
        }

        public static bool Verify(string? SourcePath, string? DestinationPath, IEnumerable<string>? IgnoreList)
        {
            if (SourcePath == null || DestinationPath == null)
            {
                throw new OperationParametersMissingException();
            }

            // we create a second ignore list for the destination path.
            // this way we can do a "reverse" run
            IEnumerable<string> sourceIgnoreList = IgnoreList ?? new List<string>();
            IEnumerable<string> destIgnoreList = sourceIgnoreList.Select(path => path.Replace(SourcePath, DestinationPath));

            return IsEqual(SourcePath, DestinationPath, sourceIgnoreList) && IsEqual(DestinationPath, SourcePath, destIgnoreList);
        }

        /// <summary>
        /// Determine whether two paths have the same contents.
        /// </summary>
        /// <param name="SourcePath"></param>
        /// <param name="DestinationPath"></param>
        /// <returns></returns>
        private static bool IsEqual(string SourcePath, string DestinationPath, IEnumerable<string> IgnoreList)
        {
            // if source does not exist, dest should not exist either
            if (!FileService.PathExists(SourcePath, out FileService.PathType? sourcePathType))
            {
                return FileService.PathExists(DestinationPath);
            }

            // if dest does not exists, it is either not equal to source or on the ignore list
            if (!FileService.PathExists(DestinationPath, out FileService.PathType? destPathType))
            {
                return IgnoreList.Contains(SourcePath);
            }

            // if the paths or of a different type, they are definitively not equal
            if (sourcePathType != destPathType)
            {
                return false;
            }

            // since both paths exist, source should not be in the ignore list
            if (IgnoreList.Contains(SourcePath))
            {
                return false;
            }

            if (sourcePathType == FileService.PathType.File)
            {
                // we know that both paths exist and are files, so to compare them, we only need to compare their MD5 hashes
                return FileService.GetMD5(SourcePath) == FileService.GetMD5(DestinationPath);
            }

            if (sourcePathType == FileService.PathType.Directory)
            {
                // we know that both paths exist and are directories, so we just need to feed their contents into the function recursively
                DirectoryInfo sourceDir = new(SourcePath);

                foreach (FileInfo file in sourceDir.GetFiles())
                {
                    string sourceFilePath = file.FullName;
                    string destFilePath = Path.Combine(DestinationPath, file.Name);
                    if (!IsEqual(sourceFilePath, destFilePath, IgnoreList))
                    {
                        return false;
                    }
                }

                foreach (DirectoryInfo dir in sourceDir.GetDirectories())
                {
                    string sourceDirPath = dir.FullName;
                    string destDirPath = Path.Combine(DestinationPath, dir.Name);

                    if (!IsEqual(sourceDirPath, destDirPath, IgnoreList))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
