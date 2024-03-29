﻿using werkbank.models;

namespace werkbank.environments
{
    public class Delphi7Environment : Environment
    {
        public Delphi7Environment(int Index) : base(Index) { }

        public override string Name => "Delphi 7";
        public override string Handle => "delphi7";
        public override string Directory => "code\\delphi\\7";

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
