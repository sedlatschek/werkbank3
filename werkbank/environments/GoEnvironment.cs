using werkbank.models;

namespace werkbank.environments
{
    public class GoEnvironment : Environment
    {
        public GoEnvironment(int Index) : base(Index) { }

        public override string Name => "Go";
        public override string Handle => "go";
        public override string Directory => "code\\go";

        public override EnvironmentPreset Preset => new(
            CompressOnArchive: true
        );

        public override bool Created(Werk Werk)
        {
            WriteGitIgnore(Werk, Properties.Resources.go_gitignore);
            WriteEditorConfig(Werk, Properties.Resources.go_editorconfig);
            return true;
        }
    }
}
