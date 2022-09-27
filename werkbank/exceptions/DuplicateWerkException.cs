namespace werkbank.exceptions
{
    public class DuplicateWerkException : Exception
    {
        public DuplicateWerkException(Guid Id) : base("A werk with the id \"" + Id.ToString() + "\" already exists") { }

        public DuplicateWerkException(string Name) : base("A werk with the name \"" + Name + "\" already exists") { }
    }
}
