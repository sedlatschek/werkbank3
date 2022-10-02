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
        Environment,
        Delete
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
        public Batch Build(Werk Werk, environments.Environment? Environment = null)
        {
            if (Werk.CurrentBatch != null)
            {
                throw new WerkIsAlreadyTransitioningException(Werk.CurrentBatch);
            }
            if (Werk.TransitionType != null)
            {
                throw new WerkIsAlreadyTransitioningException(Werk);
            }

            return OnBuild(Werk, Environment);
        }

        protected abstract Batch OnBuild(Werk Werk, environments.Environment? Environment = null);

        /// <summary>
        /// Finish the transition. Is called after the batch is done.
        /// </summary>
        /// <param name="Batch"></param>
        /// <returns></returns>
        public void Finish(Batch Batch)
        {
            OnFinish(Batch);
        }

        protected abstract void OnFinish(Batch Batch);

        public static Transition For(TransitionType Type)
        {
            return Type switch
            {
                TransitionType.Environment => new EnvironmentTransition(),
                TransitionType.Delete => new DeleteTransition(),
                TransitionType.HotToCold => new HotToColdTransition(),
                TransitionType.Backup => new BackupTransition(),
                TransitionType.ColdToHot => new ColdToHotTransition(),
                TransitionType.ColdToArchive => new ColdToArchiveTransition(),
                TransitionType.ArchiveToCold => new ArchiveToColdTransition(),
                _ => throw new UnhandledTransitionTypeException(Type),
            };
        }

        /// <summary>
        /// Get the state a transition transitions from.
        /// </summary>
        /// <param name="TransitionType"></param>
        /// <returns></returns>
        /// <exception cref="UnhandledTransitionTypeException"></exception>
        public static WerkState From(TransitionType TransitionType)
        {
            return TransitionType switch
            {
                TransitionType.HotToCold => WerkState.Hot,
                TransitionType.ColdToHot => WerkState.Cold,
                TransitionType.ColdToArchive => WerkState.Cold,
                TransitionType.ArchiveToCold => WerkState.Archived,
                _ => throw new UnhandledTransitionTypeException(TransitionType),
            };
        }

        /// <summary>
        /// Get the state a transition transitions to.
        /// </summary>
        /// <param name="TransitionType"></param>
        /// <returns></returns>
        /// <exception cref="UnhandledTransitionTypeException"></exception>
        public static WerkState To(TransitionType TransitionType)
        {
            return TransitionType switch
            {
                TransitionType.HotToCold => WerkState.Cold,
                TransitionType.ColdToHot => WerkState.Hot,
                TransitionType.ColdToArchive => WerkState.Archived,
                TransitionType.ArchiveToCold => WerkState.Cold,
                _ => throw new UnhandledTransitionTypeException(TransitionType),
            };
        }
    }
}
