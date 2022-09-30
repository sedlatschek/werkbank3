namespace installer
{
    partial class FormInstaller
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInstaller));
            this.textBox_info = new System.Windows.Forms.TextBox();
            this.button_continue = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.panel_controls = new System.Windows.Forms.Panel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.worker = new System.ComponentModel.BackgroundWorker();
            this.panel_controls.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_info
            // 
            this.textBox_info.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_info.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_info.Location = new System.Drawing.Point(12, 25);
            this.textBox_info.Multiline = true;
            this.textBox_info.Name = "textBox_info";
            this.textBox_info.ReadOnly = true;
            this.textBox_info.Size = new System.Drawing.Size(384, 70);
            this.textBox_info.TabIndex = 0;
            this.textBox_info.TabStop = false;
            this.textBox_info.Text = "This installer will install werkbank version {{version}} for {{user}}.\r\nExisting " +
    "installations may be replaced.\r\n\r\nDo you want to continue?";
            // 
            // button_continue
            // 
            this.button_continue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_continue.Location = new System.Drawing.Point(321, 14);
            this.button_continue.Name = "button_continue";
            this.button_continue.Size = new System.Drawing.Size(75, 23);
            this.button_continue.TabIndex = 1;
            this.button_continue.Text = "Continue";
            this.button_continue.UseVisualStyleBackColor = true;
            this.button_continue.Click += new System.EventHandler(this.ButtonContinueClick);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_cancel.Location = new System.Drawing.Point(12, 14);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 2;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.ButtonCancelClick);
            // 
            // panel_controls
            // 
            this.panel_controls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_controls.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel_controls.Controls.Add(this.button_cancel);
            this.panel_controls.Controls.Add(this.button_continue);
            this.panel_controls.Location = new System.Drawing.Point(0, 149);
            this.panel_controls.Name = "panel_controls";
            this.panel_controls.Size = new System.Drawing.Size(407, 49);
            this.panel_controls.TabIndex = 3;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Enabled = false;
            this.progressBar.Location = new System.Drawing.Point(12, 111);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(384, 23);
            this.progressBar.TabIndex = 4;
            // 
            // worker
            // 
            this.worker.WorkerReportsProgress = true;
            this.worker.WorkerSupportsCancellation = true;
            this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Install);
            this.worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.InstallCompleted);
            // 
            // FormInstaller
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 198);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.panel_controls);
            this.Controls.Add(this.textBox_info);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(424, 237);
            this.Name = "FormInstaller";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Werkbank: Installer";
            this.panel_controls.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox textBox_info;
        private Button button_continue;
        private Button button_cancel;
        private Panel panel_controls;
        private ProgressBar progressBar;
        private System.ComponentModel.BackgroundWorker worker;
    }
}