using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.exceptions;

namespace werkbank.operations
{
    public static class Write
    {
        public static bool Perform(string? Content, string? DestinationPath)
        {
            if (Content == null || DestinationPath == null)
            {
                throw new OperationParametersMissingException();
            }

            if (File.Exists(DestinationPath))
            {
                File.Delete(DestinationPath);
            }

            File.WriteAllText(DestinationPath, Content);
            return true;
        }

        public static bool Verify(string? Content, string? DestinationPath)
        {
            if (Content == null || DestinationPath == null)
            {
                throw new OperationParametersMissingException();
            }

            return File.Exists(DestinationPath)
                && File.ReadAllText(DestinationPath) == Content;
        }
    }
}
