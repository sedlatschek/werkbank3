using werkbank.models;
using werkbank.transitions;

namespace werkbank.environments
{
    public class PhpEnvironment : Environment
    {
        private const string DIR_NAME_NODE_MODULES = "node_modules";
        private const string DIR_NAME_VENDOR = "vendor";

        public PhpEnvironment(int Index) : base(Index) { }

        public override string Name => "PHP";
        public override string Handle => "php";
        public override string Directory => "code\\web";

        public override EnvironmentPreset Preset => new(
            CompressOnArchive: true
        );

        public override bool Created(Werk Werk)
        {
            WriteEditorConfig(Werk, Properties.Resources.default_editorconfig);
            WriteGitAttributes(Werk, Properties.Resources.default_gitattributes);
            WriteGitIgnore(Werk, Properties.Resources.php_gitignore);
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
                Batch.IgnoreList.Add(Path.Combine(Batch.Werk.CurrentDirectory, DIR_NAME_NODE_MODULES));
                Batch.IgnoreList.Add(Path.Combine(Batch.Werk.CurrentDirectory, DIR_NAME_VENDOR));
            }

            return true;
        }
    }
}
