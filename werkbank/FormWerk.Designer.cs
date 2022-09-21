namespace werkbank
{
    partial class FormWerk
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWerk));
            this.groupBox_werk = new System.Windows.Forms.GroupBox();
            this.groupBox_werk_icon = new System.Windows.Forms.GroupBox();
            this.panel_werk_icon = new System.Windows.Forms.Panel();
            this.pictureBox_werk_icon = new System.Windows.Forms.PictureBox();
            this.button_werk_select_image = new System.Windows.Forms.Button();
            this.label_werk_id = new System.Windows.Forms.Label();
            this.textBox_werk_id = new System.Windows.Forms.TextBox();
            this.label_werk_description = new System.Windows.Forms.Label();
            this.textBox_werk_description = new System.Windows.Forms.TextBox();
            this.checkBox_werk_compressOnArchive = new System.Windows.Forms.CheckBox();
            this.button_werk_name = new System.Windows.Forms.Button();
            this.label_werk_environment = new System.Windows.Forms.Label();
            this.label_werk_created = new System.Windows.Forms.Label();
            this.label_werk_name = new System.Windows.Forms.Label();
            this.label_werk_title = new System.Windows.Forms.Label();
            this.comboBox_werk_environment = new System.Windows.Forms.ComboBox();
            this.dateTimePicker_werk_created = new System.Windows.Forms.DateTimePicker();
            this.textBox_werk_name = new System.Windows.Forms.TextBox();
            this.textBox_werk_title = new System.Windows.Forms.TextBox();
            this.button_save = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox_werk.SuspendLayout();
            this.groupBox_werk_icon.SuspendLayout();
            this.panel_werk_icon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_werk_icon)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox_werk
            // 
            this.groupBox_werk.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_werk.Controls.Add(this.groupBox_werk_icon);
            this.groupBox_werk.Controls.Add(this.label_werk_id);
            this.groupBox_werk.Controls.Add(this.textBox_werk_id);
            this.groupBox_werk.Controls.Add(this.label_werk_description);
            this.groupBox_werk.Controls.Add(this.textBox_werk_description);
            this.groupBox_werk.Controls.Add(this.checkBox_werk_compressOnArchive);
            this.groupBox_werk.Controls.Add(this.button_werk_name);
            this.groupBox_werk.Controls.Add(this.label_werk_environment);
            this.groupBox_werk.Controls.Add(this.label_werk_created);
            this.groupBox_werk.Controls.Add(this.label_werk_name);
            this.groupBox_werk.Controls.Add(this.label_werk_title);
            this.groupBox_werk.Controls.Add(this.comboBox_werk_environment);
            this.groupBox_werk.Controls.Add(this.dateTimePicker_werk_created);
            this.groupBox_werk.Controls.Add(this.textBox_werk_name);
            this.groupBox_werk.Controls.Add(this.textBox_werk_title);
            this.groupBox_werk.Location = new System.Drawing.Point(12, 12);
            this.groupBox_werk.Name = "groupBox_werk";
            this.groupBox_werk.Size = new System.Drawing.Size(287, 508);
            this.groupBox_werk.TabIndex = 0;
            this.groupBox_werk.TabStop = false;
            this.groupBox_werk.Text = "Werk";
            // 
            // groupBox_werk_icon
            // 
            this.groupBox_werk_icon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_werk_icon.Controls.Add(this.panel_werk_icon);
            this.groupBox_werk_icon.Controls.Add(this.button_werk_select_image);
            this.groupBox_werk_icon.Location = new System.Drawing.Point(6, 375);
            this.groupBox_werk_icon.Name = "groupBox_werk_icon";
            this.groupBox_werk_icon.Size = new System.Drawing.Size(274, 127);
            this.groupBox_werk_icon.TabIndex = 14;
            this.groupBox_werk_icon.TabStop = false;
            this.groupBox_werk_icon.Text = "Icon";
            // 
            // panel_werk_icon
            // 
            this.panel_werk_icon.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel_werk_icon.Controls.Add(this.pictureBox_werk_icon);
            this.panel_werk_icon.Location = new System.Drawing.Point(6, 18);
            this.panel_werk_icon.Name = "panel_werk_icon";
            this.panel_werk_icon.Size = new System.Drawing.Size(102, 102);
            this.panel_werk_icon.TabIndex = 0;
            // 
            // pictureBox_werk_icon
            // 
            this.pictureBox_werk_icon.BackgroundImage = global::werkbank.Properties.Resources.icon_blank;
            this.pictureBox_werk_icon.Location = new System.Drawing.Point(1, 1);
            this.pictureBox_werk_icon.Name = "pictureBox_werk_icon";
            this.pictureBox_werk_icon.Size = new System.Drawing.Size(100, 100);
            this.pictureBox_werk_icon.TabIndex = 0;
            this.pictureBox_werk_icon.TabStop = false;
            // 
            // button_werk_select_image
            // 
            this.button_werk_select_image.Location = new System.Drawing.Point(110, 18);
            this.button_werk_select_image.Name = "button_werk_select_image";
            this.button_werk_select_image.Size = new System.Drawing.Size(107, 25);
            this.button_werk_select_image.TabIndex = 1;
            this.button_werk_select_image.Text = "Select Image";
            this.button_werk_select_image.UseVisualStyleBackColor = true;
            this.button_werk_select_image.Click += new System.EventHandler(this.ButtonWerkSelectImageClick);
            // 
            // label_werk_id
            // 
            this.label_werk_id.AutoSize = true;
            this.label_werk_id.Location = new System.Drawing.Point(6, 19);
            this.label_werk_id.Name = "label_werk_id";
            this.label_werk_id.Size = new System.Drawing.Size(18, 15);
            this.label_werk_id.TabIndex = 1;
            this.label_werk_id.Text = "ID";
            // 
            // textBox_werk_id
            // 
            this.textBox_werk_id.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_werk_id.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox_werk_id.Location = new System.Drawing.Point(6, 37);
            this.textBox_werk_id.Name = "textBox_werk_id";
            this.textBox_werk_id.ReadOnly = true;
            this.textBox_werk_id.Size = new System.Drawing.Size(275, 22);
            this.textBox_werk_id.TabIndex = 2;
            this.textBox_werk_id.TextChanged += new System.EventHandler(this.TextBoxTextChanged);
            // 
            // label_werk_description
            // 
            this.label_werk_description.AutoSize = true;
            this.label_werk_description.Location = new System.Drawing.Point(6, 243);
            this.label_werk_description.Name = "label_werk_description";
            this.label_werk_description.Size = new System.Drawing.Size(67, 15);
            this.label_werk_description.TabIndex = 11;
            this.label_werk_description.Text = "Description";
            // 
            // textBox_werk_description
            // 
            this.textBox_werk_description.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_werk_description.Location = new System.Drawing.Point(6, 261);
            this.textBox_werk_description.Multiline = true;
            this.textBox_werk_description.Name = "textBox_werk_description";
            this.textBox_werk_description.Size = new System.Drawing.Size(274, 88);
            this.textBox_werk_description.TabIndex = 12;
            // 
            // checkBox_werk_compressOnArchive
            // 
            this.checkBox_werk_compressOnArchive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_werk_compressOnArchive.AutoSize = true;
            this.checkBox_werk_compressOnArchive.Checked = true;
            this.checkBox_werk_compressOnArchive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_werk_compressOnArchive.Location = new System.Drawing.Point(6, 355);
            this.checkBox_werk_compressOnArchive.Name = "checkBox_werk_compressOnArchive";
            this.checkBox_werk_compressOnArchive.Size = new System.Drawing.Size(141, 19);
            this.checkBox_werk_compressOnArchive.TabIndex = 13;
            this.checkBox_werk_compressOnArchive.Text = "Compress On Archive";
            this.checkBox_werk_compressOnArchive.UseVisualStyleBackColor = true;
            // 
            // button_werk_name
            // 
            this.button_werk_name.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_werk_name.Location = new System.Drawing.Point(206, 125);
            this.button_werk_name.Name = "button_werk_name";
            this.button_werk_name.Size = new System.Drawing.Size(75, 25);
            this.button_werk_name.TabIndex = 7;
            this.button_werk_name.Text = "Generate";
            this.button_werk_name.UseVisualStyleBackColor = true;
            this.button_werk_name.Click += new System.EventHandler(this.ButtonWerkNameClick);
            // 
            // label_werk_environment
            // 
            this.label_werk_environment.AutoSize = true;
            this.label_werk_environment.Location = new System.Drawing.Point(6, 199);
            this.label_werk_environment.Name = "label_werk_environment";
            this.label_werk_environment.Size = new System.Drawing.Size(75, 15);
            this.label_werk_environment.TabIndex = 9;
            this.label_werk_environment.Text = "Environment";
            // 
            // label_werk_created
            // 
            this.label_werk_created.AutoSize = true;
            this.label_werk_created.Location = new System.Drawing.Point(6, 154);
            this.label_werk_created.Name = "label_werk_created";
            this.label_werk_created.Size = new System.Drawing.Size(63, 15);
            this.label_werk_created.TabIndex = 6;
            this.label_werk_created.Text = "Created At";
            // 
            // label_werk_name
            // 
            this.label_werk_name.AutoSize = true;
            this.label_werk_name.Location = new System.Drawing.Point(6, 109);
            this.label_werk_name.Name = "label_werk_name";
            this.label_werk_name.Size = new System.Drawing.Size(39, 15);
            this.label_werk_name.TabIndex = 5;
            this.label_werk_name.Text = "Name";
            // 
            // label_werk_title
            // 
            this.label_werk_title.AutoSize = true;
            this.label_werk_title.Location = new System.Drawing.Point(6, 64);
            this.label_werk_title.Name = "label_werk_title";
            this.label_werk_title.Size = new System.Drawing.Size(29, 15);
            this.label_werk_title.TabIndex = 3;
            this.label_werk_title.Text = "Title";
            // 
            // comboBox_werk_environment
            // 
            this.comboBox_werk_environment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_werk_environment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_werk_environment.FormattingEnabled = true;
            this.comboBox_werk_environment.Location = new System.Drawing.Point(6, 216);
            this.comboBox_werk_environment.Name = "comboBox_werk_environment";
            this.comboBox_werk_environment.Size = new System.Drawing.Size(275, 23);
            this.comboBox_werk_environment.TabIndex = 10;
            // 
            // dateTimePicker_werk_created
            // 
            this.dateTimePicker_werk_created.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker_werk_created.CustomFormat = "dd.MM.yyyy HH:mm:ss";
            this.dateTimePicker_werk_created.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_werk_created.Location = new System.Drawing.Point(6, 171);
            this.dateTimePicker_werk_created.Name = "dateTimePicker_werk_created";
            this.dateTimePicker_werk_created.Size = new System.Drawing.Size(275, 23);
            this.dateTimePicker_werk_created.TabIndex = 8;
            // 
            // textBox_werk_name
            // 
            this.textBox_werk_name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_werk_name.Location = new System.Drawing.Point(6, 126);
            this.textBox_werk_name.Name = "textBox_werk_name";
            this.textBox_werk_name.Size = new System.Drawing.Size(194, 23);
            this.textBox_werk_name.TabIndex = 6;
            this.textBox_werk_name.TextChanged += new System.EventHandler(this.TextBoxTextChanged);
            // 
            // textBox_werk_title
            // 
            this.textBox_werk_title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_werk_title.Location = new System.Drawing.Point(6, 81);
            this.textBox_werk_title.Name = "textBox_werk_title";
            this.textBox_werk_title.Size = new System.Drawing.Size(275, 23);
            this.textBox_werk_title.TabIndex = 4;
            this.textBox_werk_title.TextChanged += new System.EventHandler(this.TextBoxTextChanged);
            // 
            // button_save
            // 
            this.button_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_save.Location = new System.Drawing.Point(224, 526);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(75, 23);
            this.button_save.TabIndex = 2;
            this.button_save.Text = "Create";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.ButtonSaveClick);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_cancel.Location = new System.Drawing.Point(12, 526);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 1;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.ButtonCancelClick);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.png, *.ico) | *.jpg; *.jpeg; *.jpe; *.png; *" +
    ".ico";
            this.openFileDialog.Title = "Select icon for werk";
            // 
            // FormWerk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 556);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_save);
            this.Controls.Add(this.groupBox_werk);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(271, 434);
            this.Name = "FormWerk";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Werkbank: New Werk";
            this.groupBox_werk.ResumeLayout(false);
            this.groupBox_werk.PerformLayout();
            this.groupBox_werk_icon.ResumeLayout(false);
            this.panel_werk_icon.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_werk_icon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox_werk;
        private ComboBox comboBox_werk_environment;
        private DateTimePicker dateTimePicker_werk_created;
        private TextBox textBox_werk_name;
        private TextBox textBox_werk_title;
        private Label label_werk_title;
        private Label label_werk_name;
        private Label label_werk_environment;
        private Label label_werk_created;
        private Button button_werk_name;
        private Button button_save;
        private Button button_cancel;
        private CheckBox checkBox_werk_compressOnArchive;
        private Label label_werk_description;
        private TextBox textBox_werk_description;
        private Label label_werk_id;
        private TextBox textBox_werk_id;
        private GroupBox groupBox_werk_icon;
        private PictureBox pictureBox_werk_icon;
        private Button button_werk_select_image;
        private OpenFileDialog openFileDialog;
        private Panel panel_werk_icon;
    }
}