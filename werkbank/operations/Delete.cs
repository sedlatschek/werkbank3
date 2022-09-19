﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.exceptions;

namespace werkbank.operations
{
    public static class Delete
    {
        public static bool Perform(string? DestinationPath)
        {
            if (DestinationPath == null)
            {
                throw new OperationParametersMissingException();
            }

            if (Directory.Exists(DestinationPath) || File.Exists(DestinationPath))
            {
                FileAttributes attributes = File.GetAttributes(DestinationPath);
                if (attributes.HasFlag(FileAttributes.Directory))
                {
                    Directory.Delete(DestinationPath, true);
                }
                else
                {
                    File.Delete(DestinationPath);
                }
            }

            return true;
        }

        public static bool Verify(string? DestinationPath)
        {
            if (DestinationPath == null)
            {
                throw new OperationParametersMissingException();
            }

            return !Directory.Exists(DestinationPath) && !File.Exists(DestinationPath);
        }
    }
}
