namespace werkbank
{
    partial class FormQueue
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
            this.panel_objectListView = new System.Windows.Forms.Panel();
            this.worker = new System.ComponentModel.BackgroundWorker();
            this.button_operation_reset = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panel_objectListView
            // 
            this.panel_objectListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_objectListView.Location = new System.Drawing.Point(12, 12);
            this.panel_objectListView.Name = "panel_objectListView";
            this.panel_objectListView.Size = new System.Drawing.Size(776, 386);
            this.panel_objectListView.TabIndex = 0;
            // 
            // worker
            // 
            this.worker.WorkerReportsProgress = true;
            this.worker.WorkerSupportsCancellation = true;
            this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.OnWork);
            this.worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.OnWorkProgress);
            this.worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.OnWorkCompleted);
            // 
            // button_operation_reset
            // 
            this.button_operation_reset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_operation_reset.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_operation_reset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_operation_reset.Enabled = false;
            this.button_operation_reset.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button_operation_reset.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.button_operation_reset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_operation_reset.Image = global::werkbank.Properties.Resources.btn_reset;
            this.button_operation_reset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_operation_reset.Location = new System.Drawing.Point(716, 404);
            this.button_operation_reset.Name = "button_operation_reset";
            this.button_operation_reset.Size = new System.Drawing.Size(72, 34);
            this.button_operation_reset.TabIndex = 1;
            this.button_operation_reset.Text = "Reset";
            this.button_operation_reset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_operation_reset.UseVisualStyleBackColor = false;
            this.button_operation_reset.Click += new System.EventHandler(this.ButtonOperationResetClick);
            // 
            // FormQueue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_operation_reset);
            this.Controls.Add(this.panel_objectListView);
            this.Name = "FormQueue";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Werkbank: Queue";
            this.Shown += new System.EventHandler(this.FormQueueShown);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel_objectListView;
        private System.ComponentModel.BackgroundWorker worker;
        private Button button_operation_reset;
    }
}