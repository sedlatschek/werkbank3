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
using werkbank.exceptions;
using werkbank.models;

namespace werkbank
{
    public partial class FormHistory : Form
    {
        public Werk? Werk;

        private readonly ObjectListView objectListView;

        public Color ColorHot = Color.RosyBrown;
        public Color ColorCold = Color.LightBlue;
        public Color ColorArchived = Color.LightGray;

        public FormHistory()
        {
            InitializeComponent();

            Text = Application.ProductName + ": History";

            objectListView = new ObjectListView()
            {
                MultiSelect = false,
                FullRowSelect = true,
                UseCellFormatEvents = true,
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
                AspectToStringConverter = (object env) =>
                {
                    if (env == null)
                    {
                        return "Unknown";
                    }
                    return ((environments.Environment)env).Name;
                },
                Text = "Environment",
                TextAlign = HorizontalAlignment.Center,
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
                TextAlign = HorizontalAlignment.Center,
                IsEditable = false,
                Searchable = false,
                Sortable = false,
                Width = 80,
            };
            objectListView.Columns.Add(colState);

            objectListView.FormatCell += (sender, e) =>
            {
                if (e.Column == colState)
                {
                    WerkStateTimestamp werkStateTimestamp = (WerkStateTimestamp)e.Item.RowObject;
                    e.SubItem.BackColor = GetColorForState(werkStateTimestamp.State);
                }
            };

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

        private void FormHistoryShown(object sender, EventArgs e)
        {
            if (Werk != null)
            {
                objectListView.RefreshObjects(Werk.History);
            }
        }

        /// <summary>
        /// Get the configured color for a given werk state.
        /// </summary>
        /// <param name="State"></param>
        /// <returns></returns>
        /// <exception cref="UnhandledWerkStateException"></exception>
        private Color GetColorForState(WerkState State)
        {
            return State switch
            {
                WerkState.Hot => ColorHot,
                WerkState.Cold => ColorCold,
                WerkState.Archived => ColorArchived,
                _ => throw new UnhandledWerkStateException(State),
            };
        }
    }
}
