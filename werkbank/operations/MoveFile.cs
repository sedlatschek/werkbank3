using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace werkbank.operations
{
    public static class MoveFile
    {
        public static bool Perform(OperationMoveFileOptions Options)
        {
            if (File.Exists(Options.DestinationPath))
            {
                File.Delete(Options.DestinationPath);
            }
            File.Move(Options.SourcePath, Options.DestinationPath, true);
            return true;
        }

        public static bool Verify(OperationMoveFileOptions Options)
        {
            return !File.Exists(Options.SourcePath) && File.Exists(Options.DestinationPath);
        }
    }
}
