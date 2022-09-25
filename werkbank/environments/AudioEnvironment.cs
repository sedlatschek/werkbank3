namespace werkbank.environments
{
    public class AudioEnvironment : Environment
    {
        public AudioEnvironment(int Index) : base(Index) { }

        public override string Name => "Audio";

        public override string Handle => "audio";

        public override string Directory => "Audio";

        public override EnvironmentPreset Preset => new(
            CompressOnArchive: true
        );
    }
}
