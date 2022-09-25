namespace werkbank.environments
{
    public class UmlEnvironment : Environment
    {
        public UmlEnvironment(int Index) : base(Index) { }

        public override string Name => "UML";
        public override string Handle => "uml";
        public override string Directory => "code\\uml";

        public override EnvironmentPreset Preset => new(
            CompressOnArchive: true
        );
    }
}
