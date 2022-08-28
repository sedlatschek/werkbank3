using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace werkbank.operations
{
    public static class Unhide
    {
        public static bool Perform(OperationUnhideOptions Options)
        {
            FileAttributes attributes = File.GetAttributes(Options.TargetPath);
            if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
            {
                attributes &= ~FileAttributes.Hidden;
                File.SetAttributes(Options.TargetPath, attributes);
            }
            return true;
        }

        public static bool Verify(OperationUnhideOptions Options)
        {
            FileAttributes attributes = File.GetAttributes(Options.TargetPath);
            return !attributes.HasFlag(FileAttributes.Hidden);
        }
    }
}
