using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using werkbank.converters;
using werkbank.exceptions;
using werkbank.services;
using werkbank.transitions;

namespace werkbank.operations
{
    public enum OperationType
    {
        BeforeTransitionEvent,
        AfterTransitionEvent,
        Copy,
        CreateDirectory,
        Delete,
        Hide,
        MoveFile,
        Unhide,
        Write,
        Zip,
        Unzip,
    }

    public class Operation
    {
        [JsonProperty("id")]
        protected readonly Guid guid;
        [JsonProperty("created")]
        protected readonly DateTime created;
        [JsonProperty("attempt")]
        protected int attempt = 0;
        [JsonProperty("last")]
        protected DateTime? lastAttempt;
        [JsonProperty("success")]
        protected bool success = false;
        [JsonProperty("source")]
        protected string? source;
        [JsonProperty("dest")]
        protected string? dest;
        [JsonProperty("ignore")]
        protected List<string>? ignoreList;
        [JsonIgnore]
        protected Exception? error;
        [JsonIgnore]
        protected bool running = false;

        [JsonProperty("type")]
        protected OperationType type;
        [JsonIgnore]
        public Batch Batch;

        [JsonIgnore]
        public Guid Id => guid;
        [JsonIgnore]
        public DateTime Created => created;
        [JsonIgnore]
        public int Attempt => attempt;
        [JsonIgnore]
        public DateTime? LastAttempt => lastAttempt;
        [JsonIgnore]
        public bool Success => success;
        [JsonIgnore]
        public Exception? Error => error;
        [JsonIgnore]
        public bool Running => running;
        [JsonIgnore]
        public string? Source => source;
        [JsonIgnore]
        public string? Destination => dest;
        [JsonIgnore]
        public List<string>? IgnoreList => ignoreList;

        [JsonIgnore]
        public OperationType Type => type;

        [JsonIgnore]
        public bool IsInTimeout
        {
            get
            {
                return attempt > 0
                    && success == false
                    && (DateTime.Now - lastAttempt)?.TotalMilliseconds < Settings.Properties.OperationRetryTimeout;
            }
        }

        public Operation(OperationType Type, Batch Batch, string? Source, string? Destination, List<string>? IgnoreList)
        {
            guid = Guid.NewGuid();
            type = Type;
            created = DateTime.Now;
            source = Source;
            dest = Destination;
            ignoreList = IgnoreList;
            this.Batch = Batch;
        }

        /// <summary>
        /// Perform and verify the operation.
        /// </summary>
        public void Run()
        {
            try
            {
                running = true;
                attempt++;
                lastAttempt = DateTime.Now;
                success = Perform() && Verify();
            }
            catch (Exception ex)
            {
                error = ex;
                success = false;
            }
            finally
            {
                running = false;
            }
        }

        protected bool Perform()
        {
            return type switch
            {
                OperationType.AfterTransitionEvent => AfterTransition.Perform(Batch),
                OperationType.BeforeTransitionEvent => BeforeTransition.Perform(Batch),
                OperationType.Copy => Copy.Perform(Source, Destination, IgnoreList),
                OperationType.CreateDirectory => CreateDirectory.Perform(Destination),
                OperationType.Delete => Delete.Perform(Destination),
                OperationType.Hide => Hide.Perform(Destination),
                OperationType.MoveFile => MoveFile.Perform(Source, Destination),
                OperationType.Unhide => Unhide.Perform(Destination),
                OperationType.Unzip => Unzip.Perform(Source, Destination),
                OperationType.Write => Write.Perform(Source, Destination),
                OperationType.Zip => Zip.Perform(Source, Destination),
                _ => throw new UnhandledOperationTypeException(type),
            };
        }

        protected bool Verify()
        {
            return type switch
            {
                OperationType.AfterTransitionEvent => AfterTransition.Verify(Batch),
                OperationType.BeforeTransitionEvent => BeforeTransition.Verify(Batch),
                OperationType.Copy => Copy.Verify(Source, Destination, IgnoreList),
                OperationType.CreateDirectory => CreateDirectory.Verify(Destination),
                OperationType.Delete => Delete.Verify(Destination),
                OperationType.Hide => Hide.Verify(Destination),
                OperationType.MoveFile => MoveFile.Verify(Source, Destination),
                OperationType.Unhide => Unhide.Verify(Destination),
                OperationType.Unzip => Unzip.Verify(Source, Destination),
                OperationType.Write => Write.Verify(Source, Destination),
                OperationType.Zip => Zip.Verify(Source, Destination),
                _ => throw new UnhandledOperationTypeException(type),
            };
        }
    }
}
