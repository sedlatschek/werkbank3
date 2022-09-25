using werkbank.models;

namespace werkbank.environments
{
    public class Delphi10Environment : Environment
    {
        public Delphi10Environment(int Index) : base(Index) { }

        public override string Name => "Delphi 10";
        public override string Handle => "delphi10";
        public override string Directory => "code\\delphi\\10";

        public override EnvironmentPreset Preset => new(
            CompressOnArchive: true
        );

        public override bool Created(Werk Werk)
        {
            WriteGitIgnore(Werk, Properties.Resources.delphi_gitignore);
            return true;
        }
    }
}
