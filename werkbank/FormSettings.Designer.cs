namespace werkbank
{
    partial class FormSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.groupBox_settings_startup = new System.Windows.Forms.GroupBox();
            this.checkBox_settings_gather_at_launch = new System.Windows.Forms.CheckBox();
            this.checkBox_settings_launch_minimized = new System.Windows.Forms.CheckBox();
            this.checkBox_settings_autostart = new System.Windows.Forms.CheckBox();
            this.groupBox_settings_directories = new System.Windows.Forms.GroupBox();
            this.label_settings_dir_archive = new System.Windows.Forms.Label();
            this.button_settings_dir_archive_select = new System.Windows.Forms.Button();
            this.textBox_settings_dir_archive = new System.Windows.Forms.TextBox();
            this.label_settings_dir_cold = new System.Windows.Forms.Label();
            this.button_settings_dir_cold_select = new System.Windows.Forms.Button();
            this.textBox_settings_dir_cold = new System.Windows.Forms.TextBox();
            this.label_settings_dir_hot = new System.Windows.Forms.Label();
            this.button_settings_dir_hot_select = new System.Windows.Forms.Button();
            this.textBox_settings_dir_hot = new System.Windows.Forms.TextBox();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_save = new System.Windows.Forms.Button();
            this.groupBox_archiving = new System.Windows.Forms.GroupBox();
            this.label_settings_archiving_compression_desc2 = new System.Windows.Forms.Label();
            this.label_settings_archiving_compression_desc1 = new System.Windows.Forms.Label();
            this.label_settings_archiving_compression = new System.Windows.Forms.Label();
            this.trackBar_settings_archiving_compression = new System.Windows.Forms.TrackBar();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox_settings_startup.SuspendLayout();
            this.groupBox_settings_directories.SuspendLayout();
            this.groupBox_archiving.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_settings_archiving_compression)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox_settings_startup
            // 
            this.groupBox_settings_startup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_settings_startup.Controls.Add(this.checkBox_settings_gather_at_launch);
            this.groupBox_settings_startup.Controls.Add(this.checkBox_settings_launch_minimized);
            this.groupBox_settings_startup.Controls.Add(this.checkBox_settings_autostart);
            this.groupBox_settings_startup.Location = new System.Drawing.Point(12, 12);
            this.groupBox_settings_startup.Name = "groupBox_settings_startup";
            this.groupBox_settings_startup.Size = new System.Drawing.Size(325, 97);
            this.groupBox_settings_startup.TabIndex = 0;
            this.groupBox_settings_startup.TabStop = false;
            this.groupBox_settings_startup.Text = "Startup";
            // 
            // checkBox_settings_gather_at_launch
            // 
            this.checkBox_settings_gather_at_launch.AutoSize = true;
            this.checkBox_settings_gather_at_launch.Location = new System.Drawing.Point(6, 72);
            this.checkBox_settings_gather_at_launch.Name = "checkBox_settings_gather_at_launch";
            this.checkBox_settings_gather_at_launch.Size = new System.Drawing.Size(147, 19);
            this.checkBox_settings_gather_at_launch.TabIndex = 2;
            this.checkBox_settings_gather_at_launch.Text = "Gather werke at launch";
            this.checkBox_settings_gather_at_launch.UseVisualStyleBackColor = true;
            // 
            // checkBox_settings_launch_minimized
            // 
            this.checkBox_settings_launch_minimized.AutoSize = true;
            this.checkBox_settings_launch_minimized.Location = new System.Drawing.Point(6, 47);
            this.checkBox_settings_launch_minimized.Name = "checkBox_settings_launch_minimized";
            this.checkBox_settings_launch_minimized.Size = new System.Drawing.Size(195, 19);
            this.checkBox_settings_launch_minimized.TabIndex = 1;
            this.checkBox_settings_launch_minimized.Text = "Launch with minimized window";
            this.checkBox_settings_launch_minimized.UseVisualStyleBackColor = true;
            // 
            // checkBox_settings_autostart
            // 
            this.checkBox_settings_autostart.AutoSize = true;
            this.checkBox_settings_autostart.Location = new System.Drawing.Point(6, 22);
            this.checkBox_settings_autostart.Name = "checkBox_settings_autostart";
            this.checkBox_settings_autostart.Size = new System.Drawing.Size(162, 19);
            this.checkBox_settings_autostart.TabIndex = 0;
            this.checkBox_settings_autostart.Text = "Launch on system startup";
            this.checkBox_settings_autostart.UseVisualStyleBackColor = true;
            // 
            // groupBox_settings_directories
            // 
            this.groupBox_settings_directories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_settings_directories.Controls.Add(this.label_settings_dir_archive);
            this.groupBox_settings_directories.Controls.Add(this.button_settings_dir_archive_select);
            this.groupBox_settings_directories.Controls.Add(this.textBox_settings_dir_archive);
            this.groupBox_settings_directories.Controls.Add(this.label_settings_dir_cold);
            this.groupBox_settings_directories.Controls.Add(this.button_settings_dir_cold_select);
            this.groupBox_settings_directories.Controls.Add(this.textBox_settings_dir_cold);
            this.groupBox_settings_directories.Controls.Add(this.label_settings_dir_hot);
            this.groupBox_settings_directories.Controls.Add(this.button_settings_dir_hot_select);
            this.groupBox_settings_directories.Controls.Add(this.textBox_settings_dir_hot);
            this.groupBox_settings_directories.Location = new System.Drawing.Point(12, 115);
            this.groupBox_settings_directories.Name = "groupBox_settings_directories";
            this.groupBox_settings_directories.Size = new System.Drawing.Size(325, 157);
            this.groupBox_settings_directories.TabIndex = 1;
            this.groupBox_settings_directories.TabStop = false;
            this.groupBox_settings_directories.Text = "Directories";
            // 
            // label_settings_dir_archive
            // 
            this.label_settings_dir_archive.AutoSize = true;
            this.label_settings_dir_archive.Location = new System.Drawing.Point(6, 107);
            this.label_settings_dir_archive.Name = "label_settings_dir_archive";
            this.label_settings_dir_archive.Size = new System.Drawing.Size(56, 15);
            this.label_settings_dir_archive.TabIndex = 7;
            this.label_settings_dir_archive.Text = "Hot Vault";
            // 
            // button_settings_dir_archive_select
            // 
            this.button_settings_dir_archive_select.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_settings_dir_archive_select.Location = new System.Drawing.Point(244, 122);
            this.button_settings_dir_archive_select.Name = "button_settings_dir_archive_select";
            this.button_settings_dir_archive_select.Size = new System.Drawing.Size(75, 25);
            this.button_settings_dir_archive_select.TabIndex = 9;
            this.button_settings_dir_archive_select.Text = "Select";
            this.button_settings_dir_archive_select.UseVisualStyleBackColor = true;
            this.button_settings_dir_archive_select.Click += new System.EventHandler(this.ButtonSettingsDirArchiveSelectClick);
            // 
            // textBox_settings_dir_archive
            // 
            this.textBox_settings_dir_archive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_settings_dir_archive.Location = new System.Drawing.Point(6, 123);
            this.textBox_settings_dir_archive.Name = "textBox_settings_dir_archive";
            this.textBox_settings_dir_archive.Size = new System.Drawing.Size(232, 23);
            this.textBox_settings_dir_archive.TabIndex = 8;
            // 
            // label_settings_dir_cold
            // 
            this.label_settings_dir_cold.AutoSize = true;
            this.label_settings_dir_cold.Location = new System.Drawing.Point(6, 63);
            this.label_settings_dir_cold.Name = "label_settings_dir_cold";
            this.label_settings_dir_cold.Size = new System.Drawing.Size(61, 15);
            this.label_settings_dir_cold.TabIndex = 4;
            this.label_settings_dir_cold.Text = "Cold Vault";
            // 
            // button_settings_dir_cold_select
            // 
            this.button_settings_dir_cold_select.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_settings_dir_cold_select.Location = new System.Drawing.Point(244, 78);
            this.button_settings_dir_cold_select.Name = "button_settings_dir_cold_select";
            this.button_settings_dir_cold_select.Size = new System.Drawing.Size(75, 25);
            this.button_settings_dir_cold_select.TabIndex = 6;
            this.button_settings_dir_cold_select.Text = "Select";
            this.button_settings_dir_cold_select.UseVisualStyleBackColor = true;
            this.button_settings_dir_cold_select.Click += new System.EventHandler(this.ButtonSettingsDirColdSelectClick);
            // 
            // textBox_settings_dir_cold
            // 
            this.textBox_settings_dir_cold.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_settings_dir_cold.Location = new System.Drawing.Point(6, 79);
            this.textBox_settings_dir_cold.Name = "textBox_settings_dir_cold";
            this.textBox_settings_dir_cold.Size = new System.Drawing.Size(232, 23);
            this.textBox_settings_dir_cold.TabIndex = 5;
            // 
            // label_settings_dir_hot
            // 
            this.label_settings_dir_hot.AutoSize = true;
            this.label_settings_dir_hot.Location = new System.Drawing.Point(6, 19);
            this.label_settings_dir_hot.Name = "label_settings_dir_hot";
            this.label_settings_dir_hot.Size = new System.Drawing.Size(56, 15);
            this.label_settings_dir_hot.TabIndex = 1;
            this.label_settings_dir_hot.Text = "Hot Vault";
            // 
            // button_settings_dir_hot_select
            // 
            this.button_settings_dir_hot_select.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_settings_dir_hot_select.Location = new System.Drawing.Point(244, 34);
            this.button_settings_dir_hot_select.Name = "button_settings_dir_hot_select";
            this.button_settings_dir_hot_select.Size = new System.Drawing.Size(75, 25);
            this.button_settings_dir_hot_select.TabIndex = 3;
            this.button_settings_dir_hot_select.Text = "Select";
            this.button_settings_dir_hot_select.UseVisualStyleBackColor = true;
            this.button_settings_dir_hot_select.Click += new System.EventHandler(this.ButtonSettingsDirHotSelectClick);
            // 
            // textBox_settings_dir_hot
            // 
            this.textBox_settings_dir_hot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_settings_dir_hot.Location = new System.Drawing.Point(6, 35);
            this.textBox_settings_dir_hot.Name = "textBox_settings_dir_hot";
            this.textBox_settings_dir_hot.Size = new System.Drawing.Size(232, 23);
            this.textBox_settings_dir_hot.TabIndex = 2;
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_cancel.Location = new System.Drawing.Point(12, 376);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 4;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.ButtonCancelClick);
            // 
            // button_save
            // 
            this.button_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_save.Location = new System.Drawing.Point(262, 376);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(75, 23);
            this.button_save.TabIndex = 5;
            this.button_save.Text = "Save";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.ButtonSaveClick);
            // 
            // groupBox_archiving
            // 
            this.groupBox_archiving.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_archiving.Controls.Add(this.label_settings_archiving_compression_desc2);
            this.groupBox_archiving.Controls.Add(this.label_settings_archiving_compression_desc1);
            this.groupBox_archiving.Controls.Add(this.label_settings_archiving_compression);
            this.groupBox_archiving.Controls.Add(this.trackBar_settings_archiving_compression);
            this.groupBox_archiving.Location = new System.Drawing.Point(13, 277);
            this.groupBox_archiving.Name = "groupBox_archiving";
            this.groupBox_archiving.Size = new System.Drawing.Size(324, 93);
            this.groupBox_archiving.TabIndex = 3;
            this.groupBox_archiving.TabStop = false;
            this.groupBox_archiving.Text = "Archiving";
            // 
            // label_settings_archiving_compression_desc2
            // 
            this.label_settings_archiving_compression_desc2.AutoSize = true;
            this.label_settings_archiving_compression_desc2.ForeColor = System.Drawing.SystemColors.InfoText;
            this.label_settings_archiving_compression_desc2.Location = new System.Drawing.Point(206, 52);
            this.label_settings_archiving_compression_desc2.Name = "label_settings_archiving_compression_desc2";
            this.label_settings_archiving_compression_desc2.Size = new System.Drawing.Size(68, 15);
            this.label_settings_archiving_compression_desc2.TabIndex = 3;
            this.label_settings_archiving_compression_desc2.Text = "9 = Highest";
            // 
            // label_settings_archiving_compression_desc1
            // 
            this.label_settings_archiving_compression_desc1.AutoSize = true;
            this.label_settings_archiving_compression_desc1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.label_settings_archiving_compression_desc1.Location = new System.Drawing.Point(206, 37);
            this.label_settings_archiving_compression_desc1.Name = "label_settings_archiving_compression_desc1";
            this.label_settings_archiving_compression_desc1.Size = new System.Drawing.Size(64, 15);
            this.label_settings_archiving_compression_desc1.TabIndex = 2;
            this.label_settings_archiving_compression_desc1.Text = "1 = Lowest";
            // 
            // label_settings_archiving_compression
            // 
            this.label_settings_archiving_compression.AutoSize = true;
            this.label_settings_archiving_compression.Location = new System.Drawing.Point(6, 19);
            this.label_settings_archiving_compression.Name = "label_settings_archiving_compression";
            this.label_settings_archiving_compression.Size = new System.Drawing.Size(119, 15);
            this.label_settings_archiving_compression.TabIndex = 0;
            this.label_settings_archiving_compression.Text = "Compression Level: 9";
            // 
            // trackBar_settings_archiving_compression
            // 
            this.trackBar_settings_archiving_compression.Location = new System.Drawing.Point(6, 37);
            this.trackBar_settings_archiving_compression.Maximum = 9;
            this.trackBar_settings_archiving_compression.Minimum = 1;
            this.trackBar_settings_archiving_compression.Name = "trackBar_settings_archiving_compression";
            this.trackBar_settings_archiving_compression.Size = new System.Drawing.Size(194, 45);
            this.trackBar_settings_archiving_compression.TabIndex = 1;
            this.trackBar_settings_archiving_compression.Value = 9;
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 411);
            this.Controls.Add(this.groupBox_archiving);
            this.Controls.Add(this.button_save);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.groupBox_settings_directories);
            this.Controls.Add(this.groupBox_settings_startup);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(360, 441);
            this.Name = "FormSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Werkbank: Settings";
            this.groupBox_settings_startup.ResumeLayout(false);
            this.groupBox_settings_startup.PerformLayout();
            this.groupBox_settings_directories.ResumeLayout(false);
            this.groupBox_settings_directories.PerformLayout();
            this.groupBox_archiving.ResumeLayout(false);
            this.groupBox_archiving.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_settings_archiving_compression)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox_settings_startup;
        private CheckBox checkBox_settings_autostart;
        private CheckBox checkBox_settings_gather_at_launch;
        private CheckBox checkBox_settings_launch_minimized;
        private GroupBox groupBox_settings_directories;
        private Button button_settings_dir_hot_select;
        private TextBox textBox_settings_dir_hot;
        private Label label_settings_dir_hot;
        private Button button_cancel;
        private Button button_save;
        private Label label_settings_dir_archive;
        private Button button_settings_dir_archive_select;
        private TextBox textBox_settings_dir_archive;
        private Label label_settings_dir_cold;
        private Button button_settings_dir_cold_select;
        private TextBox textBox_settings_dir_cold;
        private GroupBox groupBox_archiving;
        private Label label_settings_archiving_compression;
        private TrackBar trackBar_settings_archiving_compression;
        private Label label_settings_archiving_compression_desc2;
        private Label label_settings_archiving_compression_desc1;
        private FolderBrowserDialog folderBrowserDialog;
    }
}