using werkbank.models;
using werkbank.transitions;

namespace werkbank.environments
{
    public abstract class Environment
    {
        public abstract string Name { get; }
        public abstract string Handle { get; }
        public abstract string Directory { get; }
        public abstract EnvironmentPreset Preset { get; }

        public int Index { get; }

        public Environment(int Index)
        {
            this.Index = Index;
        }

        public bool Bootstrap(Werk Werk)
        {
            return Created(Werk);
        }

        /// <summary>
        /// Is triggered whenever a new werk is created.
        /// </summary>
        /// <param name="Werk"></param>
        /// <returns></returns>
        public virtual bool Created(Werk Werk) => true;

        /// <summary>
        /// Is triggered whenever a werk is updated.
        /// </summary>
        /// <param name="Werk"></param>
        /// <returns></returns>
        public virtual bool Updated(Werk Werk) => true;

        /// <summary>
        /// Is triggered whenever a transition of a werk begins.
        /// </summary>
        /// <param name="Batch"></param>
        /// <param name="TransitionType"></param>
        /// <returns></returns>
        public virtual bool BeforeTransition(Batch Batch, TransitionType TransitionType) => false;

        /// <summary>
        /// Can be used to verify the before transition events worked.
        /// </summary>
        /// <param name="Batch"></param>
        /// <param name="TransitionType"></param>
        /// <returns></returns>
        public virtual bool VerifyBeforeTransition(Batch Batch, TransitionType TransitionType) => true;

        /// <summary>
        /// Is triggered whenever a transition of a werk is done.
        /// </summary>
        /// <param name="Batch"></param>
        /// <param name="TransitionType"></param>
        /// <returns></returns>
        public virtual bool AfterTransition(Batch Batch, TransitionType TransitionType) => false;

        /// <summary>
        /// Can be used to verify the after transition events worked.
        /// </summary>
        /// <param name="Batch"></param>
        /// <param name="TransitionType"></param>
        /// <returns></returns>
        public virtual bool VerifyAfterTransition(Batch Batch, TransitionType TransitionType) => true;

        /// <summary>
        /// Write the werks .gitattributes file.
        /// </summary>
        /// <param name="Werk"></param>
        /// <param name="Content"></param>
        protected static void WriteGitAttributes(Werk Werk, string Content)
        {
            File.WriteAllText(Path.Combine(Werk.CurrentDirectory, Config.FileNameGitAttributes), Content);
        }

        /// <summary>
        /// Write the werks .gitignore file.
        /// </summary>
        /// <param name="Werk"></param>
        /// <param name="Content"></param>
        protected static void WriteGitIgnore(Werk Werk, string Content)
        {
            File.WriteAllText(Path.Combine(Werk.CurrentDirectory, Config.FileNameGitIgnore), Content);
        }

        /// <summary>
        /// Write the werks .editorconfig file
        /// </summary>
        /// <param name="Werk"></param>
        /// <param name="Content"></param>
        protected static void WriteEditorConfig(Werk Werk, string Content)
        {
            File.WriteAllText(Path.Combine(Werk.CurrentDirectory, Config.FileNameEditorConfig), Content);
        }
    }
}
