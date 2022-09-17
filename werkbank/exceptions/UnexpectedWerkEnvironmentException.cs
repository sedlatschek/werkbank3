using werkbank.models;

namespace werkbank.exceptions
{
    public class UnexpectedWerkEnvironmentException : Exception
    {
        public UnexpectedWerkEnvironmentException(Werk Werk, string EnvironmentPath) : base("Werk \"" + Werk.Name + "\" has environment \"" + Werk.Environment.Name + "\" but is located in \"" + EnvironmentPath + "\"") { }
    }
}
