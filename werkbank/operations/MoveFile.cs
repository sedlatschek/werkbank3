using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using werkbank.exceptions;

namespace werkbank.operations
{
    public static class MoveFile
    {
        public static bool Perform(string? SourcePath, string? DestinationPath)
        {
            if (SourcePath == null || DestinationPath == null)
            {
                throw new OperationParametersMissingException();
            }

            if (File.Exists(DestinationPath))
            {
                File.Delete(DestinationPath);
            }
            File.Move(SourcePath, DestinationPath, true);
            return true;
        }

        public static bool Verify(string? SourcePath, string? DestinationPath)
        {
            if (SourcePath == null || DestinationPath == null)
            {
                throw new OperationParametersMissingException();
            }

            return !File.Exists(SourcePath) && File.Exists(DestinationPath);
        }
    }
}
