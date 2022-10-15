using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using BrightIdeasSoftware;
using werkbank.controls;
using werkbank.exceptions;
using werkbank.models;
using werkbank.services;
using werkbank.transitions;

namespace werkbank
{
    public partial class FormWerkbank : Form
    {
        public static string Title => Application.ProductName;

        private readonly List<WerkList> vaults;
        private readonly WerkList vaultHot;
        private readonly WerkList vaultCold;
        private readonly WerkList vaultArchive;
        private int gatherDoneCount = 0;
        private readonly ImageList iconList;
        private readonly FormQueue formQueue;
        private Werk? selectedWerk;

        #region "start & close"
        public FormWerkbank()
        {
            vaults = new List<WerkList>();
            iconList = new ImageList();
            formQueue = new FormQueue();
            formQueue.Changed += FormQueueChanged;
            formQueue.BatchDone += FormQueueBatchDone;
            formQueue.BatchAdded += FormQueueBatchAdded;

            InitializeComponent();

            Icon = Properties.Resources.logo;
            Text = Title;
            label_version.Text = "v" + Assembly.GetEntryAssembly()?.GetName().Version?.ToString();

            vaultHot = new WerkList(iconList, WerkState.Hot)
            {
                Text = "Hot Vault",
                Icon = Properties.Resources.vault_hot
            };
            vaults.Add(vaultHot);
            splitContainer1.Panel1.Controls.Add(vaultHot);

            vaultCold = new WerkList(iconList, WerkState.Cold)
            {
                Text = "Cold Vault",
                Icon = Properties.Resources.vault_cold
            };
            vaults.Add(vaultCold);
            splitContainer2.Panel1.Controls.Add(vaultCold);

            vaultArchive = new WerkList(iconList, WerkState.Archived)
            {
                Text = "Archive",
                Icon = Properties.Resources.vault_archive
            };
            vaults.Add(vaultArchive);
            splitContainer2.Panel2.Controls.Add(vaultArchive);

            foreach (WerkList vault in vaults)
            {
                vault.Dock = DockStyle.Fill;
                vault.WerkSelected += WerkSelected;
                vault.WerkDoubleClick += WerkDoubleClick;
                vault.GatherDone += VaultGatherDone;
                vault.TransitionDetected += VaultTransitionDetected;
            }

            timerQueue.Interval = Settings.Properties.QueueTickInterval;

            notifyIcon.Icon = Properties.Resources.logo;

            UpdateControlsAvailability();
            UpdateQueueProgressBar();
            UpdateNotifyIconContextMenu();
        }

        private void VaultTransitionDetected(object? sender, Werk e)
        {
            formQueue.RelateWithWerk(e);
        }

        private void FormWerkbankLoad(object sender, EventArgs e)
        {
            if (Settings.Properties.LaunchMinimized)
            {
                WindowState = FormWindowState.Minimized;
            }
        }
        private void FormWerkbankShown(object sender, EventArgs e)
        {
            if (!File.Exists(Config.FileSettings))
            {
                ShowSettingsForm();
            }
            else if (Settings.Properties.GatherAtLaunch)
            {
                RefreshWerke();
            }
        }

        private void FormWerkbankClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                WinApiService.ShowWindow(Handle, WinApiService.ShowWindowCommands.Minimize);
            }
            else
            {
                formQueue.Save();
            }
        }
        #endregion

        #region "queue"
        private void TimerQueueTick(object sender, EventArgs e)
        {
            formQueue.Run();
        }

        private void FormQueueChanged(object? sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                UpdateQueueProgressBar();
            }));
        }

        private void UpdateQueueProgressBar()
        {
            progressBar_queue.Visible = formQueue.TotalOperationsCount > 0;
            progressBar_queue.Maximum = formQueue.TotalOperationsCount;
            progressBar_queue.Value = formQueue.DoneOperationsCount;
        }

        /// <summary>
        /// Queue a transition of a werk.
        /// </summary>
        /// <param name="transitionType"></param>
        /// <param name="werk"></param>
        /// <param name="onBatchDone"></param>
        private void QueueTransition(TransitionType transitionType, Werk werk)
        {
            Batch batch = Transition.For(transitionType).Build(werk);
            formQueue.Add(batch);
        }
        private void FormQueueBatchAdded(object? sender, Batch batch)
        {
            UpdateControlsAvailability();
        }

        private void FormQueueBatchDone(object? sender, Batch batch)
        {
            Werk? werk = batch.Werk;

            Invoke(new Action(() =>
            {
                switch (batch.TransitionType)
                {
                    case TransitionType.HotToCold:
                    case TransitionType.ColdToHot:
                    case TransitionType.ColdToArchive:
                    case TransitionType.ArchiveToCold:
                        WerkList fromVault = GetVaultFor(Transition.From(batch.TransitionType));
                        WerkList toVault = GetVaultFor(Transition.To(batch.TransitionType));

                        Werk? werk = fromVault.RemoveWerkById(batch.WerkId) ?? batch.Werk;
                        if (werk != null)
                        {
                            toVault.List.AddObject(werk);
                            toVault.List.SelectObject(werk);
                        }
                        break;
                    case TransitionType.Delete:
                        if (batch.Werk != null)
                        {
                            GetVaultFor(batch.Werk.State).RemoveWerkById(batch.WerkId);
                        }
                        break;
                    case TransitionType.Backup:
                    case TransitionType.Environment:
                        break;
                    default: throw new UnhandledTransitionTypeException(batch.TransitionType);
                }
            }));

            batch.Untie();

            Invoke(new Action(() =>
            {
                UpdateControlsAvailability();
                UpdateNotifyIconContextMenu();
                if (werk != null)
                {
                    GetVaultFor(werk.State).List.RefreshObject(werk);
                }
            }));
        }
        #endregion

        #region "vaults"
        /// <summary>
        /// Get a list of all werke from all vaults.
        /// </summary>
        /// <returns></returns>
        public List<Werk> GetWerke()
        {
            if (vaultHot == null
                || vaultHot.List.Objects == null
                || vaultCold == null
                || vaultCold.List.Objects == null
                || vaultArchive == null
                || vaultArchive.List.Objects == null)
            {
                return new List<Werk>();
            }
            return vaultHot.List.Objects.Cast<Werk>()
                .Concat(vaultCold.List.Objects.Cast<Werk>())
                .Concat(vaultArchive.List.Objects.Cast<Werk>())
                .ToList();
        }

        /// <summary>
        /// Clear all werk lists and gather them again.
        /// </summary>
        private void RefreshWerke()
        {
            foreach (WerkList vault in vaults)
            {
                if (!vault.Busy)
                {
                    vault.GatherAsync();
                }
            }
        }

        private void VaultGatherDone(object? sender, EventArgs e)
        {
            gatherDoneCount++;

            if (sender != null)
            {
                WerkList vault = (WerkList)sender;
                formQueue.RelateWithWerke(vault.List.Objects.Cast<Werk>().ToList());
            }

            // We start the queue once all vaults have atleast gather once,
            // otherwise the batches may still have null set for the Werk property.
            if (gatherDoneCount >= 3)
            {
                timerQueue.Start();
                UpdateNotifyIconContextMenu();
                VerifyIntegrity();
            }

            UpdateControlsAvailability();
        }

        private void WerkSelected(object? sender, Werk? werk)
        {
            selectedWerk = werk;

            foreach (WerkList vault in vaults)
            {
                if (werk == null || vault.State != werk.State)
                {
                    vault.List.DeselectAll();
                }
            }

            UpdateControlsAvailability();
        }

        private void WerkDoubleClick(object? sender, Werk werk)
        {
            werk.OpenInFileExplorer();
        }

        /// <summary>
        /// Get the vault list for a given state.
        /// </summary>
        /// <param name="State"></param>
        /// <returns></returns>
        /// <exception cref="UnhandledWerkStateException"></exception>
        private WerkList GetVaultFor(WerkState State)
        {
            return State switch
            {
                WerkState.Hot => vaultHot,
                WerkState.Cold => vaultCold,
                WerkState.Archived => vaultArchive,
                _ => throw new UnhandledWerkStateException(State),
            };
        }

        private void VerifyIntegrity()
        {
            List<Werk> werke = GetWerke();

            // check whether some ids have been assigned multiple times
            // this will likely only occur when somebody copied a werk
            var duplicateId = werke.GroupBy(w => w.Id)
                    .Select(group => new
                    {
                        Id = group.Key,
                        Count = group.Count()
                    })
                    .Where(w => w.Count > 1)
                    .FirstOrDefault();
            if (duplicateId != null)
            {
                throw new DuplicateWerkException(duplicateId.Id);
            }

            // check whether some names have been assigned multiple times
            var duplicateName = werke.GroupBy(w => w.Name)
                .Select(group => new
                {
                    Name = group.Key,
                    Count = group.Count()
                })
                .Where(w => w.Count > 1)
                .FirstOrDefault();
            if (duplicateName != null)
            {
                throw new DuplicateWerkException(duplicateName.Name);
            }
        }
        #endregion

        #region "search"
        private void SearchTextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_search.Text))
            {
                foreach (WerkList vault in vaults)
                {
                    vault.List.UseFiltering = false;
                }
            }
            else
            {
                foreach (WerkList vault in vaults)
                {
                    vault.List.ModelFilter = new ModelFilter(delegate (object w)
                    {
                        Werk werk = (Werk)w;
                        return (textBox_search.Text.Length == 36 && Guid.TryParse(textBox_search.Text, out Guid guid) && werk.Id.Equals(guid))
                          || werk.Name.ToLower(CultureInfo.CurrentCulture).Contains(textBox_search.Text.ToLower(CultureInfo.CurrentCulture))
                          || werk.Title.ToLower(CultureInfo.CurrentCulture).Contains(textBox_search.Text.ToLower(CultureInfo.CurrentCulture));
                    });
                    vault.List.UseFiltering = true;
                }
            }
        }
        #endregion

        #region "controls"
        private void UpdateControlsAvailability()
        {
            button_werk_up.Enabled = selectedWerk != null
                && selectedWerk.State != WerkState.Hot
                && selectedWerk.TransitionType == null;
            button_werk_down.Enabled = selectedWerk != null
                && selectedWerk.State != WerkState.Archived
                && selectedWerk.TransitionType == null;
            button_werk_backup.Enabled = selectedWerk != null
                && selectedWerk.State == WerkState.Hot
                && selectedWerk.TransitionType == null;

            button_werk_open.Enabled = selectedWerk != null
                && selectedWerk.TransitionType == null;
            button_werk_edit.Enabled = selectedWerk != null
                && selectedWerk.TransitionType == null;
            button_werk_delete.Enabled = selectedWerk != null
                && selectedWerk.TransitionType == null;
            button_werk_history.Enabled = selectedWerk != null
                && selectedWerk.TransitionType == null;
            button_werk_vscode.Enabled = selectedWerk != null
                && selectedWerk.TransitionType == null
                && RegistryService.GetVSCodePath() != null;
            button_werk_web.Enabled = selectedWerk != null
                && selectedWerk.HasGit;
        }

        private void ButtonRefreshClick(object sender, EventArgs e)
        {
            RefreshWerke();
        }

        private void ButtonCreateWerkClick(object sender, EventArgs e)
        {
            ShowWerkForm(FormWerk.FormWerkMode.Create);
        }

        private void ButtonWerkEditClick(object sender, EventArgs e)
        {
            ShowWerkForm(FormWerk.FormWerkMode.Edit);
        }

        private void ShowWerkForm(FormWerk.FormWerkMode WerkMode, FormStartPosition StartPosition = FormStartPosition.CenterParent)
        {
            // retrieve the most used environment for create mode preselection
            environments.Environment? environment = WerkMode == FormWerk.FormWerkMode.Edit
                ? null
                : GetWerke()
                    .GroupBy(w => w.Environment)
                    .Select(group => new
                    {
                        Environment = group.Key,
                        Count = group.Count()
                    })
                    .OrderByDescending(w => w.Count)
                    .First().Environment;

            FormWerk formWerk = new(iconList, environment)
            {
                Mode = WerkMode,
                StartPosition = StartPosition
            };

            if (WerkMode == FormWerk.FormWerkMode.Edit)
            {
                formWerk.EnvironmentChanged += WerkEnvironmentChanged;
                formWerk.Werk = selectedWerk ?? throw new NullReferenceException("selectedWerk");
            }

            if (formWerk.ShowDialog() == DialogResult.OK && formWerk.Werk != null)
            {
                if (WerkMode == FormWerk.FormWerkMode.Create)
                {
                    vaultHot.List.AddObject(formWerk.Werk);
                    formWerk.Werk.OpenInFileExplorer();
                }
                else if (WerkMode == FormWerk.FormWerkMode.Edit)
                {
                    GetVaultFor(formWerk.Werk.State).List.RefreshObject(formWerk.Werk);
                }
            }
        }

        private void ButtonWerkDeleteClick(object sender, EventArgs e)
        {
            if (selectedWerk != null)
            {
                string message = "Do you really want to delete " + selectedWerk.Name + " (" + selectedWerk.Title + ")?";
                if (MessageBox.Show(message, Text + ": Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    QueueTransition(TransitionType.Delete, selectedWerk);
                }
            }
        }

        private void WerkEnvironmentChanged(object? sender, FormWerk.EnvironmentChangedEventArgs e)
        {
            if (selectedWerk != null)
            {
                EnvironmentTransition transition = new();
                Batch batch = transition.Build(selectedWerk, e.NewEnvironment);
                formQueue.Add(batch);
            }
        }

        private void ButtonHistoryClick(object sender, EventArgs e)
        {
            if (selectedWerk != null)
            {
                FormHistory formHistory = new()
                {
                    Werk = selectedWerk,
                    ColorHot = splitContainer1.Panel1.BackColor,
                    ColorCold = splitContainer2.Panel1.BackColor,
                    ColorArchived = splitContainer2.Panel2.BackColor,
                };
                formHistory.ShowDialog();
            }
        }

        private void ButtonWerkUpClick(object sender, EventArgs e)
        {
            if (selectedWerk != null)
            {
                if (selectedWerk.State == WerkState.Cold)
                {
                    QueueTransition(TransitionType.ColdToHot, selectedWerk);
                }
                else if (selectedWerk.State == WerkState.Archived)
                {
                    QueueTransition(TransitionType.ArchiveToCold, selectedWerk);
                }
            }
        }

        private void ButtonWerkDownClick(object sender, EventArgs e)
        {
            if (selectedWerk != null)
            {
                if (selectedWerk.State == WerkState.Hot)
                {
                    QueueTransition(TransitionType.HotToCold, selectedWerk);
                }
                else if (selectedWerk.State == WerkState.Cold)
                {
                    QueueTransition(TransitionType.ColdToArchive, selectedWerk);
                }
            }
        }

        private void ButtonWerkBackupClick(object sender, EventArgs e)
        {
            if (selectedWerk != null)
            {
                QueueTransition(TransitionType.Backup, selectedWerk);
            }
        }

        private void ButtonWerkOpenClick(object sender, EventArgs e)
        {
            selectedWerk?.OpenInFileExplorer();
        }

        private void ButtonWerkVsCodeClick(object sender, EventArgs e)
        {
            selectedWerk?.OpenInVsCode();
        }

        private void ButtonWerkWebClick(object sender, EventArgs e)
        {
            if (selectedWerk != null)
            {
                string? url = selectedWerk.GetGitRemoteUrl();
                if (url != null)
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("There is not remote URL configured for this werk repository.", Title + ": Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void ButtonSettingsClick(object sender, EventArgs e)
        {
            ShowSettingsForm();
        }

        private void ShowSettingsForm()
        {
            FormSettings formSettings = new();
            formSettings.AutoStartChanged += SettingAutoStartChanged;
            formSettings.DirHotVaultChanged += SettingDirHotVaultChanged;
            formSettings.DirColdVaultChanged += SettingDirColdVaultChanged;
            formSettings.DirArchiveVaultChanged += SettingDirArchiveVaultChanged;
            formSettings.ShowDialog();
        }
        private void ButtonStatisticsClick(object sender, EventArgs e)
        {
            FormStatistics formStatistics = new(GetWerke());
            formStatistics.ShowDialog();
        }

        private void ButtonQueueClick(object sender, EventArgs e)
        {
            formQueue.ShowDialog();
        }
        #endregion

        #region "settings"
        private void SettingAutoStartChanged(object? sender, SettingChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                bool autoStart = (bool)e.NewValue;
                RegistryService.UpdateApplicationAutoStart(autoStart);
            }
        }

        private void SettingDirHotVaultChanged(object? sender, SettingChangedEventArgs e)
        {
            vaultHot.GatherAsync();
        }

        private void SettingDirColdVaultChanged(object? sender, SettingChangedEventArgs e)
        {
            vaultCold.GatherAsync();
        }
        private void SettingDirArchiveVaultChanged(object? sender, SettingChangedEventArgs e)
        {
            vaultArchive.GatherAsync();
        }
        #endregion

        #region "notify icon"
        private void FormWerkbankResize(object sender, EventArgs e)
        {
            ShowInTaskbar = WindowState != FormWindowState.Minimized;
        }

        private void NotifyIconClick(object sender, EventArgs e)
        {
            WinApiService.ShowWindow(Handle, WinApiService.ShowWindowCommands.Restore);
        }

        private void UpdateNotifyIconContextMenu()
        {
            notifyContextMenuStrip.Items.Clear();

            notifyContextMenuStrip.ImageList = iconList;

            List<Werk> werke = GetWerke();

            foreach (Werk werk in werke.OrderByDescending(w => w.LastModified).Take(10))
            {
                ToolStripItem item = notifyContextMenuStrip.Items.Add(werk.Name);
                if (werk.HasIcon)
                {
                    item.ImageKey = werk.Id.ToString();
                }
                else
                {
                    item.ImageKey = "dummy";
                }
                item.Click += (sender, e) =>
                {
                    werk.OpenInFileExplorer();
                };
            }

            if (werke.Count > 0)
            {
                notifyContextMenuStrip.Items.Add(new ToolStripSeparator());
            }

            notifyContextMenuStrip.Items.Add("Create Werk").Click += (sender, e) =>
            {
                ShowWerkForm(FormWerk.FormWerkMode.Create, FormStartPosition.CenterScreen);
            };
            notifyContextMenuStrip.Items.Add("Exit").Click += (sender, e) =>
            {
                Application.Exit();
            };
        }
        #endregion
    }
}