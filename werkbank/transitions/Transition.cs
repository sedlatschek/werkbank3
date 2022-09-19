using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.models;

namespace werkbank.transitions
{
    public abstract class Transition
    {
        public abstract string Title { get; }
        public abstract WerkState From { get; }
        public abstract WerkState To { get; }

        /// <summary>
        /// Build a queue batch for this werk state transition.
        /// </summary>
        /// <param name="Werk"></param>
        /// <returns></returns>
        public abstract Batch Build(Werk Werk);
    }
}
