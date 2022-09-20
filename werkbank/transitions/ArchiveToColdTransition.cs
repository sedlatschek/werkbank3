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

        public override Batch Build(Werk Werk)
        {
            if (Werk.State != WerkState.Archived)
            {
                throw new UnexpectedWerkStateException(Werk, WerkState.Archived);
            }

            Batch batch = new(Werk, Title);

            // determine paths
            string coldDir = Werk.GetDirectoryFor(WerkState.Cold);
            string coldMetaDir = Path.Combine(coldDir, Config.DirNameWerk);
            string coldMetaFile = Path.Combine(coldMetaDir, Config.FileNameWerkJson);
            string archiveDir = Werk.GetDirectoryFor(WerkState.Archived);
            string archiveMetaDir = Path.Combine(archiveDir, Config.DirNameWerk);
            string archiveMetaFile = Path.Combine(archiveMetaDir, Config.FileNameWerkJson);

            // mark werk as moving
            Werk.Moving = true;
            batch.Write(archiveMetaFile, JsonConvert.SerializeObject(Werk));

            // trigger before transition events
            Werk.Environment.BeforeTransition(batch, this);

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
            Werk.Moving = false;
            Werk.State = WerkState.Cold;
            batch.Write(coldMetaFile, JsonConvert.SerializeObject(Werk));

            // trigger after transition events
            Werk.Environment.AfterTransition(batch, this);

            return batch;
        }
    }
}
