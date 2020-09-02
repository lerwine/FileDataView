using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileDataView
{
    public class BinaryCompareTabPage : TabPage
    {
        private readonly IContainer components;
        private readonly BindingList<FileInfo> leftFilesList;
        private readonly BindingList<ByteBlock> leftBlocksList;
        private readonly BindingList<FileInfo> rightFilesList;
        private readonly BindingList<ByteBlock> rightBlocksList;
        private readonly BindingSource leftFilesBindingSource;
        private readonly BindingSource leftBlocksBindingSource;
        private readonly BindingSource rightFilesBindingSource;
        private readonly BindingSource rightBlocksBindingSource;
        private readonly TableLayoutPanel outerTableLayoutPanel;
        private readonly SplitContainer splitContainer;
        private readonly TableLayoutPanel leftTableLayoutPanel;
        private readonly Label leftFileLabel;
        private readonly ComboBox leftFileComboBox;
        private readonly DataGridView leftBlocksDataGridView;
        private readonly TableLayoutPanel rightTableLayoutPanel;
        private readonly Label rightFileLabel;
        private readonly ComboBox rightFileComboBox;
        private readonly DataGridView rightBlocksDataGridView;
        private readonly Label lineNumberHeadingLabel;
        private readonly Label lineNumberValueLabel;
        private readonly TextBox lineOverLineTextBox;

        public BinaryCompareTabPage(FileInfo[] files)
        {
            components = new Container();
            leftFilesList = new BindingList<FileInfo>();
            leftBlocksList = new BindingList<ByteBlock>();
            rightFilesList = new BindingList<FileInfo>();
            rightBlocksList = new BindingList<ByteBlock>();
            if (files.Length > 0)
            {
                leftFilesList.Add(files[0]);
                if (files.Length > 1)
                {
                    rightFilesList.Add(files[1]);
                    for (int i = 2; i < files.Length; i++)
                    {
                        leftFilesList.Add(files[i]);
                        rightFilesList.Add(files[i]);
                    }
                }
            }
            leftFilesBindingSource = new BindingSource(components);
            leftBlocksBindingSource = new BindingSource(components);
            rightFilesBindingSource = new BindingSource(components);
            rightBlocksBindingSource = new BindingSource(components);
            SuspendLayout();
            outerTableLayoutPanel = new TableLayoutPanel();
            outerTableLayoutPanel.SuspendLayout();
            Controls.Add(outerTableLayoutPanel);
            outerTableLayoutPanel.RowCount = 2;
            outerTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f));
            outerTableLayoutPanel.RowStyles.Add(new RowStyle());
            outerTableLayoutPanel.ColumnCount = 3;
            outerTableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            outerTableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            outerTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f));
            splitContainer = new SplitContainer();
            splitContainer.SuspendLayout();
            outerTableLayoutPanel.Controls.Add(splitContainer, 0, 0);
            outerTableLayoutPanel.SetColumnSpan(splitContainer, 3);
            // #region leftTableLayoutPanel
            leftTableLayoutPanel = new TableLayoutPanel();
            leftTableLayoutPanel.SuspendLayout();
            splitContainer.Panel1.Controls.Add(leftTableLayoutPanel);
            leftTableLayoutPanel.RowCount = 2;
            leftTableLayoutPanel.RowStyles.Add(new RowStyle());
            leftTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f));
            leftTableLayoutPanel.ColumnCount = 2;
            leftTableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            leftTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f));
            leftFileLabel = new Label();
            leftTableLayoutPanel.Controls.Add(leftFileLabel, 0, 0);
            leftFileComboBox = new ComboBox();
            leftTableLayoutPanel.Controls.Add(leftFileComboBox, 1, 0);
            leftBlocksDataGridView = new DataGridView();
            leftTableLayoutPanel.Controls.Add(leftBlocksDataGridView, 1, 0);
            leftTableLayoutPanel.SetColumnSpan(leftBlocksDataGridView, 2);
            // #endregion
            // #region rightTableLayoutPanel
            rightTableLayoutPanel = new TableLayoutPanel();
            rightTableLayoutPanel.SuspendLayout();
            splitContainer.Panel2.Controls.Add(rightTableLayoutPanel);
            rightTableLayoutPanel.RowCount = 2;
            rightTableLayoutPanel.RowStyles.Add(new RowStyle());
            rightTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f));
            rightTableLayoutPanel.ColumnCount = 2;
            rightTableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            rightTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f));
            rightFileLabel = new Label();
            rightTableLayoutPanel.Controls.Add(rightFileLabel, 0, 0);
            rightFileComboBox = new ComboBox();
            rightTableLayoutPanel.Controls.Add(rightFileComboBox, 1, 0);
            rightBlocksDataGridView = new DataGridView();
            rightTableLayoutPanel.Controls.Add(rightBlocksDataGridView, 1, 0);
            rightTableLayoutPanel.SetColumnSpan(rightBlocksDataGridView, 2);
            // #endregion
            lineNumberHeadingLabel = new Label();
            outerTableLayoutPanel.Controls.Add(lineNumberHeadingLabel, 0, 1);
            lineNumberValueLabel = new Label();
            outerTableLayoutPanel.Controls.Add(lineNumberValueLabel, 1, 1);
            lineOverLineTextBox = new TextBox();
            outerTableLayoutPanel.Controls.Add(lineOverLineTextBox, 2, 1);
        }

        internal int InitializeComponents(int tabIndex)
        {
            leftFilesBindingSource.DataSource = leftFilesList;
            leftBlocksBindingSource.DataSource = leftBlocksList;
            rightFilesBindingSource.DataSource = rightFilesList;
            rightBlocksBindingSource.DataSource = rightBlocksList;
            AutoSize = true;
            Text = "Bytes";
            outerTableLayoutPanel.AutoSize = true;
            outerTableLayoutPanel.Dock = DockStyle.Fill;
            splitContainer.AutoSize = true;
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.TabIndex = tabIndex++;
            // #region rightTableLayoutPanel
            leftTableLayoutPanel.AutoSize = true;
            leftTableLayoutPanel.Dock = DockStyle.Fill;
            leftFileLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            leftFileLabel.Margin = new Padding(3, 3, 0, 3);
            leftFileLabel.AutoSize = true;
            leftFileLabel.Font = new Font(leftFileLabel.Font, FontStyle.Bold);
            leftFileLabel.Text = "First File:";
            leftFileComboBox.Dock = DockStyle.Top;
            leftFileComboBox.Margin = new Padding(0, 3, 3, 3);
            leftFileComboBox.DataSource = leftFilesBindingSource;
            leftFileComboBox.DisplayMember = "Name";
            leftBlocksDataGridView.Dock = DockStyle.Fill;
            leftBlocksDataGridView.ReadOnly = true;
            leftBlocksDataGridView.AutoGenerateColumns = false;
            leftBlocksDataGridView.AllowUserToAddRows = false;
            leftBlocksDataGridView.AllowUserToDeleteRows = false;
            leftBlocksDataGridView.DataSource = leftBlocksBindingSource;
            leftBlocksDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            leftBlocksDataGridView.MultiSelect = false;
            leftBlocksDataGridView.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn() { DataPropertyName = "DWord0", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true },
                new DataGridViewTextBoxColumn() { DataPropertyName = "DWord1", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true },
                new DataGridViewTextBoxColumn() { DataPropertyName = "DWord2", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true },
                new DataGridViewTextBoxColumn() { DataPropertyName = "DWord3", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true }
            });
            // #endregion
            // #region rightTableLayoutPanel
            rightTableLayoutPanel.AutoSize = true;
            rightTableLayoutPanel.Dock = DockStyle.Fill;
            rightFileLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rightFileLabel.Margin = new Padding(3, 3, 0, 3);
            rightFileLabel.AutoSize = true;
            rightFileLabel.Font = new Font(rightFileLabel.Font, FontStyle.Bold);
            rightFileLabel.Text = "Second File:";
            rightFileComboBox.Dock = DockStyle.Top;
            rightFileComboBox.Margin = new Padding(0, 3, 3, 3);
            rightFileComboBox.DataSource = rightFilesBindingSource;
            rightFileComboBox.DisplayMember = "Name";
            rightBlocksDataGridView.ReadOnly = true;
            rightBlocksDataGridView.AutoGenerateColumns = false;
            rightBlocksDataGridView.AllowUserToAddRows = false;
            rightBlocksDataGridView.AllowUserToDeleteRows = false;
            rightBlocksDataGridView.Dock = DockStyle.Fill;
            rightBlocksDataGridView.DataSource = rightBlocksBindingSource;
            rightBlocksDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            rightBlocksDataGridView.MultiSelect = false;
            rightBlocksDataGridView.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn() { DataPropertyName = "DWord0", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true },
                new DataGridViewTextBoxColumn() { DataPropertyName = "DWord1", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true },
                new DataGridViewTextBoxColumn() { DataPropertyName = "DWord2", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true },
                new DataGridViewTextBoxColumn() { DataPropertyName = "DWord3", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true }
            });
            // #endregion
            lineNumberHeadingLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lineNumberHeadingLabel.Margin = new Padding(3, 3, 0, 3);
            lineNumberHeadingLabel.AutoSize = true;
            lineNumberHeadingLabel.Font = new Font(rightFileLabel.Font, FontStyle.Bold);
            lineNumberHeadingLabel.Text = "Byte:";
            lineNumberValueLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            lineNumberValueLabel.Margin = new Padding(0, 3, 3, 3);
            lineNumberValueLabel.AutoSize = true;
            lineNumberValueLabel.Text = "0";
            lineOverLineTextBox.Dock = DockStyle.Fill;
            lineOverLineTextBox.Multiline = true;
            Font font = new Font(FontFamily.GenericMonospace, 10.0f);
            lineOverLineTextBox.Font = font;
            lineOverLineTextBox.MinimumSize = new Size(1, (font.Height * 3) + 6);
            lineOverLineTextBox.ScrollBars = ScrollBars.Horizontal;
            lineOverLineTextBox.ReadOnly = true;
            lineOverLineTextBox.WordWrap = false;
            FileInfo leftFile, rightFile;
            if (rightFilesList.Count > 1)
            {
                leftFileComboBox.SelectedItem = leftFile = leftFilesList[0];
                leftFileComboBox.TabIndex = tabIndex++;
                leftBlocksDataGridView.TabIndex = tabIndex++;
                rightFileComboBox.SelectedItem = rightFile = rightFilesList[0];
                rightFileComboBox.TabIndex = tabIndex++;
                rightBlocksDataGridView.TabIndex = tabIndex++;
                leftFileComboBox.SelectedIndexChanged += leftFileComboBox_SelectedIndexChanged;
                rightFileComboBox.SelectedIndexChanged += rightFileComboBox_SelectedIndexChanged;
            }
            else
            {
                if (rightFilesList.Count > 0)
                {
                    leftFileComboBox.SelectedItem = leftFile = leftFilesList[0];
                    rightFileComboBox.SelectedItem = rightFile = rightFilesList[0];
                }
                else
                {
                    rightFile = null;
                    if (leftFilesList.Count > 0)
                        leftFileComboBox.SelectedItem = leftFile = leftFilesList[0];
                    else
                        leftFile = null;
                }
                leftFileComboBox.TabStop = rightFileComboBox.TabStop = false;
                leftFileLabel.Visible = leftFileComboBox.Visible = rightFileLabel.Visible = rightFileComboBox.Visible = false;
                leftFileComboBox.Enabled = rightFileComboBox.Enabled = false;
                leftBlocksDataGridView.TabIndex = tabIndex++;
                rightBlocksDataGridView.TabIndex = tabIndex++;
            }
            lineOverLineTextBox.TabIndex = tabIndex++;
            ByteBlock.Load(leftFile, leftBlocksList);
            ByteBlock.Load(rightFile, rightBlocksList);
            if (leftBlocksList.Count > 0)
                leftBlocksDataGridView.Rows[0].Selected = true;
            if (rightBlocksList.Count > 0)
                rightBlocksDataGridView.Rows[0].Selected = true;
            leftBlocksDataGridView_SelectionChanged(leftBlocksDataGridView, EventArgs.Empty);
            leftBlocksDataGridView.SelectionChanged += leftBlocksDataGridView_SelectionChanged;
            rightBlocksDataGridView.SelectionChanged += rightBlocksDataGridView_SelectionChanged;
            return tabIndex;
        }

        private int LeftSelectedIndex
        {
            get
            {
                ByteBlock item = LeftSelectedBlock;
                return (null == item) ? -1 : leftBlocksList.IndexOf(item);
            }
            set
            {
                if (value < 0 || value >= leftBlocksList.Count)
                    LeftSelectedBlock = null;
                else
                    LeftSelectedBlock = leftBlocksList[value];
            }
        }

        private ByteBlock LeftSelectedBlock
        {
            get
            {
                if (leftBlocksDataGridView.SelectedRows.Count > 0)
                    return leftBlocksDataGridView.SelectedRows[0].DataBoundItem as ByteBlock;
                return null;
            }
            set
            {
                if (null != value)
                {
                    for (int i = 0; i < leftBlocksDataGridView.RowCount; i++)
                    {
                        if (ReferenceEquals(leftBlocksDataGridView.Rows[i].DataBoundItem, value))
                        {
                            leftBlocksDataGridView.Rows[i].Selected = true;
                            return;
                        }
                    }
                }
                if (leftBlocksDataGridView.SelectedRows.Count > 0)
                    leftBlocksDataGridView.SelectedRows[0].Selected = false;
            }
        }
        
        private int RightSelectedIndex
        {
            get
            {
                ByteBlock item = RightSelectedBlock;
                return (null == item) ? -1 : rightBlocksList.IndexOf(item);
            }
            set
            {
                if (value < 0 || value >= rightBlocksList.Count)
                    RightSelectedBlock = null;
                else
                    RightSelectedBlock = rightBlocksList[value];
            }
        }

        private ByteBlock RightSelectedBlock
        {
            get
            {
                if (rightBlocksDataGridView.SelectedRows.Count > 0)
                    return rightBlocksDataGridView.SelectedRows[0].DataBoundItem as ByteBlock;
                return null;
            }
            set
            {
                if (null != value)
                {
                    for (int i = 0; i < rightBlocksDataGridView.RowCount; i++)
                    {
                        if (ReferenceEquals(rightBlocksDataGridView.Rows[i].DataBoundItem, value))
                        {
                            rightBlocksDataGridView.Rows[i].Selected = true;
                            return;
                        }
                    }
                }
                if (rightBlocksDataGridView.SelectedRows.Count > 0)
                    rightBlocksDataGridView.SelectedRows[0].Selected = false;
            }
        }
        
        private void UpdateLineOverLine()
        {
            lineNumberValueLabel.Text = (((RightSelectedIndex < LeftSelectedIndex) ? LeftSelectedIndex : RightSelectedIndex) * 32).ToString();
            ByteBlock leftBlock = LeftSelectedBlock;
            ByteBlock rightBlock = RightSelectedBlock;
            lineOverLineTextBox.Lines = new string[]
            {
                (null == leftBlock) ? "" : String.Join(" ", leftBlock.Data.Select(b => b.ToString("x2")).ToArray()),
                (null == rightBlock) ? "" : String.Join(" ", rightBlock.Data.Select(b => b.ToString("x2")).ToArray())
            };
        }

        private void leftFileComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FileInfo leftFile = leftFileComboBox.SelectedItem as FileInfo;
            foreach (FileInfo f in leftFilesList)
            {
                if (!rightFilesList.Contains(f))
                {
                    rightFilesList.Add(f);
                    break;
                }
            }
            rightFilesList.Remove(leftFile);

            int index = (LeftSelectedIndex <  RightSelectedIndex) ? RightSelectedIndex : LeftSelectedIndex;
            ByteBlock.Load(leftFile, leftBlocksList);
            if (LeftSelectedIndex == index)
                UpdateLineOverLine();
            else
                LeftSelectedIndex = index;
        }
        
        private void leftBlocksDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (LeftSelectedIndex != RightSelectedIndex && LeftSelectedIndex >= 0)
                RightSelectedIndex = LeftSelectedIndex;
            else
                UpdateLineOverLine();
        }

        private void rightFileComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FileInfo rightFile = rightFileComboBox.SelectedItem as FileInfo;
            foreach (FileInfo f in rightFilesList)
            {
                if (!leftFilesList.Contains(f))
                {
                    leftFilesList.Add(f);
                    break;
                }
            }
            leftFilesList.Remove(rightFile);
            int index = (RightSelectedIndex < LeftSelectedIndex) ? LeftSelectedIndex : RightSelectedIndex;
            ByteBlock.Load(rightFile, rightBlocksList);
            if (RightSelectedIndex == index)
                UpdateLineOverLine();
            else
                RightSelectedIndex = index;
        }
        
        private void rightBlocksDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (RightSelectedIndex != LeftSelectedIndex && RightSelectedIndex >= 0)
                LeftSelectedIndex = RightSelectedIndex;
            else
                UpdateLineOverLine();
        }
        
        internal void ResumeComponentLayout()
        {
            leftTableLayoutPanel.ResumeLayout(false);
            leftTableLayoutPanel.PerformLayout();
            rightTableLayoutPanel.ResumeLayout(false);
            rightTableLayoutPanel.PerformLayout();
            splitContainer.ResumeLayout(false);
            splitContainer.PerformLayout();
            outerTableLayoutPanel.ResumeLayout(false);
            outerTableLayoutPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                components.Dispose();
            base.Dispose(disposing);
        }
    }
}