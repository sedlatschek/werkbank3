using werkbank.models;

namespace werkbank.environments
{
    public class PythonEnvironment : Environment
    {
        public PythonEnvironment(int Index) : base(Index) { }

        public override string Name => "Python";
        public override string Handle => "python";
        public override string Directory => "code\\python";

        public override EnvironmentPreset Preset => new(
            CompressOnArchive: true
        );

        public override bool Created(Werk Werk)
        {
            WriteGitIgnore(Werk, Properties.Resources.python_gitignore);
            return true;
        }
    }
}
