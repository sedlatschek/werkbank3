using werkbank.models;

namespace werkbank.environments
{
    public class CSharpEnvironment : Environment
    {
        public CSharpEnvironment(int Index) : base(Index) { }

        public override string Name => "C#";
        public override string Handle => "csharp";
        public override string Directory => "code\\dotnet";

        public override EnvironmentPreset Preset => new(
            CompressOnArchive: true
        );

        public override bool Created(Werk Werk)
        {
            WriteGitAttributes(Werk, Properties.Resources.csharp_gitattributes);
            WriteGitIgnore(Werk, Properties.Resources.csharp_gitignore);
            return true;
        }
    }
}
