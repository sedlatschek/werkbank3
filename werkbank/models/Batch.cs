using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using werkbank.operations;
using werkbank.services;
using werkbank.transitions;

namespace werkbank.models
{
    public class Batch
    {
        public event EventHandler<Operation>? OperationStarted;
        public event EventHandler<Operation>? OperationDone;

        public delegate void BatchDone(Batch Batch, TransitionType? TransitionType, Werk werk);

        public BatchDone? TestBatchDone;

        [JsonProperty("id")]
        private readonly Guid guid;
        [JsonIgnore]
        public Guid Id => guid;

        [JsonProperty("ops")]
        public List<Operation> Operations;

        [JsonProperty("attempts")]
        private int attempts;
        [JsonIgnore]
        public int Attempts => attempts;
        [JsonProperty("title")]
        private readonly string title;
        [JsonIgnore]
        public string Title => title;
        [JsonProperty("werkId")]
        private Guid werkId;
        [JsonIgnore]
        public Guid WerkId => werkId;

        [JsonIgnore]
        private Werk? werk;
        [JsonIgnore]
        public Werk? Werk
        {
            get => werk;
            set
            {
                werk = value;
                if (value != null)
                {
                    werkId = value.Id;
                    value.CurrentBatch = this;
                }
            }
        }

        [JsonIgnore]
        private Exception? error;
        [JsonIgnore]
        public Exception? Error => error;

        [JsonProperty("transitionType")]
        protected TransitionType transitionType;
        [JsonIgnore]
        public TransitionType TransitionType => transitionType;

        [JsonProperty("ignore")]
        protected MatchingList ignoreList;
        [JsonIgnore]
        public MatchingList IgnoreList => ignoreList;

        [JsonIgnore]
        public bool Done => Operations.All((op) => op.Success);

        [JsonIgnore]
        public bool IsInTimeout => Operations.Any((op) => op.IsInTimeout);

        [JsonIgnore]
        public int ProgressPercentage
        {
            get
            {
                int doneOperations = Operations.Where(x => x.Success).Count();
                if (doneOperations == Operations.Count)
                {
                    return 100;
                }
                return 100 / Operations.Count * doneOperations;
            }
        }

        public Batch(Werk Werk, TransitionType TransitionType, string Title)
        {
            guid = Guid.NewGuid();
            attempts = 0;
            this.Werk = Werk;
            werkId = Werk.Id;
            transitionType = TransitionType;
            Operations = new List<Operation>();
            ignoreList = new MatchingList();
            title = Title;
        }

        [JsonConstructor]
        public Batch(string Title)
        {
            Operations = new List<Operation>();
            ignoreList = new MatchingList();
            title = Title;
        }

        /// <summary>
        /// Run all operations within the package
        /// </summary>
        public void Run()
        {
            attempts += 1;

            if (Werk == null)
            {
                error = new ArgumentNullException("Werk");
                return;
            }

            foreach (Operation op in Operations)
            {
                // skip if already done
                if (op.Success) continue;

                // stop if timeout from previous attempt
                if (op.Attempt > 0 && (DateTime.Now - op.LastAttempt)?.TotalMilliseconds < Settings.Properties.OperationRetryTimeout)
                {
                    break;
                }

                OperationStarted?.Invoke(this, op);

                // wait a little before running the next operation
                Thread.Sleep(100);

                op.Run();

                OperationDone?.Invoke(this, op);

                // stop if operation failed
                if (!op.Success)
                {
                    error = op.Error;
                    break;
                }
            }

            if (Done)
            {
                Transition.For(TransitionType).Finish(this);
            }
        }

        /// <summary>
        /// Untie the batch from its werk.
        /// </summary>
        public void Untie()
        {
            if (Werk != null)
            {
                Werk.CurrentBatch = null;
                Werk.TransitionType = null;
                Werk = null;
            }
        }

        /// <summary>
        /// Create operation to trigger the environments after transition event and add it to the batch.
        /// </summary>
        /// <returns></returns>
        public Operation TriggerAfterTransitionEvent()
        {
            Operation op = new(OperationType.AfterTransitionEvent, this, null, null);
            Operations.Add(op);
            return op;
        }

        /// <summary>
        /// Create operation to trigger the environments before transition event and add it to the batch.
        /// </summary>
        /// <param name="IgnoreList"></param>
        /// <returns></returns>
        public Operation TriggerBeforeTransitionEvent()
        {
            Operation op = new(OperationType.BeforeTransitionEvent, this, null, null);
            Operations.Add(op);
            return op;
        }

        /// <summary>
        /// Create operation to copy a file or a directory and add it to the batch.
        /// </summary>
        /// <param name="SourcePath"></param>
        /// <param name="DestinationPath"></param>
        /// <returns></returns>
        public Operation Copy(string SourcePath, string DestinationPath)
        {
            Operation op = new(OperationType.Copy, this, SourcePath, DestinationPath);
            Operations.Add(op);
            return op;
        }

        /// <summary>
        /// Create operation to create a directory and add it to the batch.
        /// </summary>
        /// <param name="DestinationPath"></param>
        /// <returns></returns>
        public Operation CreateDirectory(string DestinationPath)
        {
            Operation op = new(OperationType.CreateDirectory, this, null, DestinationPath);
            Operations.Add(op);
            return op;
        }

        /// <summary>
        /// Create operation to delete a file or a directory and add it to the batch.
        /// </summary>
        /// <param name="TargetPath"></param>
        /// <returns></returns>
        public Operation Delete(string TargetPath)
        {
            Operation op = new(OperationType.Delete, this, null, TargetPath);
            Operations.Add(op);
            return op;
        }

        /// <summary>
        /// Create operation to hide a file or a directory and add it to the batch.
        /// </summary>
        /// <param name="TargetPath"></param>
        /// <returns></returns>
        public Operation Hide(string TargetPath)
        {
            Operation op = new(OperationType.Hide, this, null, TargetPath);
            Operations.Add(op);
            return op;
        }

        /// <summary>
        /// Create operation to move a file and add it to the batch.
        /// </summary>
        /// <param name="SourcePath"></param>
        /// <param name="DestinationPath"></param>
        /// <returns></returns>
        public Operation MoveFile(string SourcePath, string DestinationPath)
        {
            Operation op = new(OperationType.MoveFile, this, SourcePath, DestinationPath);
            Operations.Add(op);
            return op;
        }

        /// <summary>
        /// Create operation to unhide a file or a directory and add it to the batch.
        /// </summary>
        /// <param name="TargetPath"></param>
        /// <returns></returns>
        public Operation Unhide(string TargetPath)
        {
            Operation op = new(OperationType.Unhide, this, null, TargetPath);
            Operations.Add(op);
            return op;
        }

        /// <summary>
        /// Create operation to unzíp a zip file to a given destination path and add it to the batch.
        /// </summary>
        /// <param name="SourceZip"></param>
        /// <param name="DestinationPath"></param>
        /// <returns></returns>
        public Operation Unzip(string SourceZip, string DestinationPath)
        {
            Operation op = new(OperationType.Unzip, this, SourceZip, DestinationPath);
            Operations.Add(op);
            return op;
        }

        /// <summary>
        /// Create operation to write content to a file and add it to the batch.
        /// </summary>
        /// <param name="DestinationPath"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        public Operation Write(string DestinationPath, string Content)
        {
            Operation op = new(OperationType.Write, this, Content, DestinationPath);
            Operations.Add(op);
            return op;
        }

        /// <summary>
        /// Create operation to zip up a directory into a zip archive and add it to the batch.
        /// </summary>
        /// <param name="SourcePath"></param>
        /// <param name="DestinationPath"></param>
        /// <returns></returns>
        public Operation Zip(string SourcePath, string DestinationPath)
        {
            Operation op = new(OperationType.Zip, this, SourcePath, DestinationPath);
            Operations.Add(op);
            return op;
        }
    }
}
