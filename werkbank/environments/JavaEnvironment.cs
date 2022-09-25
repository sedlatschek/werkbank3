using werkbank.models;

namespace werkbank.environments
{
    public class JavaEnvironment : Environment
    {
        public JavaEnvironment(int Index) : base(Index) { }

        public override string Name => "Java";
        public override string Handle => "java";
        public override string Directory => "code\\java";

        public override EnvironmentPreset Preset => new(
            CompressOnArchive: true
        );

        public override bool Created(Werk Werk)
        {
            WriteGitAttributes(Werk, Properties.Resources.java_gitattributes);
            WriteGitIgnore(Werk, Properties.Resources.java_gitignore);
            return true;
        }
    }
}
