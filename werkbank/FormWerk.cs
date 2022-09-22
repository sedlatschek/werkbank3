using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Forms;
using Slugify;
using werkbank.environments;
using werkbank.exceptions;
using werkbank.models;
using werkbank.operations;
using werkbank.repositories;
using werkbank.services;

namespace werkbank
{
    public partial class FormWerk : Form
    {
        public enum FormWerkMode
        {
            Create,
            Edit
        }

        private readonly ImageList? iconList;

        private FormWerkMode mode = FormWerkMode.Create;

        private Werk? werk;

        public Werk? Werk
        {
            get { return werk; }
            set
            {
                werk = value;
                if (werk != null)
                {
                    textBox_werk_id.Text = werk.Id.ToString();
                    textBox_werk_name.Text = werk.Name;
                    textBox_werk_title.Text = werk.Title;
                    textBox_werk_description.Text = werk.Description;

                    dateTimePicker_werk_created.Value = werk.CreatedAt;

                    comboBox_werk_environment.SelectedIndex = werk.Environment.Index;

                    checkBox_werk_compressOnArchive.Enabled = werk.State != WerkState.Archived;
                    checkBox_werk_compressOnArchive.Checked = werk.CompressOnArchive;

                    if (werk.HasIcon)
                    {
                        // load icon without locking it
                        Image icon;
                        using (var bmpTemp = new Bitmap(werk.IconFile))
                        {
                            icon = new Bitmap(bmpTemp);
                        }
                        pictureBox_werk_icon.Image = icon;
                    }

                }
            }
        }

        public FormWerkMode Mode
        {
            get { return mode; }
            set
            {
                mode = value;
                if (value == FormWerkMode.Create)
                {
                    Text = Application.ProductName + ": New Werk";
                    button_save.Text = "Create";
                    textBox_werk_id.Text = Guid.NewGuid().ToString();
                    textBox_werk_name.Enabled = true;
                }
                else
                {
                    Text = Application.ProductName + ": Edit Werk";
                    button_save.Text = "Save";
                    textBox_werk_name.Enabled = false;
                    button_werk_name.Enabled = false;
                    comboBox_werk_environment.Enabled = false;
                }
            }
        }

        public FormWerk(ImageList? IconList = null)
        {
            if (IconList != null)
            {
                iconList = IconList;
            }
            else
            {
                iconList = new ImageList();
            }

            InitializeComponent();

            foreach (environments.Environment environment in EnvironmentRepository.Environments)
            {
                comboBox_werk_environment.Items.Add(environment.Name);
            }
            comboBox_werk_environment.SelectedIndex = 0;

            UpdateButtonAvailability();
        }

        private void TextBoxTextChanged(object sender, EventArgs e)
        {
            UpdateButtonAvailability();
        }

        private void UpdateButtonAvailability()
        {
            button_save.Enabled = !string.IsNullOrEmpty(textBox_werk_id.Text)
                && !string.IsNullOrEmpty(textBox_werk_name.Text)
                && !string.IsNullOrEmpty(textBox_werk_title.Text);
        }

        private void ButtonWerkSelectImageClick(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // load icon without locking it
                Image icon;
                using (var bmpTemp = new Bitmap(openFileDialog.FileName))
                {
                    icon = new Bitmap(bmpTemp);
                }

                pictureBox_werk_icon.Image = ImageService.ResizeImage(icon, pictureBox_werk_icon.Width, pictureBox_werk_icon.Height); ;
            }
        }

        private void ButtonWerkNameClick(object sender, EventArgs e)
        {
            SlugHelper slugHelper = new();
            textBox_werk_name.Text = slugHelper.GenerateSlug(
                textBox_werk_title.Text
                    .Replace("ö", "oe")
                    .Replace("ä", "ae")
                    .Replace("ü", "ue")
                    .Replace("ß", "ss")
            );
        }

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            Werk = null;
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ButtonSaveClick(object sender, EventArgs e)
        {
            // retrieve environment
            environments.Environment environment = EnvironmentRepository.Environments[comboBox_werk_environment.SelectedIndex];

            if (Mode == FormWerkMode.Create)
            {
                Guid guid = Guid.Parse(textBox_werk_id.Text);

                // check if werk already exists
                if (IsWerkNameAlreadyInUse(textBox_werk_name.Text))
                {
                    throw new WerkNameAlreadyInUseException(textBox_werk_name.Text);
                }

                // setup werk
                Werk werk = new(guid, textBox_werk_name.Text, textBox_werk_title.Text, environment, dateTimePicker_werk_created.Value)
                {
                    Description = textBox_werk_description.Text,
                    CompressOnArchive = checkBox_werk_compressOnArchive.Checked
                };

                Directory.CreateDirectory(werk.CurrentDirectory);
                DirectoryInfo metaDir = Directory.CreateDirectory(Path.Combine(werk.CurrentDirectory, Config.DirNameMeta));
                operations.Hide.Perform(metaDir.FullName);

                // write meta json
                werk.Save();

                // trigger created event
                werk.Environment.Bootstrap(werk);

                Werk = werk;
            }
            else
            {
                if (Werk == null)
                {
                    throw new Exception("A werk must be set to use the edit mode");
                }

                Werk.Title = textBox_werk_title.Text;
                Werk.Description = textBox_werk_description.Text;
                Werk.CreatedAt = dateTimePicker_werk_created.Value;
                Werk.CompressOnArchive = checkBox_werk_compressOnArchive.Checked;

                // write meta json
                Werk.Save();

                // trigger updated event
                Werk.Environment.Updated(Werk);
            }

            // icon
            if (pictureBox_werk_icon.Image != null)
            {
                pictureBox_werk_icon.Image.Save(
                    Path.Combine(Werk.CurrentDirectory, Config.DirNameMeta, Config.FileNameWerkIcon),
                    System.Drawing.Imaging.ImageFormat.Png
                );
                if (iconList != null)
                {
                    if (iconList.Images.ContainsKey(Werk.Id.ToString()))
                    {
                        iconList.Images.RemoveByKey(Werk.Id.ToString());
                    }
                    iconList.Images.Add(Werk.Id.ToString(), pictureBox_werk_icon.Image);
                }
            }

            // close dialog
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Determine whether a folder for a werk with this name already exists.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        private static bool IsWerkNameAlreadyInUse(string Name)
        {
            foreach (string folder in EnvironmentRepository.Directories)
            {
                if (Directory.Exists(Path.Combine(Settings.Properties.DirHotVault, folder, Name))
                    || Directory.Exists(Path.Combine(Settings.Properties.DirColdVault, folder, Name))
                    || Directory.Exists(Path.Combine(Settings.Properties.DirArchiveVault, folder, Name)))
                {
                    return true;
                }

            }
            return false;
        }

        private void FormWerkShown(object sender, EventArgs e)
        {
            textBox_werk_title.Focus();
        }

        private void ComboBoxWerkEnvironmentSelectedIndexChanged(object sender, EventArgs e)
        {
            environments.Environment environment = EnvironmentRepository.Environments[comboBox_werk_environment.SelectedIndex];

            if (environment.Preset == null)
            {
                return;
            }

            if (Werk == null || Werk.State != WerkState.Archived)
            {
                if (environment.Preset.CompressOnArchive != null)
                {
                    checkBox_werk_compressOnArchive.Checked = (bool)environment.Preset.CompressOnArchive;
                }
            }
        }
    }
}
