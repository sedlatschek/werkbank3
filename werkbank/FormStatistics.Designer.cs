namespace werkbank
{
    partial class FormStatistics
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStatistics));
            this.groupBox_environments = new System.Windows.Forms.GroupBox();
            this.panel_environments = new System.Windows.Forms.Panel();
            this.groupBox_environments.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_environments
            // 
            this.groupBox_environments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_environments.Controls.Add(this.panel_environments);
            this.groupBox_environments.Location = new System.Drawing.Point(12, 12);
            this.groupBox_environments.Name = "groupBox_environments";
            this.groupBox_environments.Size = new System.Drawing.Size(409, 455);
            this.groupBox_environments.TabIndex = 0;
            this.groupBox_environments.TabStop = false;
            this.groupBox_environments.Text = "Environments";
            // 
            // panel_environments
            // 
            this.panel_environments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_environments.Location = new System.Drawing.Point(6, 22);
            this.panel_environments.Name = "panel_environments";
            this.panel_environments.Size = new System.Drawing.Size(397, 427);
            this.panel_environments.TabIndex = 0;
            // 
            // FormStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 479);
            this.Controls.Add(this.groupBox_environments);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(449, 518);
            this.Name = "FormStatistics";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Werkbank: Statistics";
            this.Load += new System.EventHandler(this.FormStatisticsLoad);
            this.groupBox_environments.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox_environments;
        private Panel panel_environments;
    }
}