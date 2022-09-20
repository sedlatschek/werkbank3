using System.Diagnostics;
using werkbank.controls;
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

        private readonly ImageList iconList;

        private readonly FormQueue formQueue;

        private Werk? selectedWerk;

        public FormWerkbank()
        {
            vaults = new List<WerkList>();
            iconList = new ImageList();
            formQueue = new FormQueue();

            InitializeComponent();

            label_version.Text = "v" + Application.ProductVersion.ToString();

            vaultHot = new WerkList(iconList, WerkState.Hot);
            vaults.Add(vaultHot);
            splitContainer1.Panel1.Controls.Add(vaultHot);

            vaultCold = new WerkList(iconList, WerkState.Cold);
            vaults.Add(vaultCold);
            splitContainer2.Panel1.Controls.Add(vaultCold);

            vaultArchive = new WerkList(iconList, WerkState.Archived);
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
        }

        private void FormWerkbankLoad(object sender, EventArgs e)
        {
            Settings.Save();
            RefreshWerke();
        }

        private void FormWerkbankClosing(object sender, FormClosingEventArgs e)
        {
            formQueue.Save();
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

        private void VaultGatherDone(object? sender, List<Werk> werke)
        {
            formQueue.RelateWithWerke(werke);
            if (sender != null)
            {
                ((WerkList)sender).List.RefreshObjects(werke);
            }
            timerQueue.Start();
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
            werk.OpenExplorer();
        }

        private void UpdateControlsAvailability()
        {
            button_werk_up.Enabled = selectedWerk != null && selectedWerk.State != WerkState.Hot;
            button_werk_down.Enabled = selectedWerk != null && selectedWerk.State != WerkState.Archived;
            button_werk_backup.Enabled = selectedWerk != null && selectedWerk.State == WerkState.Hot;

            button_werk_open.Enabled = selectedWerk != null;
            button_werk_edit.Enabled = selectedWerk != null;
            button_werk_vscode.Enabled = selectedWerk != null;
            button_werk_web.Enabled = selectedWerk != null;
        }

        private void ButtonRefreshClick(object sender, EventArgs e)
        {
            RefreshWerke();
        }

        /// <summary>
        /// Queue a transition of a werk.
        /// </summary>
        /// <param name="transitionType"></param>
        /// <param name="werk"></param>
        /// <param name="onBatchDone"></param>
        private void QueueTransition(TransitionType transitionType, Werk werk, Action onBatchDone)
        {
            Transition transition = Transition.For(transitionType);
            Batch batch = transition.Build(werk);
            batch.OnBatchDone += (sender, e) =>
            {
                if (batch.Done)
                {
                    Invoke(onBatchDone);
                }
            };
            formQueue.Add(batch);
        }

        private void ButtonWerkUpClick(object sender, EventArgs e)
        {
            if (selectedWerk != null)
            {
                Werk werk = selectedWerk;
                if (werk.State == WerkState.Cold)
                {
                    QueueTransition(TransitionType.ColdToHot, werk, new Action(() =>
                    {
                        vaultCold.List.RemoveObject(werk);
                        vaultHot.List.AddObject(werk);
                        vaultHot.List.SelectedObject = werk;
                    }));
                }
                else if (werk.State == WerkState.Archived)
                {
                    QueueTransition(TransitionType.ArchiveToCold, werk, new Action(() =>
                    {
                        vaultArchive.List.RemoveObject(werk);
                        vaultCold.List.AddObject(werk);
                        vaultCold.List.SelectedObject = werk;
                    }));
                }
            }
        }

        private void ButtonWerkDownClick(object sender, EventArgs e)
        {
            if (selectedWerk != null)
            {
                Werk werk = selectedWerk;
                if (werk.State == WerkState.Hot)
                {
                    QueueTransition(TransitionType.HotToCold, werk, new Action(() =>
                    {
                        vaultHot.List.RemoveObject(werk);
                        vaultCold.List.AddObject(werk);
                        vaultCold.List.SelectedObject = werk;
                    }));
                }
                else if (werk.State == WerkState.Cold)
                {
                    QueueTransition(TransitionType.ColdToArchive, werk, new Action(() =>
                    {
                        vaultCold.List.RemoveObject(werk);
                        vaultArchive.List.AddObject(werk);
                        vaultArchive.List.SelectedObject = werk;
                    }));
                }
            }
        }

        private void ButtonWerkBackupClick(object sender, EventArgs e)
        {
            if (selectedWerk != null)
            {
                Werk werk = selectedWerk;
                QueueTransition(TransitionType.Backup, werk, new Action(() =>
                {
                    vaultHot.List.RefreshObject(werk);
                }));
            }
        }

        private void ButtonWerkOpenClick(object sender, EventArgs e)
        {
            selectedWerk?.OpenExplorer();
        }

        private void ButtonWerkEditClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        private void ButtonQueueClick(object sender, EventArgs e)
        {
            formQueue.ShowDialog();
        }

        private void TimerQueueTick(object sender, EventArgs e)
        {
            formQueue.Run();
        }
    }
}