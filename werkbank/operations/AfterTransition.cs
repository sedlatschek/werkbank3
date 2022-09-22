using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.transitions;

namespace werkbank.operations
{
    public static class AfterTransition
    {
        public static bool Perform(Batch Batch)
        {
            Batch.Werk.Environment.AfterTransition(Batch, Batch.TransitionType);
            return true;
        }

        public static bool Verify(Batch Batch)
        {
            return Batch.Werk.Environment.VerifyAfterTransition(Batch, Batch.TransitionType);
        }
    }
}
