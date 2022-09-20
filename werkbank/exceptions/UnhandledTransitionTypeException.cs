using werkbank.transitions;

namespace werkbank.exceptions
{
    internal class UnhandledTransitionTypeException : Exception
    {
        public UnhandledTransitionTypeException(TransitionType Type) : base("The type " + Type.ToString() + " is not handled.") { }
    }
}
