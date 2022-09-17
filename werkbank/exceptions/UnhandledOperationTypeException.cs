using werkbank.operations;

namespace werkbank.exceptions
{
    internal class UnhandledOperationTypeException : Exception
    {
        public UnhandledOperationTypeException(OperationType Type) : base("The type " + Type.ToString() + " is not handled.") { }
    }
}
