namespace werkbank.environments
{
    public class VegasEnvironment : Environment
    {
        public VegasEnvironment(int Index) : base(Index) { }

        public override string Name => "Vegas";
        public override string Handle => "vegas";
        public override string Directory => "Video";

        public override EnvironmentPreset Preset => new(
            CompressOnArchive: false
        );
    }
}
