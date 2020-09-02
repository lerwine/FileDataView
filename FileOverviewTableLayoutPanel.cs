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
    public abstract class FileOverviewTableLayoutPanel : TableLayoutPanel
    {
        protected Label DirectoryNameLabel { get; }
        protected TextBox DirectoryNameTextBox { get; }
        protected Label FileNameLabel { get; }
        protected TextBox FileNameTextBox { get; }
        
        protected FileOverviewTableLayoutPanel(int rowCount)
        {
            Name = "overviewTableLayoutPanel";
            SuspendLayout();
            DirectoryNameLabel = CreateLabel("directoryNameLabel");
            DirectoryNameTextBox = CreateTextBox("directoryNameTextBox");
            FileNameLabel = CreateLabel("fileNameLabel");
            FileNameTextBox = CreateTextBox("fileNameTextBox");
            AutoSize = true;
            Dock = DockStyle.Fill;
            ColumnCount = 2;
            rowCount = (RowCount = (rowCount < 4) ? 4 : RowCount) - 1;
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.0f));
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.0f));
            for (int i = 0; i < rowCount; i++)
                RowStyles.Add(new RowStyle());
            RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f));
            Controls.Add(DirectoryNameLabel, 0, 0);
            SetColumnSpan(DirectoryNameLabel, 2);
            Controls.Add(DirectoryNameTextBox, 0, 1);
            SetColumnSpan(DirectoryNameTextBox, 2);
            Controls.Add(FileNameLabel, 0, 2);
            Controls.Add(FileNameLabel, 0, 3);
        }

        protected internal abstract int InitializeControls(int baseTabIndex);

        internal virtual void ResumeComponentLayout()
        {
            DirectoryNameLabel.ResumeLayout(false);
            FileNameLabel.ResumeLayout(false);
            DirectoryNameTextBox.ResumeLayout(false);
            FileNameTextBox.ResumeLayout(false);
            OnResumeLayout();
            DirectoryNameLabel.PerformLayout();
            FileNameLabel.PerformLayout();
            DirectoryNameTextBox.PerformLayout();
            FileNameTextBox.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        protected abstract void OnResumeLayout();

        protected static Label CreateLabel(string name)
        {
            Label label = new Label();
            label.SuspendLayout();
            label.AutoSize = true;
            label.Font = new Font(label.Font, FontStyle.Bold);
            label.Name = name;
            return label;
        }
        protected static void InitializeLabel(Label label, string text, bool isTopOfControl)
        {
            if (isTopOfControl)
            {
                label.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                label.Margin = new Padding(3, 3, 3, 0);
            }
            else
            {
                label.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                label.Margin = new Padding(3, 3, 0, 3);
            }
            label.AutoSize = true;
            label.Font = new Font(label.Font, FontStyle.Bold);
            label.Text = text;
        }

        protected static TextBox CreateTextBox(string name)
        {
            TextBox textBox = new TextBox();
            textBox.SuspendLayout();
            textBox.Name = name;
            textBox.ReadOnly = true;
            return textBox;
        }

        protected static void InitializeTextBox(TextBox textBox, string text, int tabIndex, bool isMultiLine, bool isBottomOfLabel)
        {
            textBox.Multiline = isMultiLine;
            textBox.Dock = (isMultiLine) ? DockStyle.Fill : DockStyle.Top;
            if (isBottomOfLabel)
                textBox.Margin = new Padding(3, 0, 3, 3);
            else
                textBox.Margin = new Padding(0, 3, 3, 3);
            textBox.TabIndex = tabIndex;
        }

    }
}