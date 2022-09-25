namespace werkbank.environments
{
    public class WmmEnvironment : Environment
    {
        public WmmEnvironment(int Index) : base(Index) { }

        public override string Name => "Windows Movie Maker";
        public override string Handle => "wmm";
        public override string Directory => "Video";

        public override EnvironmentPreset Preset => new(
            CompressOnArchive: false
        );
    }
}
