using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.models;
using werkbank.transitions;

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

        public bool BeforeTransition(Batch Batch, TransitionType TransitionType)
        {
            return false;
        }

        public bool VerifyBeforeTransition(Batch Batch, TransitionType TransitionType)
        {
            return true;
        }

        public bool AfterTransition(Batch Batch, TransitionType TransitionType)
        {
            return false;
        }

        public bool VerifyAfterTransition(Batch Batch, TransitionType TransitionType)
        {
            return true;
        }
    }
}
