using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.exceptions;
using werkbank.services;

namespace werkbank.operations
{
    public static class Delete
    {
        public static bool Perform(string? DestinationPath)
        {
            if (DestinationPath == null)
            {
                throw new OperationParametersMissingException();
            }

            if (Directory.Exists(DestinationPath) || File.Exists(DestinationPath))
            {
                FileAttributes attributes = File.GetAttributes(DestinationPath);
                if (attributes.HasFlag(FileAttributes.Directory))
                {
                    DeleteDirectory(DestinationPath);
                }
                else
                {
                    DeleteFile(DestinationPath);
                }
            }

            return true;
        }

        /// <summary>
        /// Delete a file from a given location.
        /// </summary>
        /// <param name="TargetPath"></param>
        private static void DeleteFile(string TargetPath)
        {
            File.SetAttributes(TargetPath, FileAttributes.Normal);
            File.Delete(TargetPath);
        }

        /// <summary>
        /// Delete a dirctory and all its contents from a given location.
        /// </summary>
        /// <param name="TargetDirectory"></param>
        private static void DeleteDirectory(string TargetDirectory)
        {
            File.SetAttributes(TargetDirectory, FileAttributes.Normal);

            if (FileService.IsSymbolic(TargetDirectory))
            {
                // symlinks do not seem to work with Directory.Delete nor File.Delete,
                // therefor we use the shellcomand rm to delete it
                FileService.ShellDeleteFile(TargetDirectory);
            }
            else
            {
                foreach (string file in Directory.GetFiles(TargetDirectory))
                {
                    DeleteFile(file);
                }
                foreach (string dir in Directory.GetDirectories(TargetDirectory))
                {
                    DeleteDirectory(dir);
                }

                Directory.Delete(TargetDirectory, false);
            }
        }

        public static bool Verify(string? DestinationPath)
        {
            if (DestinationPath == null)
            {
                throw new OperationParametersMissingException();
            }

            return !Directory.Exists(DestinationPath) && !File.Exists(DestinationPath);
        }
    }
}
