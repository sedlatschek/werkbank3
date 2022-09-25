using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using werkbank.models;
using werkbank.repositories;

namespace werkbank
{
    public partial class FormStatistics : Form
    {
        private readonly ObjectListView envListView;

        private readonly List<Werk> werke;

        public FormStatistics(List<Werk>? Werke = null)
        {
            InitializeComponent();

            werke = Werke ?? new List<Werk>();

            Icon = Properties.Resources.logo;
            Text = Application.ProductName + ": Statistics";

            envListView = new ObjectListView()
            {
                MultiSelect = false,
                FullRowSelect = true,
            };

            OLVColumn colName = new()
            {
                Name = envListView.Name + "_column_name",
                AspectName = "Name",
                Text = "Name",
                Sortable = false,
                Width = 240,
            };
            envListView.Columns.Add(colName);

            OLVColumn colWerkCount = new()
            {
                Name = envListView.Name + "_column_werk_count",
                AspectGetter = (object e) =>
                {
                    environments.Environment environment = (environments.Environment)e;
                    return werke.Count(w => w.Environment.Handle == environment.Handle).ToString();
                },
                Text = "Werke",
                TextAlign = HorizontalAlignment.Center,
                Sortable = false,
                Width = 76,
            };
            envListView.Columns.Add(colWerkCount);

            OLVColumn colWerkCountPercentage = new()
            {
                Name = envListView.Name + "_column_werk_count_percentage",
                AspectGetter = (object e) =>
                {
                    environments.Environment environment = (environments.Environment)e;
                    int count = werke.Count(w => w.Environment.Handle == environment.Handle);
                    return ((100 / werke.Count) * count).ToString() + "%";
                },
                Text = "Partition",
                TextAlign = HorizontalAlignment.Center,
                Sortable = false,
                Width = 76,
            };
            envListView.Columns.Add(colWerkCountPercentage);

            panel_environments.Controls.Add(envListView);
            envListView.Dock = DockStyle.Fill;
        }

        private void FormStatisticsLoad(object sender, EventArgs e)
        {
            envListView.SetObjects(EnvironmentRepository.Environments);
        }
    }
}
