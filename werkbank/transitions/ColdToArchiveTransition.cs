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

        public override Batch Build(Werk Werk)
        {
            if (Werk.State != WerkState.Cold)
            {
                throw new UnexpectedWerkStateException(Werk, WerkState.Cold);
            }

            Batch batch = new(Werk, Title);

            // determine paths
            string coldDir = Werk.GetDirectoryFor(WerkState.Cold);
            string coldMetaDir = Path.Combine(coldDir, Config.DirNameWerk);
            string coldMetaFile = Path.Combine(coldMetaDir, Config.FileNameWerkJson);
            string archiveDir = Werk.GetDirectoryFor(WerkState.Archived);
            string archiveMetaDir = Path.Combine(archiveDir, Config.DirNameWerk);
            string archiveMetaFile = Path.Combine(archiveMetaDir, Config.FileNameWerkJson);

            // mark werk es moving
            Werk.Moving = true;
            batch.Write(coldMetaFile, JsonConvert.SerializeObject(Werk));

            // trigger before transition events
            Werk.Environment.BeforeTransition(batch, this);

            // clean up archive dir
            batch.Delete(archiveDir);
            batch.CreateDirectory(archiveDir);

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
            }
            else
            {
                // copy directory into archive
                batch.Copy(coldDir, archiveDir);
            }

            // delete cold directory
            batch.Delete(coldDir);

            // hide meta dir
            batch.Hide(archiveMetaDir);

            // change state to archived and save
            Werk.Moving = false;
            Werk.State = WerkState.Archived;
            batch.Write(archiveMetaFile, JsonConvert.SerializeObject(Werk));

            // trigger after transition events
            Werk.Environment.AfterTransition(batch, this);

            return batch;
        }
    }
}
