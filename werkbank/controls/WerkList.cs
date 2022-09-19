﻿using BrightIdeasSoftware;
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
using werkbank.transitions;

namespace werkbank.controls
{
    public partial class WerkList : UserControl
    {
        public readonly WerkState State;

        private readonly ImageList IconList;

        /// <summary>
        /// The gather is done.
        /// </summary>
        public event EventHandler<List<Werk>>? GatherDone;

        /// <summary>
        /// Triggers whenever a werk is selected.
        /// </summary>
        public event EventHandler<Werk?>? WerkSelected;

        /// <summary>
        /// Triggers whenever a werk is double clicked.
        /// </summary>
        public event EventHandler<Werk>? WerkDoubleClick;

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

            OLVColumn colCurTransition = new()
            {
                Name = objectListView.Name + "_column_transition",
                AspectName = "CurrentBatch",
                Text = "Transition",
                IsEditable = false,
                Width = 108,
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
                    if (werk.CurrentBatch != null)
                    {
                        foreach (OLVListSubItem subItem in args.Item.SubItems)
                        {
                            subItem.BackColor = Color.MediumTurquoise;
                        }
                    }
                }
            };

            objectListView.SmallImageList = IconList;
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
                                if (!werk.CurrentDirectory.StartsWith(envDir.FullName) && !werk.Moving)
                                {
                                    throw new UnexpectedWerkEnvironmentException(werk, envDir.FullName);
                                }

                                // check whether the computed path matches the actual path
                                if (werk.CurrentDirectory != werkDir.FullName && !werk.Moving)
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

                if (!sync) worker.ReportProgress(maxEnvPercentage);

                envIndex += 1;
            }

            if (sync)
            {
                objectListView.SetObjects(werke);
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
            objectListView.SetObjects(werke);

            GatherDone?.Invoke(this, werke);
        }

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
    }
}