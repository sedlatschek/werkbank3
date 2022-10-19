using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.exceptions;
using werkbank.models;
using werkbank.repositories;
using werkbank.services;

namespace werkbank.transitions
{
    public class EnvironmentTransition : Transition
    {
        public override string Title => "Environment";
        public override TransitionType Type => TransitionType.Environment;

        protected override Batch OnBuild(Werk Werk, environments.Environment? TargetEnvironment)
        {
            if (TargetEnvironment == null)
            {
                throw new ArgumentNullException(nameof(TargetEnvironment));
            }
            if (Werk.Environment.Handle == TargetEnvironment.Handle)
            {
                throw new InvalidTargetEnvironmentException(TargetEnvironment);
            }

            Batch batch = new(Werk, Type, Werk.Environment.Name + " to " + TargetEnvironment.Name);

            // determine paths
            string curDir = Werk.CurrentDirectory;
            string curMetaDir = Path.Combine(curDir, Config.DirNameMeta);
            string curMetaFile = Path.Combine(curMetaDir, Config.FileNameMetaJson);
            string targetDir = Werk.GetDirectoryFor(TargetEnvironment);
            string targetMetaDir = Path.Combine(targetDir, Config.DirNameMeta);
            string targetMetaFile = Path.Combine(targetMetaDir, Config.FileNameMetaJson);

            // mark werk as transitioning
            Werk.TransitionType = Type;
            batch.Write(curMetaFile, JsonConvert.SerializeObject(Werk));

            // trigger before transition events
            batch.TriggerBeforeTransitionEvent();

            // clean up target dir
            batch.Delete(targetDir);
            batch.CreateDirectory(targetDir);

            // move to target dir
            batch.Copy(curDir, targetDir);
            batch.Delete(curDir);

            // hide previously hidden dirs/files
            List<string> hiddenPaths = FileService.GetHiddenPaths(curDir).Select(path => path.Replace(curDir, targetDir)).ToList();
            foreach (string hiddenPath in hiddenPaths)
            {
                batch.Hide(hiddenPath);
            }

            // change state to archived and save
            environments.Environment currentEnvironment = Werk.Environment;
            Werk.TransitionType = null;
            Werk.Environment = TargetEnvironment;
            batch.Write(targetMetaFile, JsonConvert.SerializeObject(Werk));

            // reset werk for current runtime
            Werk.TransitionType = Type;
            Werk.Environment = currentEnvironment;

            // trigger after transition events
            batch.TriggerAfterTransitionEvent();

            return batch;
        }

        protected override void OnFinish(Batch Batch)
        {
            if (Batch.Werk == null)
            {
                throw new NullReferenceException("Batch must have a werk that is not null to finish transition");
            }

            operations.Operation? createDirOp = Batch.Operations.Find((op) => op.Type == operations.OperationType.CreateDirectory);
            if (createDirOp == null || createDirOp.Destination == null)
            {
                throw new NullReferenceException("Could not retrieve target environment from batch. Operation is null.");
            }

            // remove werk name from the path
            string envDir = createDirOp.Destination[..^Batch.Werk.Name.Length];

            environments.Environment? targetEnvironment = EnvironmentRepository.ByDirectory(envDir);
            if (targetEnvironment == null)
            {
                throw new NullReferenceException("Could not retrieve target environment from batch. TargetEnvironment is null.");
            }

            Batch.Werk.Environment = targetEnvironment;
        }
    }
}
