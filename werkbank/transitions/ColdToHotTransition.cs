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
        public override WerkState From => WerkState.Cold;
        public override WerkState To => WerkState.Hot;

        public override Batch Build(Werk Werk)
        {
            if (Werk.State != WerkState.Cold)
            {
                throw new UnexpectedWerkStateException(Werk, WerkState.Cold);
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
            string hotGitZip = Path.Combine(hotDir, "git.zip");
            string coldGitZip = Path.Combine(coldDir, "git.zip");

            // mark werk as moving
            Werk.Moving = true;
            batch.Write(coldMetaFile, JsonConvert.SerializeObject(Werk));

            // trigger before transition events
            Werk.Environment.BeforeTransition(batch, From, To);

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
            Werk.Moving = false;
            Werk.State = WerkState.Hot;
            batch.Write(hotMetaFile, JsonConvert.SerializeObject(Werk));

            // trigger after transition events
            Werk.Environment.AfterTransition(batch, From, To);

            return batch;
        }
    }
}
