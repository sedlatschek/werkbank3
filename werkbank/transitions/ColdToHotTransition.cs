using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.exceptions;
using werkbank.models;
using Newtonsoft.Json;

namespace werkbank.transitions
{
    public class ColdToHotTransition : Transition
    {
        public override string Title => "Cold to Hot";
        public override TransitionType Type => TransitionType.ColdToHot;

        public override Batch Build(Werk Werk, environments.Environment? Environment = null)
        {
            if (Werk.State != WerkState.Cold)
            {
                throw new UnexpectedWerkStateException(Werk, WerkState.Cold);
            }

            Batch batch = new(Werk, Type, Title);

            // determine paths
            string hotDir = Werk.GetDirectoryFor(WerkState.Hot);
            string hotMetaDir = Path.Combine(hotDir, Config.DirNameMeta);
            string hotMetaFile = Path.Combine(hotMetaDir, Config.FileNameMetaJson);
            string coldDir = Werk.GetDirectoryFor(WerkState.Cold);
            string coldMetaDir = Path.Combine(coldDir, Config.DirNameMeta);
            string coldMetaFile = Path.Combine(coldMetaDir, Config.FileNameMetaJson);
            string gitDir = Path.Combine(hotDir, Config.DirNameGit);
            string hotGitZip = Path.Combine(hotDir, Config.FileNameGitZip);
            string coldGitZip = Path.Combine(coldDir, Config.FileNameGitZip);

            // mark werk as transitioning
            Werk.TransitionType = Type;
            batch.Write(coldMetaFile, JsonConvert.SerializeObject(Werk));

            // trigger before transition events
            batch.TriggerBeforeTransitionEvent();

            // clean up hot dir
            batch.Delete(hotDir);
            batch.CreateDirectory(hotDir);

            // move to hot dir
            batch.Copy(coldDir, hotDir);
            batch.Delete(coldDir);

            // hide meta dir
            batch.Hide(hotMetaDir);

            // unzip git
            if (File.Exists(coldGitZip))
            {
                batch.CreateDirectory(gitDir);
                batch.Unzip(hotGitZip, gitDir);
                batch.Hide(gitDir);
                batch.Delete(hotGitZip);
            }

            // change state to hot and save
            Werk.TransitionType = null;
            Werk.State = WerkState.Hot;
            Werk.UpdateHistory();
            batch.Write(hotMetaFile, JsonConvert.SerializeObject(Werk));

            // reset werk for current runtime
            Werk.TransitionType = Type;
            Werk.State = WerkState.Cold;

            // trigger after transition events
            batch.TriggerAfterTransitionEvent();

            return batch;
        }

        public override void Finish(Batch Batch)
        {
            if (Batch.Werk == null)
            {
                throw new NullReferenceException("Batch must have a werk that is not null to finish transition");
            }
            Batch.Werk.State = WerkState.Hot;
        }
    }
}
