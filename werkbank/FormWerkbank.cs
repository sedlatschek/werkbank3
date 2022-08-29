using werkbank.services;

namespace werkbank
{
    public partial class FormWerkbank : Form
    {
        public FormWerkbank()
        {
            InitializeComponent();
        }

        private void FormWerkbank_Load(object sender, EventArgs e)
        {
            Settings.Save();
        }
    }
}