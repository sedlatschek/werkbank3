using werkbank.models;

namespace werkbank.environments
{
    public class DockerEnvironment : Environment
    {
        public DockerEnvironment(int Index) : base(Index) { }

        public override string Name => "Docker";
        public override string Handle => "docker";
        public override string Directory => "code\\docker";

        public override EnvironmentPreset Preset => new(
            CompressOnArchive: true
        );

        public override bool Created(Werk Werk)
        {
            WriteEditorConfig(Werk, Properties.Resources.default_editorconfig);
            return true;
        }
    }
}
