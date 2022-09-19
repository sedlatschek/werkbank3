using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using werkbank.exceptions;

namespace werkbank.operations
{
    public static class CreateDirectory
    {
        public static bool Perform(string? DestinationPath)
        {
            if (DestinationPath == null)
            {
                throw new OperationParametersMissingException();
            }
            Directory.CreateDirectory(DestinationPath);
            return true;
        }

        public static bool Verify(string? DestinationPath)
        {
            if (DestinationPath == null)
            {
                throw new OperationParametersMissingException();
            }
            return Directory.Exists(DestinationPath);
        }
    }
}
