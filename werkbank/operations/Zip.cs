using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace werkbank.operations
{
    public static class Zip
    {
        public static bool Perform(OperationZipOptions Options)
        {
            ZipDir(new DirectoryInfo(Options.SourcePath), new FileInfo(Options.DestinationPath));
            return true;
        }

        /// <summary>
        /// Create a zip file of a given directory.
        /// </summary>
        /// <param name="Dir"></param>
        /// <param name="DestinationFile"></param>
        private static void ZipDir(DirectoryInfo Dir, FileInfo DestinationFile)
        {
            FileStream fileStream = File.Create(DestinationFile.FullName);
            ZipOutputStream zipStream = new(fileStream);

            // compression level (0-9)
            zipStream.SetLevel(Config.CompressionLevel);

            int offset = Dir.FullName.Length;

            try
            {
                ZipAddDir(Dir, zipStream, offset);
            }
            finally
            {
                zipStream.IsStreamOwner = true;
                zipStream.Close();
            }
        }

        /// <summary>
        /// Add a directory to given zip stream.
        /// </summary>
        /// <param name="Dir"></param>
        /// <param name="ZipStream"></param>
        /// <param name="Offset"></param>
        private static void ZipAddDir(DirectoryInfo Dir, ZipOutputStream ZipStream, int Offset)
        {
            foreach (FileInfo file in Dir.GetFiles())
            {
                ZipAddFile(file, ZipStream, Offset);
            }
            foreach (DirectoryInfo subDir in Dir.GetDirectories())
            {
                ZipAddDir(subDir, ZipStream, Offset);
            }
        }

        /// <summary>
        /// Add a file to a given zip stream.
        /// </summary>
        /// <param name="FileInfo"></param>
        /// <param name="ZipStream"></param>
        /// <param name="Offset"></param>
        private static void ZipAddFile(FileInfo FileInfo, ZipOutputStream ZipStream, int Offset)
        {
            // Name in zip based on the folder
            string entryName = FileInfo.FullName[Offset..];

            // Remove drive from name and fixes slash direction
            entryName = ZipEntry.CleanName(entryName);
            ZipEntry newEntry = new(entryName)
            {
                // Note the zip format stores 2 second granularity
                DateTime = FileInfo.LastWriteTime,

                // Specifying the AESKeySize triggers AES encryption. Allowable values are 0 (off), 128 or 256.
                // A password on the ZipOutputStream is required if using AES.
                // if (encryption)
                //     newEntry.AESKeySize = 256;

                // To permit the zip to be unpacked by built-in extractor in WinXP and Server2003, WinZip 8, Java, and other older code,
                // you need to do one of the following: Specify UseZip64.Off, or set the Size.
                // If the file may be bigger than 4GB, or you do not need WinXP built-in compatibility, you do not need either,
                // but the zip will be in Zip64 format which not all utilities can understand.
                //   zipStream.UseZip64 = UseZip64.Off;
                Size = FileInfo.Length
            };

            ZipStream.PutNextEntry(newEntry);

            // Zip the file in buffered chunks
            // the "using" will close the stream even if an exception occurs
            byte[] buffer = new byte[4096];
            using (FileStream streamReader = File.OpenRead(FileInfo.FullName))
            {
                StreamUtils.Copy(streamReader, ZipStream, buffer);
            }
            ZipStream.CloseEntry();
        }

        public static bool Verify(OperationZipOptions Options)
        {
            if (!File.Exists(Options.DestinationPath))
            {
                return false;
            }

            ZipContents zipContents = GetZipContents(Options.DestinationPath);

            DirectoryInfo dir = new(Options.SourcePath);
            return FilesExistInZip(ref zipContents, dir)
                && zipContents.Directories.Count == 0
                && zipContents.Files.Count == 0;
        }

        /// <summary>
        /// Determine whether or not the files and directories of a given directory are present in the given zip contents.
        /// </summary>
        /// <param name="ZipContents"></param>
        /// <param name="Directory"></param>
        /// <param name="Prefix"></param>
        /// <returns></returns>
        private static bool FilesExistInZip(ref ZipContents ZipContents, DirectoryInfo Directory, string Prefix = "")
        {
            if (!Directory.Exists)
            {
                return false;
            }

            foreach (FileInfo file in Directory.GetFiles())
            {
                string fileName = Path.Combine(Prefix, file.Name);
                if (!ZipContents.Files.TryGetValue(fileName, out long FileSize) || FileSize != file.Length)
                {
                    return false;
                }
                else
                {

                    ZipContents.Files.Remove(fileName);
                }
            }

            foreach (DirectoryInfo subDir in Directory.GetDirectories())
            {
                string dirName = Path.Combine(Prefix, subDir.Name);
                if (ZipContents.Directories.Contains(dirName))
                {
                    ZipContents.Directories.Remove(dirName);
                    return FilesExistInZip(ref ZipContents, subDir, dirName);
                }
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get a list of all contents of a given Zip file.
        /// </summary>
        /// <param name="ZipFilePath"></param>
        /// <returns></returns>
        private static ZipContents GetZipContents(string ZipFilePath)
        {
            ZipContents contents = new();
            FileInfo zipFile = new(ZipFilePath);
            using (ZipInputStream s = new(File.OpenRead(zipFile.FullName)))
            {
                ZipEntry entry;

                while ((entry = s.GetNextEntry()) != null)
                {
                    string? directoryName = Path.GetDirectoryName(entry.Name);
                    string fileName = Path.GetFileName(entry.Name);

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        contents.Files.Add(entry.Name.Replace('/', '\\'), entry.Size);
                    }

                    if (!string.IsNullOrEmpty(directoryName))
                    {
                        contents.Directories.Add(directoryName.Replace('/', '\\'));
                    }
                }
            }
            return contents;
        }
    }

    internal class ZipContents
    {

        public readonly HashSet<string> Directories;
        public readonly Dictionary<string, long> Files;

        public ZipContents()
        {
            Directories = new HashSet<string>();
            Files = new Dictionary<string, long>();
        }
    }
}
