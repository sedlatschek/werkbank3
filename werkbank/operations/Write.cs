using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace werkbank.operations
{
    public static class Write
    {
        public static bool Perform(OperationWriteOptions Options)
        {
            File.WriteAllText(Options.DestinationPath, Options.Content);
            return true;
        }

        public static bool Verify(OperationWriteOptions Options)
        {
            return File.Exists(Options.DestinationPath)
                && File.ReadAllText(Options.DestinationPath) == Options.Content;
        }
    }
}
