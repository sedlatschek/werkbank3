using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Newtonsoft.Json;
using werkbank.models;
using werkbank.operations;
using werkbank.transitions;

namespace werkbank
{
    public partial class FormQueue : Form
    {
        private readonly List<Batch> batches;

        private readonly ObjectListView objectListView;

        private readonly OLVColumn colBatch;

        private int totalOperationsCount = 0;
        /// <summary>
        /// The total count of operations within the queue.
        /// </summary>
        public int TotalOperationsCount => totalOperationsCount;

        private int openOperationsCount = 0;
        /// <summary>
        /// The count of open operations within the queue.
        /// </summary>
        public int OpenOperationsCount => openOperationsCount;

        private int doneOperationsCount = 0;
        /// <summary>
        /// The count of done operations within the queue.
        /// </summary>
        public int DoneOperationsCount => doneOperationsCount;

        public event EventHandler? Changed;

        public FormQueue()
        {
            InitializeComponent();

            batches = Restore() ?? new List<Batch>();

            objectListView = new ObjectListView()
            {
                MultiSelect = false,
                FullRowSelect = true
            };

            objectListView.ItemsChanged += ObjectListViewItemsChanged;

            OLVColumn colWerk = new()
            {
                Name = objectListView.Name + "_column_werk",
                AspectGetter = (object x) =>
                {
                    return ((Operation)x).Batch.Werk?.Name ?? "";
                },
                Text = "Werk",
                IsEditable = false,
                Searchable = false,
                Sortable = false,
                Width = 120,
            };
            objectListView.Columns.Add(colWerk);

            OLVColumn colTransition = new()
            {
                Name = objectListView.Name + "_column_transition",
                AspectGetter = (object x) =>
                {
                    return ((Operation)x).Batch.Title;
                },
                Text = "Transition",
                IsEditable = false,
                Searchable = false,
                Sortable = false,
                Width = 89,
            };
            objectListView.Columns.Add(colTransition);

            OLVColumn colType = new()
            {
                Name = objectListView.Name + "_column_operation_type",
                AspectGetter = (object x) =>
                {
                    return ((Operation)x).Type;
                },
                Text = "Type",
                IsEditable = false,
                Searchable = false,
                Sortable = false,
                Width = 89,
            };
            objectListView.Columns.Add(colType);

            OLVColumn colSource = new()
            {
                Name = objectListView.Name + "_column_source",
                AspectName = "Source",
                Text = "Source",
                IsEditable = false,
                Searchable = false,
                Sortable = false,
                Width = 160,
            };
            objectListView.Columns.Add(colSource);

            OLVColumn colDest = new()
            {
                Name = objectListView.Name + "_column_dest",
                AspectName = "Destination",
                Text = "Destination",
                IsEditable = false,
                Searchable = false,
                Sortable = false,
                Width = 160,
            };
            objectListView.Columns.Add(colDest);

            OLVColumn colState = new()
            {
                Name = objectListView.Name + "_column_state",
                AspectGetter = (object x) =>
                {
                    Operation op = (Operation)x;
                    if (op.Running)
                        return "Running";
                    else if (op.Success)
                        return "Success";
                    else if (op.IsInTimeout)
                        return "Timeout";
                    else
                        return "Pending";
                },
                Text = "State",
                IsEditable = false,
                Searchable = false,
                Sortable = false,
                Width = 56,
            };
            objectListView.Columns.Add(colState);

            OLVColumn colAttempt = new()
            {
                Name = objectListView.Name + "_column_attempt",
                AspectName = "Attempt",
                Text = "Attempt",
                IsEditable = false,
                Searchable = false,
                Sortable = false,
                Width = 26,
            };
            objectListView.Columns.Add(colAttempt);

            OLVColumn colLastAttempt = new()
            {
                Name = objectListView.Name + "_column_last_attempt",
                AspectGetter = (object x) =>
                {
                    Operation op = (Operation)x;
                    if (op.LastAttempt == null)
                    {
                        return string.Empty;
                    }
                    return op.LastAttempt;
                },
                Text = "Last Attempt",
                IsEditable = false,
                Searchable = false,
                Sortable = false,
                Width = 124,
            };
            objectListView.Columns.Add(colLastAttempt);

            OLVColumn colError = new()
            {
                Name = objectListView.Name + "_column_error",
                AspectGetter = (object x) =>
                {
                    Operation op = (Operation)x;
                    if (op.Error == null)
                    {
                        return string.Empty;
                    }
                    return op.Error.ToString();
                },
                Text = "Error",
                IsEditable = false,
                Searchable = false,
                Sortable = false,
                Width = 124,
            };
            objectListView.Columns.Add(colError);

            OLVColumn colCreated = new()
            {
                Name = objectListView.Name + "_column_created",
                AspectName = "Created",
                Text = "Created",
                IsEditable = false,
                Searchable = false,
                Sortable = false,
                Width = 124,
            };
            objectListView.Columns.Add(colCreated);

            OLVColumn colId = new()
            {
                Name = objectListView.Name + "_column_id",
                AspectName = "Id",
                Text = "Id",
                IsEditable = false,
                Searchable = false,
                Sortable = false,
                Width = 244,
            };
            objectListView.Columns.Add(colId);

            colBatch = new()
            {
                Name = objectListView.Name + "_column_batch_id",
                AspectGetter = (object x) =>
                {
                    return ((Operation)x).Batch.Id;
                },
                Text = "Batch",
                IsEditable = false,
                Searchable = false,
                Sortable = true,
                Width = 244,
            };
            objectListView.Columns.Add(colBatch);

            objectListView.FormatCell += (sender, e) =>
            {
                if (e.Column == colState)
                {
                    Operation op = (Operation)e.Item.RowObject;
                    if (op.Running)
                    {
                        e.SubItem.BackColor = Color.Blue;
                        e.SubItem.ForeColor = Color.White;
                    }
                    else if (op.Success)
                    {
                        e.SubItem.BackColor = Color.Green;
                        e.SubItem.ForeColor = Color.White;
                    }
                    else if (op.IsInTimeout)
                    {
                        e.SubItem.BackColor = Color.DarkOrange;
                        e.SubItem.ForeColor = Color.White;
                    }
                }
            };

            SyncLists();

            panel_objectListView.Controls.Add(objectListView);
            objectListView.Dock = DockStyle.Fill;
        }

        private void ObjectListViewItemsChanged(object? sender, ItemsChangedEventArgs e)
        {
            RegisterOperationChange();
        }

        private void RegisterOperationChange()
        {
            List<Operation> operations = objectListView.Objects.Cast<Operation>().ToList();
            openOperationsCount = operations.Count(op => !op.Success);
            doneOperationsCount = operations.Count(op => op.Success);
            totalOperationsCount = openOperationsCount + doneOperationsCount;
            Changed?.Invoke(this, EventArgs.Empty);
        }

        private void FormQueueShown(object sender, EventArgs e)
        {
            GroupByBatch();
        }

        /// <summary>
        /// Restore a state of the queue from the hard drive.
        /// </summary>
        /// <returns></returns>
        private static List<Batch>? Restore()
        {
            if (File.Exists(Config.FileQueue))
            {
                string content = File.ReadAllText(Config.FileQueue);
                return JsonConvert.DeserializeObject<List<Batch>>(content);
            }
            return null;
        }
        /// <summary>
        /// Relates the current queue with the current list of werke. Needs to be called upon Deserialization.
        /// </summary>
        /// <param name="Werke"></param>
        public void RelateWithWerke(List<Werk> Werke)
        {
            foreach (Werk werk in Werke)
            {
                foreach (Batch batch in batches)
                {
                    if (batch.WerkId == werk.Id)
                    {
                        batch.Werk = werk;
                        werk.CurrentBatch = batch;
                        foreach (Operation op in batch.Operations)
                        {
                            objectListView.RefreshObject(op);
                        }
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Save the current state of the queue to the hard drive.
        /// </summary>
        public void Save()
        {
            File.WriteAllText(
                Config.FileQueue,
                JsonConvert.SerializeObject(batches)
            );
        }

        /// <summary>
        /// Run the queue
        /// </summary>
        public void Run()
        {
            if (!worker.IsBusy)
            {
                worker.RunWorkerAsync();
            }
        }

        private void SyncLists()
        {
            objectListView.BeginUpdate();

            try
            {
                objectListView.Items.Clear();

                foreach (Batch batch in batches)
                {
                    foreach (Operation operation in batch.Operations)
                    {
                        operation.Batch = batch;
                    }
                    objectListView.AddObjects(batch.Operations);
                }
            }
            finally
            {
                objectListView.EndUpdate();
            }
        }

        private void GroupByBatch()
        {
            objectListView.BuildGroups(colBatch, SortOrder.Ascending);
        }

        private void OnWork(object sender, DoWorkEventArgs e)
        {
            Batch? batch = GetNextBatch();
            while (batch != null)
            {
                batch.OnOperationStart += (senderPackage, op) =>
                {
                    worker.ReportProgress(0, op);
                    if (Visible)
                    {
                        Invoke(new Action(() =>
                        {
                            objectListView.RefreshObject(op);
                        }));
                    }
                    RegisterOperationChange();
                    Save();
                };
                batch.OnOperationDone += (senderPackage, op) =>
                {
                    worker.ReportProgress(0, op);
                    RegisterOperationChange();
                    if (Visible)
                    {
                        Invoke(new Action(() =>
                        {
                            objectListView.RefreshObject(op);
                        }));
                    }
                    RegisterOperationChange();
                    Save();
                };

                batch.Run();

                if (batch.Done)
                {
                    RemoveBatch(batch);
                }
                else
                {
                    if (batch.Error != null)
                    {
                        throw batch.Error;
                    }
                }

                Save();

                batch = GetNextBatch();
            }
        }

        private void OnWorkProgress(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
            {
                Operation op = (Operation)e.UserState;
                objectListView.RefreshObject(op);
            }
        }

        private void OnWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Queue in a new batch of operations.
        /// </summary>
        /// <param name="Batch"></param>
        public void Add(Batch Batch)
        {
            batches.Add(Batch);
            objectListView.AddObjects(Batch.Operations);
        }

        /// <summary>
        /// Remove a batch from the queue.
        /// </summary>
        /// <param name="Batch"></param>
        private void RemoveBatch(Batch Batch)
        {
            batches.Remove(Batch);
            objectListView.RemoveObjects(Batch.Operations);
        }

        /// <summary>
        /// Get the next batch in line.
        /// </summary>
        /// <returns></returns>
        private Batch? GetNextBatch()
        {
            foreach (Batch batch in batches)
            {
                if (!batch.Done && !batch.IsInTimeout && batch.Werk != null)
                {
                    return batch;
                }
            }
            return null;
        }
    }
}
