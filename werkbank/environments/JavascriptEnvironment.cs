using werkbank.models;
using werkbank.transitions;

namespace werkbank.environments
{
    public class JavascriptEnvironment : Environment
    {
        private const string DIR_NAME_BOWER_COMPONENTS = "bower_components";
        private const string DIR_NAME_NODE_MODULES = "node_modules";

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
                Batch.IgnoreList.Add(Path.Combine(Batch.Werk.CurrentDirectory, DIR_NAME_BOWER_COMPONENTS));
                Batch.IgnoreList.Add(Path.Combine(Batch.Werk.CurrentDirectory, DIR_NAME_NODE_MODULES));
            }

            return true;
        }
    }
}
