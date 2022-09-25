using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.transitions;

namespace werkbank.operations
{
    public static class BeforeTransition
    {
        public static bool Perform(Batch Batch)
        {
            if (Batch.Werk == null)
            {
                return false;
            }
            Batch.Werk.Environment.BeforeTransition(Batch, Batch.TransitionType);
            return true;
        }

        public static bool Verify(Batch Batch)
        {
            if (Batch.Werk == null)
            {
                return false;
            }
            return Batch.Werk.Environment.VerifyBeforeTransition(Batch, Batch.TransitionType);
        }
    }
}
