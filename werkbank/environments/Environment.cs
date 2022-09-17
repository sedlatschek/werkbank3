using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace werkbank.environments
{
    public abstract class Environment
    {
        public abstract string Name { get; }
        public abstract string Handle { get; }
        public abstract string Directory { get; }
        public int Index { get; }

        public Environment(int Index)
        {
            this.Index = Index;
        }
    }
}
