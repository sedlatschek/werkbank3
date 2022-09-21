using werkbank.services;

namespace werkbank
{
    public partial class FormSettings : Form
    {
        public event EventHandler<SettingChangedEventArgs>? AutoStartChanged;
        public event EventHandler<SettingChangedEventArgs>? LaunchMinimizedChanged;
        public event EventHandler<SettingChangedEventArgs>? GatherAtLaunchChanged;
        public event EventHandler<SettingChangedEventArgs>? DirHotVaultChanged;
        public event EventHandler<SettingChangedEventArgs>? DirColdVaultChanged;
        public event EventHandler<SettingChangedEventArgs>? DirArchiveVaultChanged;
        public event EventHandler<SettingChangedEventArgs>? ArchiveCompressionLevelChanged;

        public FormSettings()
        {
            InitializeComponent();

            Text = Application.ProductName + ": Settings";

            checkBox_settings_autostart.Checked = Settings.Properties.AutoStart;
            checkBox_settings_launch_minimized.Checked = Settings.Properties.LaunchMinimized;
            checkBox_settings_gather_at_launch.Checked = Settings.Properties.GatherAtLaunch;

            textBox_settings_dir_hot.Text = Settings.Properties.DirHotVault;
            textBox_settings_dir_cold.Text = Settings.Properties.DirColdVault;
            textBox_settings_dir_archive.Text = Settings.Properties.DirArchiveVault;

            trackBar_settings_archiving_compression.Value = Settings.Properties.ArchiveCompressionLevel;
        }
        private void ButtonSettingsDirHotSelectClick(object sender, EventArgs e)
        {
            OpenFolderBrowser(textBox_settings_dir_hot);
        }

        private void ButtonSettingsDirColdSelectClick(object sender, EventArgs e)
        {
            OpenFolderBrowser(textBox_settings_dir_cold);
        }

        private void ButtonSettingsDirArchiveSelectClick(object sender, EventArgs e)
        {
            OpenFolderBrowser(textBox_settings_dir_archive);
        }

        private void OpenFolderBrowser(TextBox TextBox)
        {
            folderBrowserDialog.SelectedPath = TextBox.Text;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                TextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ButtonSaveClick(object sender, EventArgs e)
        {
            // 🤮 TODO: refactor this to automatically work for all properties

            if (checkBox_settings_autostart.Checked != Settings.Properties.AutoStart)
            {
                AutoStartChanged?.Invoke(this, new SettingChangedEventArgs() { OldValue = Settings.Properties.AutoStart, NewValue = checkBox_settings_autostart.Checked });
                Settings.Properties.AutoStart = checkBox_settings_autostart.Checked;
            }

            if (checkBox_settings_launch_minimized.Checked != Settings.Properties.LaunchMinimized)
            {
                LaunchMinimizedChanged?.Invoke(this, new SettingChangedEventArgs() { OldValue = Settings.Properties.LaunchMinimized, NewValue = checkBox_settings_launch_minimized.Checked });
                Settings.Properties.LaunchMinimized = checkBox_settings_launch_minimized.Checked;
            }

            if (checkBox_settings_gather_at_launch.Checked != Settings.Properties.GatherAtLaunch)
            {
                GatherAtLaunchChanged?.Invoke(this, new SettingChangedEventArgs() { OldValue = Settings.Properties.GatherAtLaunch, NewValue = checkBox_settings_gather_at_launch.Checked });
                Settings.Properties.GatherAtLaunch = checkBox_settings_gather_at_launch.Checked;
            }

            if (textBox_settings_dir_hot.Text != Settings.Properties.DirHotVault)
            {
                DirHotVaultChanged?.Invoke(this, new SettingChangedEventArgs() { OldValue = Settings.Properties.DirHotVault, NewValue = textBox_settings_dir_hot.Text });
                Settings.Properties.DirHotVault = textBox_settings_dir_hot.Text;
            }

            if (textBox_settings_dir_cold.Text != Settings.Properties.DirColdVault)
            {
                DirColdVaultChanged?.Invoke(this, new SettingChangedEventArgs() { OldValue = Settings.Properties.DirColdVault, NewValue = textBox_settings_dir_cold.Text });
                Settings.Properties.DirColdVault = textBox_settings_dir_cold.Text;
            }

            if (textBox_settings_dir_archive.Text != Settings.Properties.DirArchiveVault)
            {
                DirArchiveVaultChanged?.Invoke(this, new SettingChangedEventArgs() { OldValue = Settings.Properties.DirArchiveVault, NewValue = textBox_settings_dir_archive.Text });
                Settings.Properties.DirArchiveVault = textBox_settings_dir_archive.Text;
            }

            if (trackBar_settings_archiving_compression.Value != Settings.Properties.ArchiveCompressionLevel)
            {
                ArchiveCompressionLevelChanged?.Invoke(this, new SettingChangedEventArgs() { OldValue = Settings.Properties.ArchiveCompressionLevel, NewValue = trackBar_settings_archiving_compression.Value });
                Settings.Properties.ArchiveCompressionLevel = trackBar_settings_archiving_compression.Value;
            }

            Settings.Save();

            DialogResult = DialogResult.OK;
            Close();
        }
    }

    public class SettingChangedEventArgs : EventArgs
    {
        public object? OldValue { get; set; }
        public object? NewValue { get; set; }
    }
}
