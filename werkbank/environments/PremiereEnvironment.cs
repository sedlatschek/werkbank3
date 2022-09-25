namespace werkbank.environments
{
    public class PremiereEnvironment : Environment
    {
        public PremiereEnvironment(int Index) : base(Index) { }

        public override string Name => "Premiere";
        public override string Handle => "premiere";
        public override string Directory => "Video";

        public override EnvironmentPreset Preset => new(
            CompressOnArchive: false
        );
    }
}
