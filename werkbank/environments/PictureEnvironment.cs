namespace werkbank.environments
{
    public class PictureEnvironment : Environment
    {
        public PictureEnvironment(int Index) : base(Index) { }

        public override string Name => "Picture";
        public override string Handle => "picture";
        public override string Directory => "Bild";

        public override EnvironmentPreset Preset => new(
            CompressOnArchive: true
        );
    }
}
