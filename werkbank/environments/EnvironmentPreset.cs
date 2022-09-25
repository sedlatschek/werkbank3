namespace werkbank.environments
{
    public class EnvironmentPreset
    {
        public bool? CompressOnArchive { get; }

        public EnvironmentPreset(bool? CompressOnArchive)
        {
            this.CompressOnArchive = CompressOnArchive;
        }
    }
}
