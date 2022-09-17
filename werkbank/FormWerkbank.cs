using werkbank.controls;
using werkbank.models;
using werkbank.services;

namespace werkbank
{
    public partial class FormWerkbank : Form
    {
        private readonly List<WerkList> vaults;
        private readonly WerkList vaultHot;

        private readonly ImageList IconList;

        public FormWerkbank()
        {
            vaults = new List<WerkList>();
            IconList = new ImageList();

            InitializeComponent();

            vaultHot = new WerkList(IconList, WerkState.Hot);
            vaults.Add(vaultHot);

            foreach (WerkList vault in vaults)
            {
                splitContainer1.Panel1.Controls.Add(vault);
                vault.Dock = DockStyle.Fill;
            }
        }

        private void FormWerkbank_Load(object sender, EventArgs e)
        {
            Settings.Save();

            vaultHot.GatherAsync();
        }
    }
}