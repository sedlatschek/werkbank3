namespace werkbank.environments
{
    public class CadEnvironment : Environment
    {
        public CadEnvironment(int Index) : base(Index) { }

        public override string Name => "CAD";

        public override string Handle => "cad";

        public override string Directory => "CAD";

        public override EnvironmentPreset Preset => new(
            CompressOnArchive: true
        );
    }
}
