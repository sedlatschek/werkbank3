﻿namespace werkbank.controls
{
    partial class WerkList
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.objectListView = new BrightIdeasSoftware.ObjectListView();
            this.worker = new System.ComponentModel.BackgroundWorker();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label_progress = new System.Windows.Forms.Label();
            this.panel_loading = new System.Windows.Forms.Panel();
            this.timer_hide_loading = new System.Windows.Forms.Timer(this.components);
            this.pictureBox_vault_icon = new System.Windows.Forms.PictureBox();
            this.rotatingLabel_title = new werkbank.controls.RotatingLabel();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView)).BeginInit();
            this.panel_loading.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_vault_icon)).BeginInit();
            this.SuspendLayout();
            // 
            // objectListView
            // 
            this.objectListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.objectListView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.objectListView.FullRowSelect = true;
            this.objectListView.Location = new System.Drawing.Point(40, 3);
            this.objectListView.MultiSelect = false;
            this.objectListView.Name = "objectListView";
            this.objectListView.Size = new System.Drawing.Size(663, 383);
            this.objectListView.TabIndex = 0;
            this.objectListView.View = System.Windows.Forms.View.Details;
            this.objectListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ObjectListViewItemSelectionChanged);
            this.objectListView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ObjectListViewKeyUp);
            this.objectListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ObjectListViewMouseDoubleClick);
            // 
            // worker
            // 
            this.worker.WorkerReportsProgress = true;
            this.worker.WorkerSupportsCancellation = true;
            this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.OnGather);
            this.worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.OnGatherProgressChanged);
            this.worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.OnGatherCompleted);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(22, 34);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(369, 24);
            this.progressBar.TabIndex = 1;
            // 
            // label_progress
            // 
            this.label_progress.AutoSize = true;
            this.label_progress.Location = new System.Drawing.Point(20, 14);
            this.label_progress.Name = "label_progress";
            this.label_progress.Size = new System.Drawing.Size(23, 15);
            this.label_progress.TabIndex = 2;
            this.label_progress.Text = "0%";
            // 
            // panel_loading
            // 
            this.panel_loading.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel_loading.BackColor = System.Drawing.SystemColors.Window;
            this.panel_loading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_loading.Controls.Add(this.progressBar);
            this.panel_loading.Controls.Add(this.label_progress);
            this.panel_loading.Location = new System.Drawing.Point(147, 163);
            this.panel_loading.Name = "panel_loading";
            this.panel_loading.Size = new System.Drawing.Size(417, 76);
            this.panel_loading.TabIndex = 3;
            this.panel_loading.Visible = false;
            // 
            // timer_hide_loading
            // 
            this.timer_hide_loading.Interval = 1000;
            this.timer_hide_loading.Tick += new System.EventHandler(this.TimerHideLoadingTick);
            // 
            // pictureBox_vault_icon
            // 
            this.pictureBox_vault_icon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox_vault_icon.Image = global::werkbank.Properties.Resources.vault_hot;
            this.pictureBox_vault_icon.Location = new System.Drawing.Point(3, 354);
            this.pictureBox_vault_icon.Name = "pictureBox_vault_icon";
            this.pictureBox_vault_icon.Size = new System.Drawing.Size(32, 32);
            this.pictureBox_vault_icon.TabIndex = 4;
            this.pictureBox_vault_icon.TabStop = false;
            // 
            // rotatingLabel_title
            // 
            this.rotatingLabel_title.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.rotatingLabel_title.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.rotatingLabel_title.Location = new System.Drawing.Point(5, 0);
            this.rotatingLabel_title.Name = "rotatingLabel_title";
            this.rotatingLabel_title.NewText = "Hot Vault";
            this.rotatingLabel_title.RotateAngle = -90;
            this.rotatingLabel_title.Size = new System.Drawing.Size(30, 97);
            this.rotatingLabel_title.TabIndex = 6;
            // 
            // WerkList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_loading);
            this.Controls.Add(this.rotatingLabel_title);
            this.Controls.Add(this.pictureBox_vault_icon);
            this.Controls.Add(this.objectListView);
            this.Name = "WerkList";
            this.Size = new System.Drawing.Size(706, 389);
            this.SizeChanged += new System.EventHandler(this.WerkListSizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.objectListView)).EndInit();
            this.panel_loading.ResumeLayout(false);
            this.panel_loading.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_vault_icon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView objectListView;
        private System.ComponentModel.BackgroundWorker worker;
        private ProgressBar progressBar;
        private Label label_progress;
        private Panel panel_loading;
        private System.Windows.Forms.Timer timer_hide_loading;
        private PictureBox pictureBox_vault_icon;
        private RotatingLabel rotatingLabel_title;
    }
}
