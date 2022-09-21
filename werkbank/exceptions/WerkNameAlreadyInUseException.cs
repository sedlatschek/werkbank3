using werkbank.models;

namespace werkbank.exceptions
{
    public class WerkNameAlreadyInUseException : Exception
    {
        public WerkNameAlreadyInUseException(string Name) : base("A werk with the name " + Name + " already exists") { }
    }
}
