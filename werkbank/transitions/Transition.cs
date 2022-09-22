using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.exceptions;
using werkbank.models;

namespace werkbank.transitions
{
    public enum TransitionType
    {
        HotToCold,
        Backup,
        ColdToHot,
        ColdToArchive,
        ArchiveToCold,
        Environment
    }

    public abstract class Transition
    {
        public abstract string Title { get; }

        public abstract TransitionType Type { get; }

        /// <summary>
        /// Build a queue batch for this werk state transition.
        /// </summary>
        /// <param name="Werk"></param>
        /// <param name="Environment"></param>
        /// <returns></returns>
        public abstract Batch Build(Werk Werk, environments.Environment? Environment = null);

        public static Transition For(TransitionType Type)
        {
            return Type switch
            {
                TransitionType.Environment => new EnvironmentTransition(),
                TransitionType.HotToCold => new HotToColdTransition(),
                TransitionType.Backup => new BackupTransition(),
                TransitionType.ColdToHot => new ColdToHotTransition(),
                TransitionType.ColdToArchive => new ColdToArchiveTransition(),
                TransitionType.ArchiveToCold => new ArchiveToColdTransition(),
                _ => throw new UnhandledTransitionTypeException(Type),
            };
        }
    }
}
