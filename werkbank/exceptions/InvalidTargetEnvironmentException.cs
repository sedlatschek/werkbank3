namespace werkbank.exceptions
{
    public class InvalidTargetEnvironmentException: Exception
    {
        public InvalidTargetEnvironmentException(environments.Environment TargetEnvironment) : base("Environment \"" + TargetEnvironment.Name + "\" is invalid") { }
    }
}
