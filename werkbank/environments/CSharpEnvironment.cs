using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.models;

namespace werkbank.environments
{
    public class CSharpEnvironment : Environment
    {
        public CSharpEnvironment(int Index) : base(Index) { }

        public override string Name => "C#";
        public override string Handle => "csharp";
        public override string Directory => "code\\csharp";

        public override EnvironmentPreset Preset => new(
            CompressOnArchive: true
        );

        public override bool Created(Werk Werk)
        {
            File.WriteAllText(Path.Combine(Werk.CurrentDirectory, ".gitignore"), Properties.Resources.csharp_gitignore);
            File.WriteAllText(Path.Combine(Werk.CurrentDirectory, ".gitattributes"), Properties.Resources.csharp_gitattributes);
            return true;
        }
    }
}
