using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.exceptions;
using werkbank.models;

namespace werkbank.transitions
{
    public class EnvironmentTransition : Transition
    {
        public override string Title => "Environment";
        public override TransitionType Type => TransitionType.Environment;

        public override Batch Build(Werk Werk, environments.Environment? TargetEnvironment)
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

            // mark werk as moving
            Werk.Moving = true;
            batch.Write(curMetaFile, JsonConvert.SerializeObject(Werk));

            // trigger before transition events
            batch.TriggerBeforeTransitionEvent();

            // clean up target dir
            batch.Delete(targetDir);
            batch.CreateDirectory(targetDir);

            // move to target dir
            batch.Copy(curDir, targetDir);
            batch.Delete(curDir);

            // change state to archived and save
            Werk.Moving = false;
            Werk.Environment = TargetEnvironment;
            batch.Write(targetMetaFile, JsonConvert.SerializeObject(Werk));

            // trigger after transition events
            batch.TriggerAfterTransitionEvent();

            return batch;
        }
    }
}
