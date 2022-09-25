using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using werkbank.exceptions;
using werkbank.models;
using werkbank.services;

namespace werkbank.transitions
{
    public class ArchiveToColdTransition : Transition
    {
        public override string Title => "Archive to Cold";
        public override TransitionType Type => TransitionType.ArchiveToCold;

        protected override Batch OnBuild(Werk Werk, environments.Environment? Environment = null)
        {
            if (Werk.State != WerkState.Archived)
            {
                throw new UnexpectedWerkStateException(Werk, WerkState.Archived);
            }

            Batch batch = new(Werk, Type, Title);

            // determine paths
            string coldDir = Werk.GetDirectoryFor(WerkState.Cold);
            string coldMetaDir = Path.Combine(coldDir, Config.DirNameMeta);
            string coldMetaFile = Path.Combine(coldMetaDir, Config.FileNameMetaJson);
            string archiveDir = Werk.GetDirectoryFor(WerkState.Archived);
            string archiveMetaDir = Path.Combine(archiveDir, Config.DirNameMeta);
            string archiveMetaFile = Path.Combine(archiveMetaDir, Config.FileNameMetaJson);

            // mark werk as transitioning
            Werk.TransitionType = Type;
            batch.Write(archiveMetaFile, JsonConvert.SerializeObject(Werk));

            // trigger before transition events
            batch.TriggerBeforeTransitionEvent();

            // clean up cold dir
            batch.Delete(coldDir);
            batch.CreateDirectory(coldDir);

            if (Werk.CompressOnArchive)
            {
                // copy .werk folder into cold vault
                batch.Copy(archiveMetaDir, coldMetaDir);

                // unzip werk files into cold vault
                string zipFile = Path.Combine(
                    archiveDir,
                    FileService.ReplaceInvalidCharsFromPath(Werk.Name) + ".zip"
                );
                batch.Unzip(zipFile, coldDir);
            }
            else
            {
                // move directory into cold vault
                batch.Copy(archiveDir, coldDir);
            }

            // delete archive directory
            batch.Delete(archiveDir);

            // hide meta dir
            batch.Hide(coldMetaDir);

            // change state to cold and save
            Werk.TransitionType = null;
            Werk.State = WerkState.Cold;
            Werk.UpdateHistory();
            batch.Write(coldMetaFile, JsonConvert.SerializeObject(Werk));

            // reset werk for current runtime
            Werk.TransitionType = Type;
            Werk.State = WerkState.Archived;

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
            Batch.Werk.State = WerkState.Cold;
        }
    }
}
