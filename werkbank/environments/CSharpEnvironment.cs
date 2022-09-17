using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace werkbank.environments
{
    public class CSharpEnvironment : Environment
    {
        public CSharpEnvironment(int Index) : base(Index) { }

        public override string Name => "C#";
        public override string Handle => "csharp";
        public override string Directory => "code\\csharp";
    }
}
