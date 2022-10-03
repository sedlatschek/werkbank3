using BrightIdeasSoftware;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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

        /// <summary>
        /// The gather is done.
        /// </summary>
        public event EventHandler? GatherDone;

        /// <summary>
        /// Triggers whenever a werk is selected.
        /// </summary>
        public event EventHandler<Werk?>? WerkSelected;

        /// <summary>
        /// Triggers whenever a werk is double clicked.
        /// </summary>
        public event EventHandler<Werk>? WerkDoubleClick;

        /// <summary>
        /// Triggers whenever a werk in transition is detected. This may also include werke, that do not make it to the list, as they are inbetween states.
        /// </summary>
        public event EventHandler<Werk>? TransitionDetected;

        public override string Text
        {
            get { return rotatingLabel_title.NewText; }
            set { rotatingLabel_title.NewText = value; }
        }

        public Image Icon
        {
            get { return pictureBox_vault_icon.Image; }
            set { pictureBox_vault_icon.Image = value; }
        }

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
            objectListView.SmallImageList = IconList;

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
                Width = 120,
                Sortable = true
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

            OLVColumn colCurTransition = new()
            {
                Name = objectListView.Name + "_column_transition",
                AspectName = "CurrentBatch",
                Text = "Transition",
                IsEditable = false,
                Width = 134,
                AspectToStringConverter = (object x) =>
                {
                    if (x == null) return "Idle";
                    Batch batch = (Batch)x;
                    return batch.Title + " (" + batch.ProgressPercentage.ToString() + "%)";
                }
            };
            objectListView.Columns.Add(colCurTransition);

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
                    return ((environments.Environment)env).Name;
                },
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
                if (args.Column == colCurTransition)
                {
                    Werk werk = (Werk)args.Item.RowObject;

                    if (werk.TransitionType != null)
                    {
                        foreach (OLVListSubItem subItem in args.Item.SubItems)
                        {
                            subItem.BackColor = Color.LightSteelBlue;
                        }
                    }
                }
            };

            objectListView.Sorting = SortOrder.Ascending;
            objectListView.PrimarySortColumn = colName;
            objectListView.ShowGroups = false;
            objectListView.UseCellFormatEvents = true;
        }

        /// <summary>
        /// Gather all the werke from the lists directory.
        /// </summary>
        public void Gather()
        {
            objectListView.DeselectAll();
            panel_loading.Visible = true;
            objectListView.Enabled = false;
            Work(new DoWorkEventArgs(this), true);
        }

        /// <summary>
        /// Gather all the werke from the lists directory.
        /// </summary>
        public void GatherAsync()
        {
            if (!worker.IsBusy)
            {
                objectListView.DeselectAll();
                panel_loading.Visible = true;
                objectListView.Enabled = false;
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

        private void Work(DoWorkEventArgs e, bool sync = false)
        {
            List<Werk> werke = new();

            if (!sync) worker.ReportProgress(0);

            int envIndex = 0;
            int envCount = EnvironmentRepository.Environments.Count;

            foreach (string envPath in EnvironmentRepository.Directories)
            {
                if (e.Cancel) break;

                int partitionEnvPercentage = 100 / envCount;
                int baseEnvPercentage = partitionEnvPercentage * envIndex;
                int maxEnvPercentage = baseEnvPercentage + partitionEnvPercentage;


                // go through each environment in vault
                DirectoryInfo envDir = new(Path.Combine(Directory.FullName, envPath));
                if (!envDir.Exists)
                {
                    if (!sync) worker.ReportProgress(maxEnvPercentage);
                    continue;
                }

                // get all subdirectories of vault/environment directory
                var werkDirs = envDir.EnumerateDirectories().OrderByDescending(d => d.LastWriteTime);
                int werkCount = werkDirs.Count();

                int werkIndex = 0;
                foreach (DirectoryInfo werkDir in werkDirs)
                {
                    // stop gathering if background worker is cancelled
                    if (e.Cancel) break;

                    if (!sync) worker.ReportProgress(baseEnvPercentage + partitionEnvPercentage / werkCount * werkIndex, werkDir.Name);

                    foreach (DirectoryInfo subDir in werkDir.GetDirectories())
                    {
                        // stop gathering if background worker is cancelled
                        if (e.Cancel) break;

                        // only look into /werk directories for gathering werke
                        if (subDir.Name == Config.DirNameMeta)
                        {
                            foreach (FileInfo file in subDir.GetFiles())
                            {
                                // stop gathering if background worker is cancelled
                                if (e.Cancel) break;

                                // skip if file is not werk.json
                                if (file.Name != Config.FileNameMetaJson) continue;

                                // deserialize werk.json
                                string text = File.ReadAllText(file.FullName);
                                Werk? werk = JsonConvert.DeserializeObject<Werk>(text);

                                if (werk == null)
                                {
                                    break;
                                }

                                // werk is transitioning
                                if (werk.TransitionType != null)
                                {
                                    TransitionDetected?.Invoke(this, werk);

                                    // if this werk is in the target vault, skip, we only want the source werk.
                                    if (werk.State != State)
                                    {
                                        break;
                                    }
                                }

                                werk.LastModified = werkDir.LastWriteTime;

                                // check whether werk state fits expected vault state
                                // while a werk is moving to another vault the state may not be right
                                if (werk.State != State && werk.TransitionType == null)
                                {
                                    throw new UnexpectedWerkStateException(werk, State);
                                }

                                // check whether the werks environment matches the directory of the werk
                                if (!werk.CurrentDirectory.StartsWith(envDir.FullName) && werk.TransitionType == null)
                                {
                                    throw new UnexpectedWerkEnvironmentException(werk, envDir.FullName);
                                }

                                // check whether the computed path matches the actual path
                                if (werk.CurrentDirectory != werkDir.FullName && werk.TransitionType == null)
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

                                    // experimental: ImageList does not seem to be thread safe
                                    Invoke(new Action(() =>
                                    {
                                        if (IconList.Images.ContainsKey(werk.Id.ToString()))
                                        {
                                            IconList.Images.RemoveByKey(werk.Id.ToString());
                                        }
                                        IconList.Images.Add(werk.Id.ToString(), icon);
                                    }));
                                }
                                break;
                            }
                        }
                    }
                    werkIndex += 1;
                }

                if (!sync) worker.ReportProgress(maxEnvPercentage);

                envIndex += 1;
            }

            if (sync)
            {
                SetWerke(werke);
                panel_loading.Visible = false;
                objectListView.Enabled = true;
            }
            e.Result = werke;

            worker.ReportProgress(100, "Done");
        }

        private void OnGather(object sender, DoWorkEventArgs e)
        {
            Work(e);
        }

        private void OnGatherProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            string state = e.UserState != null ? " - " + e.UserState.ToString() : "";
            label_progress.Text = e.ProgressPercentage + "%" + state;
        }

        private void OnGatherCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                throw e.Error;
            }

            if (e.Result == null)
            {
                throw new Exception("No result after work");
            }

            progressBar.Value = progressBar.Maximum;
            timer_hide_loading.Start();

            // draw gathered items
            List<Werk> werke = (List<Werk>)e.Result;
            SetWerke(werke);
        }

        /// <summary>
        /// Set the werke that are part of the table. Missing werke will be removed, existing ignored, and new will be added.
        /// </summary>
        /// <param name="Werke"></param>
        private void SetWerke(List<Werk> Werke)
        {
            List<Werk> curWerke = objectListView.Objects != null ? objectListView.Objects.Cast<Werk>().ToList() : new List<Werk>();

            foreach (Werk werk in Werke)
            {
                int existingIndex = curWerke.FindIndex((w) => w.Id == werk.Id);
                if (existingIndex != -1)
                {
                    if (werk.Name != curWerke[existingIndex].Name)
                    {
                        throw new DuplicateWerkException(werk.Id);
                    }

                    // only update some properties
                    curWerke[existingIndex].LastModified = werk.LastModified;
                }
                else
                {
                    curWerke.Add(werk);
                }
            }

            // remove werke that were not picked up during the latest gather
            foreach (Werk curWerk in curWerke)
            {
                int newIndex = Werke.FindIndex((w) => w.Id == curWerk.Id);
                if (newIndex == -1)
                {
                    curWerke.Remove(curWerk);
                }
            }

            objectListView.SetObjects(curWerke);

            // we do this so updated objects will also display the changes
            objectListView.RefreshObjects(curWerke);

            GatherDone?.Invoke(this, EventArgs.Empty);
        }

        #region "ui"
        private void WerkListSizeChanged(object sender, EventArgs e)
        {
            AlignLoadingPanel();
        }

        private void AlignLoadingPanel()
        {
            panel_loading.Left = Width / 2 - panel_loading.Width / 2;
            panel_loading.Top = Height / 2 - panel_loading.Height / 2;
        }

        private void TimerHideLoadingTick(object sender, EventArgs e)
        {
            timer_hide_loading.Enabled = false;
            objectListView.Enabled = true;
            panel_loading.Visible = false;
        }
        #endregion

        private void ObjectListViewItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (objectListView.SelectedObject != null)
            {
                WerkSelected?.Invoke(this, (Werk)objectListView.SelectedObject);
            }
            else
            {
                WerkSelected?.Invoke(this, null);
            }
        }

        private void ObjectListViewMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (objectListView.SelectedObject != null)
            {
                WerkDoubleClick?.Invoke(this, (Werk)objectListView.SelectedObject);
            }
        }

        private void ObjectListViewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
            {
                if (objectListView.SelectedObject != null)
                {
                    WerkDoubleClick?.Invoke(this, (Werk)objectListView.SelectedObject);
                }
            }
        }

        /// <summary>
        /// Get a werk object from the vault by its id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Werk? GetWerkById(Guid Id)
        {
            return objectListView.Objects.Cast<Werk>().ToList().Find(w => w.Id == Id);
        }

        /// <summary>
        /// Remove a werk object from the vault by its id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Werk? RemoveWerkById(Guid Id)
        {
            Werk? werk = GetWerkById(Id);
            if (werk == null)
            {
                return null;
            }
            objectListView.RemoveObject(werk);
            return werk;
        }
    }
}

