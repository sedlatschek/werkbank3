using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace werkbank.operations
{
    public static class Unzip
    {
        public static bool Perform(OperationUnzipOptions Options)
        {
            FileInfo zipFile = new(Options.SourceZip);
            DirectoryInfo dir = Directory.CreateDirectory(Options.DestinationPath);

            using (ZipInputStream s = new(File.OpenRead(zipFile.FullName)))
            {
                ZipEntry entry;
                while ((entry = s.GetNextEntry()) != null)
                {
                    string? directoryName = Path.GetDirectoryName(entry.Name);
                    string fileName = Path.GetFileName(entry.Name);

                    if (!string.IsNullOrEmpty(directoryName))
                    {
                        Directory.CreateDirectory(Path.Combine(dir.FullName, directoryName));
                    }

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        string targetFile = Path.Combine(dir.FullName, entry.Name);
                        using (FileStream streamWriter = File.Create(targetFile))
                        {
                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                    streamWriter.Write(data, 0, size);
                                else break;
                            }
                        }
                    }
                }
            }

            return true;
        }

        public static bool Verify(OperationUnzipOptions Options)
        {
            FileInfo zipFile = new(Options.SourceZip);
            DirectoryInfo dir = new(Options.DestinationPath);

            if (!dir.Exists)
            {
                return false;
            }

            using (ZipInputStream s = new(File.OpenRead(zipFile.FullName)))
            {
                ZipEntry entry;
                while ((entry = s.GetNextEntry()) != null)
                {
                    string? directoryName = Path.GetDirectoryName(entry.Name);
                    string fileName = Path.GetFileName(entry.Name);

                    if (!string.IsNullOrEmpty(directoryName))
                    {
                        string destDir = Path.Combine(dir.FullName, directoryName);
                        if (!Directory.Exists(destDir))
                        {
                            return false;
                        }
                    }

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        string destFile = Path.Combine(dir.FullName, entry.Name);
                        if (!File.Exists(destFile) || (new FileInfo(destFile)).Length != entry.Size)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
