using werkbank.models;
using werkbank.transitions;

namespace werkbank.exceptions
{
    public class WerkIsAlreadyTransitioningException : Exception
    {
        public WerkIsAlreadyTransitioningException(Batch Batch) : base("Werk \"" + Batch.Werk?.Name ?? "Unknown" + "\" is already transitioning: " + Batch.Title) { }

        public WerkIsAlreadyTransitioningException(Werk Werk) : base("Werk \"" + Werk.Name ?? "Unknown" + "\" is already transitioning: " + Werk.TransitionType.ToString()) { }
    }
}
