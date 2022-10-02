using werkbank.models;
using werkbank.transitions;

namespace werkbank.environments
{
    public class JavascriptEnvironment : Environment
    {
        public JavascriptEnvironment(int Index) : base(Index) { }

        public override string Name => "JavaScript";
        public override string Handle => "js";
        public override string Directory => "code\\web";

        public override EnvironmentPreset Preset => new(
            CompressOnArchive: true
        );

        public override bool Created(Werk Werk)
        {
            WriteEditorConfig(Werk, Properties.Resources.default_editorconfig);
            WriteGitAttributes(Werk, Properties.Resources.default_gitattributes);
            WriteGitIgnore(Werk, Properties.Resources.js_gitignore);
            return true;
        }

        public override bool BeforeTransition(Batch Batch, TransitionType TransitionType)
        {
            if (Batch.Werk == null)
            {
                throw new NullReferenceException("Batch.Werk");
            }

            if (TransitionType == TransitionType.HotToCold || TransitionType == TransitionType.Backup)
            {
                Batch.IgnoreList.AddPattern(@".*\\bower_components\\{0,1}.*");
                Batch.IgnoreList.AddPattern(@".*\\node_modules\\{0,1}.*");
            }

            return true;
        }
    }
}
