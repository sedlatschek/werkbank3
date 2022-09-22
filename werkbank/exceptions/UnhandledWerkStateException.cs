using werkbank.models;

namespace werkbank.exceptions
{
    internal class UnhandledWerkStateException : Exception
    {
        public UnhandledWerkStateException(WerkState State) : base("The state " + State.ToString() + " is not handled.") { }
    }
}
