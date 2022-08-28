using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.operations;

namespace werkbank.exceptions
{
    internal class UnhandledOperationTypeException : Exception
    {
        public UnhandledOperationTypeException(OperationType Type) : base("The type " + Type.ToString() + " is not handled.") { }
    }
}
