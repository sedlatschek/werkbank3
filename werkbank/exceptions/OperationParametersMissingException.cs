using werkbank.models;

namespace werkbank.exceptions
{
    public class OperationParametersMissingException : Exception
    {
        public OperationParametersMissingException() : base("Operation is missing mandatory parameters") { }
    }
}
