namespace werkbank.controls
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
            this.objectListView = new BrightIdeasSoftware.ObjectListView();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView)).BeginInit();
            this.SuspendLayout();
            // 
            // objectListView
            // 
            this.objectListView.Location = new System.Drawing.Point(76, 80);
            this.objectListView.Name = "objectListView";
            this.objectListView.Size = new System.Drawing.Size(121, 97);
            this.objectListView.TabIndex = 0;
            this.objectListView.View = System.Windows.Forms.View.Details;
            // 
            // WerkList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.objectListView);
            this.Name = "WerkList";
            this.Size = new System.Drawing.Size(706, 389);
            ((System.ComponentModel.ISupportInitialize)(this.objectListView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView objectListView;
    }
}
