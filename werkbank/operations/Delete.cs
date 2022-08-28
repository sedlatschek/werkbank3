using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace werkbank.operations
{
    public static class Delete
    {
        public static bool Perform(OperationDeleteOptions Options)
        {
            FileAttributes attributes = File.GetAttributes(Options.TargetPath);
            if (attributes.HasFlag(FileAttributes.Directory))
            {
                Directory.Delete(Options.TargetPath, true);
            }
            else
            {
                File.Delete(Options.TargetPath);
            }
            return true;
        }

        public static bool Verify(OperationDeleteOptions Options)
        {
            return !Directory.Exists(Options.TargetPath) && !File.Exists(Options.TargetPath);
        }
    }
}
