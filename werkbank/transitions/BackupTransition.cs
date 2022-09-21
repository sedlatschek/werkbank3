using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.exceptions;
using werkbank.models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace werkbank.transitions
{
    public class BackupTransition : Transition
    {
        public override string Title => "Backup";
        public override TransitionType Type => TransitionType.Backup;

        public override Batch Build(Werk Werk)
        {
            if (Werk.State != WerkState.Hot)
            {
                throw new UnexpectedWerkStateException(Werk, WerkState.Hot);
            }

            Batch batch = new(Werk, Title);

            List<string> ignoreList = new();

            // determine paths
            string hotDir = Werk.GetDirectoryFor(WerkState.Hot);
            string hotMetaDir = Path.Combine(hotDir, Config.DirNameMeta);
            string coldDir = Werk.GetDirectoryFor(WerkState.Cold);
            string gitDir = Path.Combine(hotDir, Config.DirNameGit);
            string gitZip = Path.Combine(coldDir, Config.FileNameGitZip);

            // trigger before transition events
            Werk.Environment.BeforeTransition(batch, this);

            // add .werk directory to blacklist
            ignoreList.Add(hotMetaDir);

            // clean up cold dir
            batch.Delete(coldDir);
            batch.CreateDirectory(coldDir);

            // zip up git dir
            if (Directory.Exists(gitDir))
            {
                ignoreList.Add(gitDir);
                batch.Zip(gitDir, gitZip);
            }

            // copy directories that are not blacklisted
            batch.Copy(hotDir, coldDir, ignoreList);
  
            // trigger after transition events
            Werk.Environment.AfterTransition(batch, this);

            return batch;
        }
    }
}
