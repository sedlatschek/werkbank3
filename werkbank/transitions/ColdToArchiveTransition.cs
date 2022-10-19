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
    public class ColdToArchiveTransition : Transition
    {
        public override string Title => "Cold to Archive";
        public override TransitionType Type => TransitionType.ColdToArchive;

        protected override Batch OnBuild(Werk Werk, environments.Environment? Environment = null)
        {
            if (Werk.State != WerkState.Cold)
            {
                throw new UnexpectedWerkStateException(Werk, WerkState.Cold);
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
            batch.Write(coldMetaFile, JsonConvert.SerializeObject(Werk));

            // trigger before transition events
            batch.TriggerBeforeTransitionEvent();

            // clean up archive dir
            batch.Delete(archiveDir);
            batch.CreateDirectory(archiveDir);

            // get the currently hidden paths
            List<string> hiddenPaths = FileService.GetHiddenPaths(coldDir);

            if (Werk.CompressOnArchive)
            {
                // copy .werk folder into archive
                batch.Copy(coldMetaDir, archiveMetaDir);

                // zip up the rest of the files
                string zipFile = Path.Combine(
                    archiveDir,
                    FileService.ReplaceInvalidCharsFromPath(Werk.Name) + ".zip"
                );
                batch.Zip(coldDir, zipFile);

                // write .werk/hidden.json
                batch.Write(
                    Path.Combine(archiveMetaDir, Config.FileNameHiddenJson),
                    JsonConvert.SerializeObject(hiddenPaths.Select(path => path.Replace(coldDir + "\\", "")).ToList())
                );

                // hide meta dir
                batch.Hide(archiveMetaDir);
            }
            else
            {
                // copy directory into archive
                batch.Copy(coldDir, archiveDir);

                // hide previously hidden dirs/files
                foreach (string hiddenPath in hiddenPaths.Select(path => path.Replace(coldDir, archiveDir)).ToList())
                {
                    batch.Hide(hiddenPath);
                }
            }

            // delete cold directory
            batch.Delete(coldDir);

            // change state to archived and save
            Werk.TransitionType = null;
            Werk.State = WerkState.Archived;
            Werk.UpdateHistory();
            batch.Write(archiveMetaFile, JsonConvert.SerializeObject(Werk));

            // reset werk for current runtime
            Werk.TransitionType = Type;
            Werk.State = WerkState.Cold;

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
            Batch.Werk.State = WerkState.Archived;
        }
    }
}
