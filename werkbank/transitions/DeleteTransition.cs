using werkbank.models;

namespace werkbank.transitions
{
    public class DeleteTransition : Transition
    {
        public override string Title => "Delete";

        public override TransitionType Type => TransitionType.Delete;

        protected override Batch OnBuild(Werk Werk, environments.Environment? Environment = null)
        {
            Batch batch = new(Werk, Type, Title);

            // trigger before transition events
            batch.TriggerBeforeTransitionEvent();

            batch.Delete(Werk.CurrentDirectory);

            // we do not trigger after transition events, as it could result in an edge case where
            // the user closes the application when the delete operation is done, but the after
            // transition operation is pending. Upon restart, the werk object can not be retrieved,
            // as its folder is already deleted. The queue will be stuck on the after transition
            // event operation, as it can never succeed without a werk object.

            return batch;
        }

        protected override void OnFinish(Batch Batch)
        {
            return;
        }
    }
}
