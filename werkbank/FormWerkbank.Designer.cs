using werkbank.models;

namespace werkbank
{
    partial class FormWerkbank
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWerkbank));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel_controls = new System.Windows.Forms.Panel();
            this.button_werk_history = new System.Windows.Forms.Button();
            this.button_create_werk = new System.Windows.Forms.Button();
            this.button_werk_web = new System.Windows.Forms.Button();
            this.label_version = new System.Windows.Forms.Label();
            this.button_refresh = new System.Windows.Forms.Button();
            this.button_werk_open = new System.Windows.Forms.Button();
            this.button_werk_backup = new System.Windows.Forms.Button();
            this.button_werk_down = new System.Windows.Forms.Button();
            this.button_werk_up = new System.Windows.Forms.Button();
            this.button_werk_vscode = new System.Windows.Forms.Button();
            this.button_werk_edit = new System.Windows.Forms.Button();
            this.button_settings = new System.Windows.Forms.Button();
            this.panel_queue = new System.Windows.Forms.Panel();
            this.pictureBox_serach = new System.Windows.Forms.PictureBox();
            this.textBox_search = new System.Windows.Forms.TextBox();
            this.progressBar_queue = new System.Windows.Forms.ProgressBar();
            this.button_queue = new System.Windows.Forms.Button();
            this.timerQueue = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.SuspendLayout();
            this.panel_controls.SuspendLayout();
            this.panel_queue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_serach)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.RosyBrown;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(985, 675);
            this.splitContainer1.SplitterDistance = 258;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.Color.LightBlue;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.Color.LightGray;
            this.splitContainer2.Size = new System.Drawing.Size(985, 413);
            this.splitContainer2.SplitterDistance = 205;
            this.splitContainer2.TabIndex = 0;
            // 
            // panel_controls
            // 
            this.panel_controls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_controls.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel_controls.Controls.Add(this.button_werk_history);
            this.panel_controls.Controls.Add(this.button_create_werk);
            this.panel_controls.Controls.Add(this.button_werk_web);
            this.panel_controls.Controls.Add(this.label_version);
            this.panel_controls.Controls.Add(this.button_refresh);
            this.panel_controls.Controls.Add(this.button_werk_open);
            this.panel_controls.Controls.Add(this.button_werk_backup);
            this.panel_controls.Controls.Add(this.button_werk_down);
            this.panel_controls.Controls.Add(this.button_werk_up);
            this.panel_controls.Controls.Add(this.button_werk_vscode);
            this.panel_controls.Controls.Add(this.button_werk_edit);
            this.panel_controls.Location = new System.Drawing.Point(985, 0);
            this.panel_controls.Name = "panel_controls";
            this.panel_controls.Size = new System.Drawing.Size(72, 715);
            this.panel_controls.TabIndex = 4;
            // 
            // button_werk_history
            // 
            this.button_werk_history.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_werk_history.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_werk_history.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button_werk_history.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.button_werk_history.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_werk_history.Image = global::werkbank.Properties.Resources.btn_history;
            this.button_werk_history.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button_werk_history.Location = new System.Drawing.Point(4, 204);
            this.button_werk_history.Name = "button_werk_history";
            this.button_werk_history.Size = new System.Drawing.Size(64, 61);
            this.button_werk_history.TabIndex = 6;
            this.button_werk_history.Text = "History";
            this.button_werk_history.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button_werk_history.UseVisualStyleBackColor = false;
            this.button_werk_history.Click += new System.EventHandler(this.ButtonHistoryClick);
            // 
            // button_create_werk
            // 
            this.button_create_werk.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_create_werk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_create_werk.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button_create_werk.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.button_create_werk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_create_werk.Image = global::werkbank.Properties.Resources.btn_create;
            this.button_create_werk.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button_create_werk.Location = new System.Drawing.Point(4, 70);
            this.button_create_werk.Name = "button_create_werk";
            this.button_create_werk.Size = new System.Drawing.Size(64, 61);
            this.button_create_werk.TabIndex = 4;
            this.button_create_werk.Text = "Create";
            this.button_create_werk.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button_create_werk.UseVisualStyleBackColor = false;
            this.button_create_werk.Click += new System.EventHandler(this.ButtonCreateWerkClick);
            // 
            // button_werk_web
            // 
            this.button_werk_web.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_werk_web.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_werk_web.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button_werk_web.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.button_werk_web.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_werk_web.Image = global::werkbank.Properties.Resources.btn_web;
            this.button_werk_web.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button_werk_web.Location = new System.Drawing.Point(4, 632);
            this.button_werk_web.Name = "button_werk_web";
            this.button_werk_web.Size = new System.Drawing.Size(64, 61);
            this.button_werk_web.TabIndex = 12;
            this.button_werk_web.Text = "Web";
            this.button_werk_web.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button_werk_web.UseVisualStyleBackColor = false;
            this.button_werk_web.Click += new System.EventHandler(this.ButtonWerkWebClick);
            // 
            // label_version
            // 
            this.label_version.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label_version.AutoSize = true;
            this.label_version.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label_version.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label_version.Location = new System.Drawing.Point(41, 702);
            this.label_version.Name = "label_version";
            this.label_version.Size = new System.Drawing.Size(28, 12);
            this.label_version.TabIndex = 2;
            this.label_version.Text = "v0.0.0";
            // 
            // button_refresh
            // 
            this.button_refresh.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_refresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_refresh.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button_refresh.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.button_refresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_refresh.Image = global::werkbank.Properties.Resources.btn_refresh;
            this.button_refresh.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button_refresh.Location = new System.Drawing.Point(4, 3);
            this.button_refresh.Name = "button_refresh";
            this.button_refresh.Size = new System.Drawing.Size(64, 61);
            this.button_refresh.TabIndex = 3;
            this.button_refresh.Text = "Refresh";
            this.button_refresh.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button_refresh.UseVisualStyleBackColor = false;
            this.button_refresh.Click += new System.EventHandler(this.ButtonRefreshClick);
            // 
            // button_werk_open
            // 
            this.button_werk_open.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_werk_open.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_werk_open.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button_werk_open.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.button_werk_open.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_werk_open.Image = global::werkbank.Properties.Resources.btn_open;
            this.button_werk_open.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button_werk_open.Location = new System.Drawing.Point(4, 498);
            this.button_werk_open.Name = "button_werk_open";
            this.button_werk_open.Size = new System.Drawing.Size(64, 61);
            this.button_werk_open.TabIndex = 10;
            this.button_werk_open.Text = "Open";
            this.button_werk_open.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button_werk_open.UseVisualStyleBackColor = false;
            this.button_werk_open.Click += new System.EventHandler(this.ButtonWerkOpenClick);
            // 
            // button_werk_backup
            // 
            this.button_werk_backup.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_werk_backup.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_werk_backup.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button_werk_backup.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.button_werk_backup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_werk_backup.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button_werk_backup.Image = global::werkbank.Properties.Resources.btn_backup;
            this.button_werk_backup.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button_werk_backup.Location = new System.Drawing.Point(4, 418);
            this.button_werk_backup.Name = "button_werk_backup";
            this.button_werk_backup.Size = new System.Drawing.Size(64, 61);
            this.button_werk_backup.TabIndex = 9;
            this.button_werk_backup.Text = "Backup";
            this.button_werk_backup.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button_werk_backup.UseVisualStyleBackColor = false;
            this.button_werk_backup.Click += new System.EventHandler(this.ButtonWerkBackupClick);
            // 
            // button_werk_down
            // 
            this.button_werk_down.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_werk_down.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_werk_down.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button_werk_down.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.button_werk_down.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_werk_down.Image = global::werkbank.Properties.Resources.btn_down;
            this.button_werk_down.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button_werk_down.Location = new System.Drawing.Point(4, 351);
            this.button_werk_down.Name = "button_werk_down";
            this.button_werk_down.Size = new System.Drawing.Size(64, 61);
            this.button_werk_down.TabIndex = 8;
            this.button_werk_down.Text = "Down";
            this.button_werk_down.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button_werk_down.UseVisualStyleBackColor = false;
            this.button_werk_down.Click += new System.EventHandler(this.ButtonWerkDownClick);
            // 
            // button_werk_up
            // 
            this.button_werk_up.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_werk_up.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_werk_up.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button_werk_up.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.button_werk_up.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_werk_up.Image = global::werkbank.Properties.Resources.btn_up;
            this.button_werk_up.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button_werk_up.Location = new System.Drawing.Point(4, 284);
            this.button_werk_up.Name = "button_werk_up";
            this.button_werk_up.Size = new System.Drawing.Size(64, 61);
            this.button_werk_up.TabIndex = 7;
            this.button_werk_up.Text = "Up";
            this.button_werk_up.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button_werk_up.UseVisualStyleBackColor = false;
            this.button_werk_up.Click += new System.EventHandler(this.ButtonWerkUpClick);
            // 
            // button_werk_vscode
            // 
            this.button_werk_vscode.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_werk_vscode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_werk_vscode.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button_werk_vscode.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.button_werk_vscode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_werk_vscode.Image = global::werkbank.Properties.Resources.btn_vscode;
            this.button_werk_vscode.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button_werk_vscode.Location = new System.Drawing.Point(4, 565);
            this.button_werk_vscode.Name = "button_werk_vscode";
            this.button_werk_vscode.Size = new System.Drawing.Size(64, 61);
            this.button_werk_vscode.TabIndex = 11;
            this.button_werk_vscode.Text = "VS Code";
            this.button_werk_vscode.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button_werk_vscode.UseVisualStyleBackColor = false;
            this.button_werk_vscode.Click += new System.EventHandler(this.ButtonWerkVsCodeClick);
            // 
            // button_werk_edit
            // 
            this.button_werk_edit.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_werk_edit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_werk_edit.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button_werk_edit.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.button_werk_edit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_werk_edit.Image = global::werkbank.Properties.Resources.btn_edit;
            this.button_werk_edit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button_werk_edit.Location = new System.Drawing.Point(4, 137);
            this.button_werk_edit.Name = "button_werk_edit";
            this.button_werk_edit.Size = new System.Drawing.Size(64, 61);
            this.button_werk_edit.TabIndex = 5;
            this.button_werk_edit.Text = "Edit";
            this.button_werk_edit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button_werk_edit.UseVisualStyleBackColor = false;
            this.button_werk_edit.Click += new System.EventHandler(this.ButtonWerkEditClick);
            // 
            // button_settings
            // 
            this.button_settings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_settings.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_settings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_settings.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button_settings.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.button_settings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_settings.Image = global::werkbank.Properties.Resources.btn_settings;
            this.button_settings.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_settings.Location = new System.Drawing.Point(3, 4);
            this.button_settings.Name = "button_settings";
            this.button_settings.Size = new System.Drawing.Size(83, 34);
            this.button_settings.TabIndex = 2;
            this.button_settings.Text = "Settings";
            this.button_settings.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_settings.UseVisualStyleBackColor = false;
            this.button_settings.Click += new System.EventHandler(this.ButtonSettingsClick);
            // 
            // panel_queue
            // 
            this.panel_queue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_queue.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel_queue.Controls.Add(this.pictureBox_serach);
            this.panel_queue.Controls.Add(this.textBox_search);
            this.panel_queue.Controls.Add(this.progressBar_queue);
            this.panel_queue.Controls.Add(this.button_queue);
            this.panel_queue.Controls.Add(this.button_settings);
            this.panel_queue.Location = new System.Drawing.Point(0, 675);
            this.panel_queue.Name = "panel_queue";
            this.panel_queue.Size = new System.Drawing.Size(985, 40);
            this.panel_queue.TabIndex = 5;
            // 
            // pictureBox_serach
            // 
            this.pictureBox_serach.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_serach.Image = global::werkbank.Properties.Resources.edit_search;
            this.pictureBox_serach.Location = new System.Drawing.Point(957, 8);
            this.pictureBox_serach.Name = "pictureBox_serach";
            this.pictureBox_serach.Size = new System.Drawing.Size(25, 25);
            this.pictureBox_serach.TabIndex = 5;
            this.pictureBox_serach.TabStop = false;
            // 
            // textBox_search
            // 
            this.textBox_search.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_search.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_search.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox_search.Location = new System.Drawing.Point(678, 10);
            this.textBox_search.Name = "textBox_search";
            this.textBox_search.Size = new System.Drawing.Size(273, 23);
            this.textBox_search.TabIndex = 0;
            this.textBox_search.TextChanged += new System.EventHandler(this.SearchTextChanged);
            // 
            // progressBar_queue
            // 
            this.progressBar_queue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.progressBar_queue.Location = new System.Drawing.Point(181, 8);
            this.progressBar_queue.Name = "progressBar_queue";
            this.progressBar_queue.Size = new System.Drawing.Size(206, 25);
            this.progressBar_queue.TabIndex = 4;
            // 
            // button_queue
            // 
            this.button_queue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_queue.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_queue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_queue.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button_queue.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.button_queue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_queue.Image = global::werkbank.Properties.Resources.btn_queue;
            this.button_queue.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_queue.Location = new System.Drawing.Point(92, 4);
            this.button_queue.Name = "button_queue";
            this.button_queue.Size = new System.Drawing.Size(83, 34);
            this.button_queue.TabIndex = 3;
            this.button_queue.Text = "Queue";
            this.button_queue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_queue.UseVisualStyleBackColor = false;
            this.button_queue.Click += new System.EventHandler(this.ButtonQueueClick);
            // 
            // timerQueue
            // 
            this.timerQueue.Interval = 1000;
            this.timerQueue.Tick += new System.EventHandler(this.TimerQueueTick);
            // 
            // FormWerkbank
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 715);
            this.Controls.Add(this.panel_queue);
            this.Controls.Add(this.panel_controls);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1073, 754);
            this.Name = "FormWerkbank";
            this.Text = "Werkbank";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormWerkbankClosing);
            this.Load += new System.EventHandler(this.FormWerkbankLoad);
            this.Shown += new System.EventHandler(this.FormWerkbankShown);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel_controls.ResumeLayout(false);
            this.panel_controls.PerformLayout();
            this.panel_queue.ResumeLayout(false);
            this.panel_queue.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_serach)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private Panel panel_controls;
        private Button button_werk_edit;
        private Button button_werk_vscode;
        private Label label_version;
        private Button button_werk_up;
        private Button button_werk_backup;
        private Button button_werk_down;
        private Button button_werk_open;
        private Button button_settings;
        private Button button_refresh;
        private Button button_werk_web;
        private Panel panel_queue;
        private System.Windows.Forms.Timer timerQueue;
        private Button button_queue;
        private ProgressBar progressBar_queue;
        private Button button_create_werk;
        private Button button_werk_history;
        private TextBox textBox_search;
        private PictureBox pictureBox_serach;
    }
}