using werkbank.models;

namespace werkbank.environments
{
    public class TerraformEnvironment : Environment
    {
        public TerraformEnvironment(int Index) : base(Index) { }

        public override string Name => "Terraform";
        public override string Handle => "tf";
        public override string Directory => "code\\terraform";

        public override EnvironmentPreset Preset => new(
            CompressOnArchive: true
        );

        public override bool Created(Werk Werk)
        {
            WriteEditorConfig(Werk, Properties.Resources.default_editorconfig);
            WriteGitAttributes(Werk, Properties.Resources.default_gitattributes);
            return true;
        }
    }
}
