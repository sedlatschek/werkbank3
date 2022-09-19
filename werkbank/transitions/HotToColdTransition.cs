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
    public class HotToColdTransition : Transition
    {
        public override string Title => "Hot to Cold";
        public override WerkState From => WerkState.Hot;
        public override WerkState To => WerkState.Cold;

        public override Batch Build(Werk Werk)
        {
            if (Werk.State != WerkState.Hot)
            {
                throw new UnexpectedWerkStateException(Werk, WerkState.Hot);
            }

            Batch batch = new(Werk, Title);

            // determine paths
            string hotDir = Werk.GetDirectoryFor(WerkState.Hot);
            string hotMetaDir = Path.Combine(hotDir, Config.DirNameWerk);
            string hotMetaFile = Path.Combine(hotMetaDir, Config.FileNameWerkJson);
            string coldDir = Werk.GetDirectoryFor(WerkState.Cold);
            string coldMetaDir = Path.Combine(coldDir, Config.DirNameWerk);
            string coldMetaFile = Path.Combine(coldMetaDir, Config.FileNameWerkJson);
            string gitDir = Path.Combine(hotDir, ".git");
            string gitZip = Path.Combine(hotDir, "git.zip");

            // mark werk as moving
            Werk.Moving = true;
            batch.Write(hotMetaFile, JsonConvert.SerializeObject(Werk));

            // trigger before transition events
            Werk.Environment.BeforeTransition(batch, From, To);

            // zip up git
            if (Directory.Exists(gitDir))
            {
                batch.Zip(gitDir, gitZip);
                batch.Delete(gitDir);
            }

            // clean up cold dir
            batch.Delete(coldDir);
            batch.CreateDirectory(coldDir);

            // move to cold dir
            batch.Copy(hotDir, coldDir);
            batch.Delete(hotDir);

            // hide meta dir
            batch.Hide(coldMetaDir);

            // change state to cold and save
            Werk.Moving = false;
            Werk.State = WerkState.Cold;
            batch.Write(coldMetaFile, JsonConvert.SerializeObject(Werk));

            // trigger after transition events
            Werk.Environment.AfterTransition(batch, From, To);

            return batch;
        }
    }
}
