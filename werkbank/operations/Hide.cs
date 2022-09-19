using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.exceptions;

namespace werkbank.operations
{
    public static class Hide
    {
        public static bool Perform(string? DestinationPath)
        {
            if (DestinationPath == null)
            {
                throw new OperationParametersMissingException();
            }

            FileAttributes attributes = File.GetAttributes(DestinationPath);
            if ((attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
            {
                attributes |= FileAttributes.Hidden;
                File.SetAttributes(DestinationPath, attributes);
            }
            return true;
        }

        public static bool Verify(string? DestinationPath)
        {
            if (DestinationPath == null)
            {
                throw new OperationParametersMissingException();
            }

            FileAttributes attributes = File.GetAttributes(DestinationPath);
            return attributes.HasFlag(FileAttributes.Hidden);
        }
    }
}
