using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.models;

namespace werkbank.transitions
{
    public enum TransitionType
    {
        HotToCold,
        Backup,
        ColdToHot,
        ColdToArchive,
        ArchiveToCold
    }

    public abstract class Transition
    {
        public abstract string Title { get; }

        public abstract TransitionType Type { get; }

        /// <summary>
        /// Build a queue batch for this werk state transition.
        /// </summary>
        /// <param name="Werk"></param>
        /// <returns></returns>
        public abstract Batch Build(Werk Werk);
    }
}
