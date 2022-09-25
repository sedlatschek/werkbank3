using werkbank.models;

namespace werkbank.environments
{
    public class MarkdownEnvironment : Environment
    {
        public MarkdownEnvironment(int Index) : base(Index) { }

        public override string Name => "Markdown";
        public override string Handle => "md";
        public override string Directory => "code\\markdown";

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
