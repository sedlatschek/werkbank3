using werkbank.models;

namespace werkbank.exceptions
{
    public class UnexpectedWerkDirectoryNameException : Exception
    {
        public UnexpectedWerkDirectoryNameException(Werk Werk, string WerkPath) : base("Werk \"" + Werk.Name + "\" is located in \"" + WerkPath + "\" but should be located in \"" + Werk.CurrentDirectory + "\"") { }
    }
}
