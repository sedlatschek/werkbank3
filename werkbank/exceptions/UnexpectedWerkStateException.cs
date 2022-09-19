using werkbank.models;

namespace werkbank.exceptions
{
    public class UnexpectedWerkStateException : Exception
    {
        public UnexpectedWerkStateException(Werk Werk, WerkState ExpectedState) : base("Werk \"" + Werk.Name + "\" has state \"" + Werk.State + "\" but should have \"" + ExpectedState + "\"") { }
    }
}
