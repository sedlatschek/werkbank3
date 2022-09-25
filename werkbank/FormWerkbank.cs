using System.Diagnostics;
using werkbank.controls;
using werkbank.exceptions;
using werkbank.models;
using werkbank.services;
using werkbank.transitions;

namespace werkbank
{
    public partial class FormWerkbank : Form
    {
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

            Text = Application.ProductName;
            label_version.Text = "v" + Application.ProductVersion.ToString();

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
            }

            timerQueue.Interval = Settings.Properties.QueueTickInterval;

            UpdateControlsAvailability();
            UpdateQueueProgressBar();
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
            formQueue.Save();
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

                        Werk? werk = fromVault.RemoveWerkById(batch.WerkId);
                        if (werk != null)
                        {
                            toVault.List.AddObject(werk);
                            toVault.List.SelectObject(werk);
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
                if (werk != null)
                {
                    GetVaultFor(werk.State).List.RefreshObject(werk);
                }
            }));
        }
        #endregion

        #region "vaults"
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
            button_werk_history.Enabled = selectedWerk != null
                && selectedWerk.TransitionType == null;
            button_werk_vscode.Enabled = selectedWerk != null
                && selectedWerk.TransitionType == null;
            button_werk_web.Enabled = selectedWerk != null;
        }

        private void ButtonRefreshClick(object sender, EventArgs e)
        {
            RefreshWerke();
        }

        private void ButtonCreateWerkClick(object sender, EventArgs e)
        {
            FormWerk formWerk = new(iconList)
            {
                Mode = FormWerk.FormWerkMode.Create
            };
            if (formWerk.ShowDialog() == DialogResult.OK && formWerk.Werk != null)
            {
                vaultHot.List.AddObject(formWerk.Werk);
                formWerk.Werk.OpenInFileExplorer();
            }
        }

        private void ButtonWerkEditClick(object sender, EventArgs e)
        {
            if (selectedWerk != null)
            {
                FormWerk formWerk = new(iconList)
                {
                    Mode = FormWerk.FormWerkMode.Edit,
                    Werk = selectedWerk
                };
                formWerk.EnvironmentChanged += WerkEnvironmentChanged;
                if (formWerk.ShowDialog() == DialogResult.OK && formWerk.Werk != null)
                {
                    GetVaultFor(formWerk.Werk.State).List.RefreshObject(formWerk.Werk);
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
            throw new NotImplementedException();
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
    }
}