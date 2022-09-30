namespace werkbank.exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string Message) : base(Message) { }
    }
}
