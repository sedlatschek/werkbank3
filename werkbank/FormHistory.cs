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

namespace werkbank
{
    public partial class FormHistory : Form
    {
        public Werk? Werk;

        private readonly ObjectListView objectListView;

        public FormHistory()
        {
            InitializeComponent();

            Text = Application.ProductName + ": History";

            objectListView = new ObjectListView()
            {
                MultiSelect = false,
                FullRowSelect = true
            };

            OLVColumn colTimestamp = new()
            {
                Name = objectListView.Name + "_column_timestamp",
                AspectName = "Timestamp",
                Text = "Timestamp",
                IsEditable = false,
                Searchable = false,
                Sortable = false,
                Width = 130,
            };
            objectListView.Columns.Add(colTimestamp);

            OLVColumn colEnvironment = new()
            {
                Name = objectListView.Name + "_column_environment",
                AspectName = "Environment",
                Text = "Environment",
                AspectToStringConverter = (object env) =>
                {
                    if (env == null)
                    {
                        return "Unknown";
                    }
                    return ((environments.Environment)env).Name;
                },
                IsEditable = false,
                Searchable = false,
                Sortable = false,
                Width = 130,
            };
            objectListView.Columns.Add(colEnvironment);

            OLVColumn colState = new()
            {
                Name = objectListView.Name + "_column_state",
                AspectName = "State",
                Text = "State",
                IsEditable = false,
                Searchable = false,
                Sortable = false,
                Width = 80,
            };
            objectListView.Columns.Add(colState);

            panel_objectListView.Controls.Add(objectListView);
            objectListView.Dock = DockStyle.Fill;
        }

        private void FormHistoryLoad(object sender, EventArgs e)
        {
            if (Werk != null)
            {
                objectListView.SetObjects(Werk.History);
            }
        }
    }
}
