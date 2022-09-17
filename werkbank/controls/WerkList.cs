using BrightIdeasSoftware;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using werkbank.exceptions;
using werkbank.models;
using werkbank.repositories;

namespace werkbank.controls
{
    public partial class WerkList : UserControl
    {
        public readonly WerkState State;

        private readonly ImageList IconList;

        private DirectoryInfo Directory
        {
            get
            {
                return new DirectoryInfo(VaultRepository.GetDirectory(State));
            }
        }

        public bool Busy
        {
            get { return worker.IsBusy; }
        }

        public ObjectListView List
        {
            get { return objectListView; }
        }

        public WerkList(ImageList IconList, WerkState State = WerkState.Hot)
        {
            this.State = State;
            this.IconList = IconList;
            InitializeComponent();
            SetupColumns();
        }

        private void SetupColumns()
        {
            OLVColumn colIcon = new()
            {
                Name = objectListView.Name + "_column_icon",
                AspectName = "ImageIndex",
                Text = "Icon",
                IsEditable = false,
                Searchable = false,
                Sortable = false,
                Width = 26,
                ImageGetter = (object x) =>
                {
                    Werk werk = (Werk)x;
                    if (werk.HasIcon)
                    {
                        return werk.Id.ToString();
                    }
                    else
                    {
                        return "dummy";
                    }
                },
                AspectToStringConverter = (object x) =>
                {
                    return "";
                }
            };

            objectListView.Columns.Add(colIcon);

            OLVColumn colName = new()
            {
                Name = objectListView.Name + "_column_name",
                AspectName = "Name",
                Text = "Name",
                IsEditable = false,
                Width = 120
            };
            objectListView.Columns.Add(colName);

            OLVColumn colTitle = new()
            {
                Name = objectListView.Name + "_column_title",
                AspectName = "Title",
                Text = "Title",
                IsEditable = false,
                Width = 140
            };
            objectListView.Columns.Add(colTitle);

            OLVColumn colCurMove = new()
            {
                Name = objectListView.Name + "_column_move",
                AspectName = "CurrentOperationPackage",
                Text = "Move",
                IsEditable = false,
                Width = 86,
                AspectToStringConverter = (object x) =>
                {
                    if (x == null) return "Idle";
                    // OperationPackage package = (OperationPackage)x;
                    return "Something is going on";
                    // return package.MoveType.ToString() + " (" + package.Attempts.ToString() + ")";
                }
            };
            objectListView.Columns.Add(colCurMove);

            OLVColumn colId = new()
            {
                Name = objectListView.Name + "_column_id",
                AspectName = "Id",
                Text = "Id",
                IsEditable = false,
                Width = 220
            };
            objectListView.Columns.Add(colId);

            OLVColumn colEnv = new()
            {
                Name = objectListView.Name + "_column_env",
                AspectName = "Environment",
                Text = "Environment",
                IsEditable = false,
                Width = 80,
                AspectToStringConverter = (object env) =>
                {
                    return (env as environments.Environment).Name;
                },
                ImageGetter = (object werk) =>
                {
                    return null;
                    // return (werk as Werk).Environment.Icon;
                }
            };
            objectListView.Columns.Add(colEnv);

            OLVColumn colCreated = new()
            {
                Name = objectListView.Name + "_column_created",
                AspectName = "CreatedAt",
                Text = "Created",
                IsEditable = false,
                Width = 110
            };
            objectListView.Columns.Add(colCreated);

            OLVColumn colModified = new()
            {
                Name = objectListView.Name + "_column_modified",
                AspectName = "LastModified",
                Text = "Modified",
                IsEditable = false,
                Width = 110
            };
            objectListView.Columns.Add(colModified);


            // formatting
            objectListView.FormatCell += (sender, args) =>
            {
                /*
                if (args.Column == colCurMove)
                {
                    Werk werk = (Werk)args.Item.RowObject;
                    if (werk.CurrentOperationPackage != null)
                    {
                        foreach (OLVListSubItem subItem in args.Item.SubItems)
                        {
                            subItem.BackColor = Color.MediumTurquoise;
                        }
                    }
                }
                */
            };

            objectListView.SmallImageList = IconList;
        }

        /// <summary>
        /// Gather all the werke from the lists directory.
        /// </summary>
        public void Gather()
        {
            objectListView.DeselectAll();
            Work(this, new DoWorkEventArgs(this), true);
        }

        /// <summary>
        /// Gather all the werke from the lists directory.
        /// </summary>
        public void GatherAsync()
        {
            if (!worker.IsBusy)
            {
                objectListView.DeselectAll();
                worker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Cancel the gather.
        /// </summary>
        public void CancelAsync()
        {
            worker.CancelAsync();
        }

        private void Work(object sender, DoWorkEventArgs e, bool sync = false)
        {
            List<Werk> werke = new();

            int envIndex = 0;
            foreach (string envPath in EnvironmentRepository.Directories)
            {
                if (e.Cancel) break;

                // go through each environment in vault
                DirectoryInfo envDir = new(Path.Combine(Directory.FullName, envPath));
                if (!envDir.Exists) continue;

                // get all subdirectories of vault/environment directory
                var werkDirs = envDir.EnumerateDirectories().OrderByDescending(d => d.LastWriteTime);
                int werkCount = werkDirs.Count();

                int werkIndex = 0;
                foreach (DirectoryInfo werkDir in werkDirs)
                {
                    // stop gathering if background worker is cancelled
                    if (e.Cancel) break;

                    foreach (DirectoryInfo subDir in werkDir.GetDirectories())
                    {
                        // stop gathering if background worker is cancelled
                        if (e.Cancel) break;

                        // only look into /werk directories for gathering werke
                        if (subDir.Name == Config.DirNameWerk)
                        {
                            foreach (FileInfo file in subDir.GetFiles())
                            {
                                // stop gathering if background worker is cancelled
                                if (e.Cancel) break;

                                // skip if file is not werk.json
                                if (file.Name != Config.FileNameWerkJson) continue;

                                // deserialize werk.json
                                string text = File.ReadAllText(file.FullName);
                                Werk? werk = JsonConvert.DeserializeObject<Werk>(text);
                                if (werk == null)
                                {
                                    break;
                                }
                                werk.LastModified = werkDir.LastWriteTime;

                                // check whether werk state fits expected vault state
                                // while a werk is moving to another vault the state may not be right
                                if (werk.State != State && !werk.Moving)
                                {
                                    throw new UnexpectedWerkStateException(werk, State);
                                }

                                // check whether the werks environment matches the directory of the werk
                                if (!werk.CurrentDirectory.StartsWith(envDir.FullName))
                                {
                                    throw new UnexpectedWerkEnvironmentException(werk, envDir.FullName);
                                }

                                // check whether the computed path matches the actual path
                                if (werk.CurrentDirectory != werkDir.FullName)
                                {
                                    throw new UnexpectedWerkDirectoryNameException(werk, werkDir.FullName);
                                }

                                // add werk to list
                                werke.Add(werk);

                                // load werk icon
                                string iconFile = Path.Combine(subDir.FullName, Config.FileNameWerkIcon);
                                if (File.Exists(iconFile))
                                {
                                    Image icon;
                                    using (var bmpTemp = new Bitmap(iconFile))
                                    {
                                        icon = new Bitmap(bmpTemp);
                                    }

                                    if (IconList.Images.ContainsKey(werk.Id.ToString()))
                                    {
                                        IconList.Images.RemoveByKey(werk.Id.ToString());
                                    }
                                    IconList.Images.Add(werk.Id.ToString(), icon);
                                }
                                break;
                            }
                        }
                    }
                    werkIndex += 1;
                }
                envIndex += 1;
            }

            if (sync)
            {
                objectListView.SetObjects(werke);
            }
            e.Result = werke;
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Work(sender, e);
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                throw e.Error;
            }

            if (e.Result == null)
            {
                throw new Exception("No result after work");
            }

            List<Werk> werke = (List<Werk>)e.Result;

            // draw gathered items
            objectListView.SetObjects(werke);
        }
    }
}
