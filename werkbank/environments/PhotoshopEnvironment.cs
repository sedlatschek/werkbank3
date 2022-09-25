namespace werkbank.environments
{
    public class PhotoshopEnvironment : Environment
    {
        public PhotoshopEnvironment(int Index) : base(Index) { }

        public override string Name => "Photoshop";
        public override string Handle => "ps";
        public override string Directory => "Bild";

        public override EnvironmentPreset Preset => new(
            CompressOnArchive: true
        );
    }
}
