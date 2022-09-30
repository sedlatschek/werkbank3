using System.Diagnostics;
using System.IO.Compression;
using System.Media;
using System.Reflection;

namespace installer
{
    public partial class FormInstaller : Form
    {
        private const string AppHandle = "werkbank3";

        private readonly string DestDir;
        private readonly string DestExe;
        private readonly string DesktopLink;

        public FormInstaller()
        {
            DestDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppHandle);
            DestExe = Path.Combine(DestDir, "werkbank.exe");
            DesktopLink = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Werkbank.lnk");

            InitializeComponent();

            textBox_info.Text = textBox_info.Text
                .Replace("{{version}}", "v" + Assembly.GetEntryAssembly()?.GetName().Version?.ToString())
                .Replace("{{user}}", Environment.UserName);
        }

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ButtonContinueClick(object? sender, EventArgs e)
        {
            panel_controls.Enabled = false;
            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.Enabled = true;
            worker.RunWorkerAsync();
        }

        private void Install(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            // setup temp dir
            string tempDir = Path.Combine(Path.GetTempPath(), AppHandle + "_installer", Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);

            // write zip to disk
            string zipFile = Path.Combine(tempDir, "bin.zip");
            File.WriteAllBytes(zipFile, Properties.Resources.bin);

            // kill running instances
            foreach (var process in Process.GetProcessesByName("werkbank"))
            {
                process.Kill();
            }

            // clear destination directory
            if (Directory.Exists(DestDir))
            {
                Directory.Delete(DestDir, true);
            }
            Directory.CreateDirectory(DestDir);

            // extract files to dest dir
            ZipFile.ExtractToDirectory(zipFile, DestDir);

            // create shortcut
            if (!File.Exists(DesktopLink))
            {
                File.WriteAllBytes(DesktopLink, Properties.Resources.shortcut);
            }
        }

        private void InstallCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.Value = progressBar.Maximum;
            panel_controls.Enabled = true;

            if (e.Error != null)
            {
                button_continue.Text = "Retry";
                SystemSounds.Exclamation.Play();
                MessageBox.Show(e.Error.ToString(), AppHandle + ": Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                button_continue.Text = "Done";
                button_continue.Click -= ButtonContinueClick;
                button_continue.Click += (sender, e) =>
                {
                    Process.Start(DestExe);
                    Close();
                };
                SystemSounds.Asterisk.Play();
            }
        }
    }
}