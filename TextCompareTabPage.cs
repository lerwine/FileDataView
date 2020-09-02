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
    public class TextCompareTabPage : TabPage
    {
        private readonly IContainer components;
        private readonly BindingList<FileInfo> leftFilesList;
        private readonly BindingList<string> leftLinesList;
        private readonly BindingList<FileInfo> rightFilesList;
        private readonly BindingList<string> rightLinesList;
        private readonly BindingSource leftFilesBindingSource;
        private readonly BindingSource leftLinesBindingSource;
        private readonly BindingSource rightFilesBindingSource;
        private readonly BindingSource rightLinesBindingSource;
        private readonly TableLayoutPanel outerTableLayoutPanel;
        private readonly SplitContainer splitContainer;
        private readonly TableLayoutPanel leftTableLayoutPanel;
        private readonly Label leftFileLabel;
        private readonly ComboBox leftFileComboBox;
        private readonly ListBox leftLinesListBox;
        private readonly TableLayoutPanel rightTableLayoutPanel;
        private readonly Label rightFileLabel;
        private readonly ComboBox rightFileComboBox;
        private readonly ListBox rightLinesListBox;
        private readonly Label lineNumberHeadingLabel;
        private readonly Label lineNumberValueLabel;
        private readonly TextBox lineOverLineTextBox;
        
        public TextCompareTabPage(FileInfo[] files)
        {
            components = new Container();
            leftFilesList = new BindingList<FileInfo>();
            leftLinesList = new BindingList<string>();
            rightFilesList = new BindingList<FileInfo>();
            rightLinesList = new BindingList<string>();
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
            leftLinesBindingSource = new BindingSource(components);
            rightFilesBindingSource = new BindingSource(components);
            rightLinesBindingSource = new BindingSource(components);
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
            leftLinesListBox = new ListBox();
            leftTableLayoutPanel.Controls.Add(leftLinesListBox, 1, 0);
            leftTableLayoutPanel.SetColumnSpan(leftLinesListBox, 2);
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
            rightLinesListBox = new ListBox();
            rightTableLayoutPanel.Controls.Add(rightLinesListBox, 1, 0);
            rightTableLayoutPanel.SetColumnSpan(rightLinesListBox, 2);
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
            leftLinesBindingSource.DataSource = leftLinesList;
            rightFilesBindingSource.DataSource = rightFilesList;
            rightLinesBindingSource.DataSource = rightLinesList;
            AutoSize = true;
            Text = "Lines";
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
            leftLinesListBox.Dock = DockStyle.Fill;
            leftLinesListBox.DataSource = leftLinesBindingSource;
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
            rightLinesListBox.Dock = DockStyle.Fill;
            rightLinesListBox.DataSource = rightLinesBindingSource;
            // #endregion
            lineNumberHeadingLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lineNumberHeadingLabel.Margin = new Padding(3, 3, 0, 3);
            lineNumberHeadingLabel.AutoSize = true;
            lineNumberHeadingLabel.Font = new Font(rightFileLabel.Font, FontStyle.Bold);
            lineNumberHeadingLabel.Text = "Line:";
            lineNumberValueLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            lineNumberValueLabel.Margin = new Padding(0, 3, 3, 3);
            lineNumberValueLabel.AutoSize = true;
            lineNumberValueLabel.Text = "0";
            lineOverLineTextBox.Dock = DockStyle.Top;
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
                leftLinesListBox.TabIndex = tabIndex++;
                rightFileComboBox.SelectedItem = rightFile = rightFilesList[0];
                rightFileComboBox.TabIndex = tabIndex++;
                rightLinesListBox.TabIndex = tabIndex++;
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
                leftLinesListBox.TabIndex = tabIndex++;
                rightLinesListBox.TabIndex = tabIndex++;
            }
            lineOverLineTextBox.TabIndex = tabIndex++;
            if (null != leftFile)
            {
                string line;
                using (StreamReader reader = leftFile.OpenText())
                {
                    while (null != (line = reader.ReadLine()))
                        leftLinesList.Add(line);
                }
                if (null != rightFile)
                    using (StreamReader reader = rightFile.OpenText())
                    {
                        while (null != (line = reader.ReadLine()))
                            rightLinesList.Add(line);
                    }
                if (leftLinesList.Count > 0)
                    leftLinesListBox.SelectedIndex = 0;
                if (rightLinesList.Count > 0)
                    rightLinesListBox.SelectedIndex = 0;
            }
            leftLinesListBox_SelectedIndexChanged(leftLinesListBox, EventArgs.Empty);
            leftLinesListBox.SelectedIndexChanged += leftLinesListBox_SelectedIndexChanged;
            rightLinesListBox.SelectedIndexChanged += rightLinesListBox_SelectedIndexChanged;

            return tabIndex;
        }

        private void UpdateLineOverLine(int lineNumber, string top, string bottom)
        {
            lineNumberValueLabel.Text = lineNumber.ToString();
            lineOverLineTextBox.Lines = new string[] { (null == top) ? "" : ToEncodedText(top), (null == bottom) ? "" : ToEncodedText(bottom) };
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
            int index = (leftLinesListBox.SelectedIndex < rightLinesListBox.SelectedIndex) ? rightLinesListBox.SelectedIndex : leftLinesListBox.SelectedIndex;
            leftLinesList.Clear();
            using (StreamReader reader = leftFile.OpenText())
            {
                string line;
                while (null != (line = reader.ReadLine()))
                    leftLinesList.Add(line);
            }
            if (leftLinesList.Count > index)
            {
                if (leftLinesListBox.SelectedIndex != index)
                    leftLinesListBox.SelectedIndex = index;
                else
                    UpdateLineOverLine(index + 1, leftLinesListBox.SelectedItem as string, (index == rightLinesListBox.SelectedIndex) ? rightLinesListBox.SelectedItem as string : null);
            }
            else if (rightLinesListBox.SelectedIndex != index)
                rightLinesListBox.SelectedIndex = index;
            else
                UpdateLineOverLine(index + 1, null, rightLinesListBox.SelectedItem as string);
        }
        
        private void leftLinesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (leftLinesListBox.SelectedIndex != rightLinesListBox.SelectedIndex)
            {
                if (leftLinesListBox.SelectedIndex < rightLinesList.Count)
                    rightLinesListBox.SelectedIndex = leftLinesListBox.SelectedIndex;
                else
                    UpdateLineOverLine(leftLinesListBox.SelectedIndex + 1, leftLinesListBox.SelectedItem as string, null);
            } else
                UpdateLineOverLine(leftLinesListBox.SelectedIndex + 1, leftLinesListBox.SelectedItem as string, rightLinesListBox.SelectedItem as string);
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
            int index = (rightLinesListBox.SelectedIndex < leftLinesListBox.SelectedIndex) ? leftLinesListBox.SelectedIndex : rightLinesListBox.SelectedIndex;
            rightLinesList.Clear();
            using (StreamReader reader = rightFile.OpenText())
            {
                string line;
                while (null != (line = reader.ReadLine()))
                    rightLinesList.Add(line);
            }
            if (rightLinesList.Count > index)
            {
                if (rightLinesListBox.SelectedIndex != index)
                    rightLinesListBox.SelectedIndex = index;
                else
                    UpdateLineOverLine(index + 1, (index == leftLinesListBox.SelectedIndex) ? leftLinesListBox.SelectedItem as string : null, rightLinesListBox.SelectedItem as string);
            }
            else if (leftLinesListBox.SelectedIndex != index)
                leftLinesListBox.SelectedIndex = index;
            else
                UpdateLineOverLine(index + 1, leftLinesListBox.SelectedItem as string, null);
        }
        
        private void rightLinesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rightLinesListBox.SelectedIndex != rightLinesListBox.SelectedIndex)
            {
                if (leftLinesListBox.SelectedIndex < leftLinesList.Count)
                    leftLinesListBox.SelectedIndex = rightLinesListBox.SelectedIndex;
                else
                    UpdateLineOverLine(rightLinesListBox.SelectedIndex + 1, null, rightLinesListBox.SelectedItem as string);
            } else
                UpdateLineOverLine(rightLinesListBox.SelectedIndex + 1, leftLinesListBox.SelectedItem as string, rightLinesListBox.SelectedItem as string);
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

        public static string ToEncodedText(string text)
        {
            if (text.Length == 0)
                return "\"\"";
            StringBuilder sb = new StringBuilder("\"");
            foreach (char c in text.ToCharArray())
            {
                switch (c)
                {
                    case '\a':
                        sb.Append("\\a");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\v':
                        sb.Append("«VT»");
                        break;
                    case '"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\u0000':
                        sb.Append("«NUL»");
                        break;
                    case '\u0001':
                        sb.Append("«SOH»");
                        break;
                    case '\u0002':
                        sb.Append("«STX»");
                        break;
                    case '\u0003':
                        sb.Append("«ETX»");
                        break;
                    case '\u0004':
                        sb.Append("«EOT»");
                        break;
                    case '\u0005':
                        sb.Append("«ENQ»");
                        break;
                    case '\u0006':
                        sb.Append("«ACK»");
                        break;
                    case '\u000E':
                        sb.Append("«SO»");
                        break;
                    case '\u000F':
                        sb.Append("«SI»");
                        break;
                    case '\u0010':
                        sb.Append("«DLE»");
                        break;
                    case '\u0011':
                        sb.Append("«DC1»");
                        break;
                    case '\u0012':
                        sb.Append("«DC2»");
                        break;
                    case '\u0013':
                        sb.Append("«DC3»");
                        break;
                    case '\u0014':
                        sb.Append("«DC4»");
                        break;
                    case '\u0015':
                        sb.Append("«NAK»");
                        break;
                    case '\u0016':
                        sb.Append("«SYN»");
                        break;
                    case '\u0017':
                        sb.Append("«ETB»");
                        break;
                    case '\u0018':
                        sb.Append("«CAN»");
                        break;
                    case '\u0019':
                        sb.Append("«EM»");
                        break;
                    case '\u001A':
                        sb.Append("«SUB»");
                        break;
                    case '\u001B':
                        sb.Append("«ESC»");
                        break;
                    case '\u001C':
                        sb.Append("«FS»");
                        break;
                    case '\u001D':
                        sb.Append("«CS»");
                        break;
                    case '\u001E':
                        sb.Append("«RS»");
                        break;
                    case '\u001F':
                        sb.Append("«US»");
                        break;
                    case ' ':
                        sb.Append(" ");
                        break;
                    case '\u0082':
                        sb.Append("«BPH»");
                        break;
                    case '\u0083':
                        sb.Append("«NBH»");
                        break;
                    case '\u0084':
                        sb.Append("«IND»");
                        break;
                    case '\u0085':
                        sb.Append("«NEL»");
                        break;
                    case '\u0086':
                        sb.Append("«SSA»");
                        break;
                    case '\u0087':
                        sb.Append("«ESA»");
                        break;
                    case '\u0088':
                        sb.Append("«HTS»");
                        break;
                    case '\u0089':
                        sb.Append("«HTJ»");
                        break;
                    case '\u008A':
                        sb.Append("«VTS»");
                        break;
                    case '\u008B':
                        sb.Append("«PLD»");
                        break;
                    case '\u008C':
                        sb.Append("«PLU»");
                        break;
                    case '\u008D':
                        sb.Append("«RI»");
                        break;
                    case '\u008E':
                        sb.Append("«SS2»");
                        break;
                    case '\u008F':
                        sb.Append("«SS3»");
                        break;
                    case '\u0090':
                        sb.Append("«DCS»");
                        break;
                    case '\u0091':
                        sb.Append("«PU1»");
                        break;
                    case '\u0092':
                        sb.Append("«PU2»");
                        break;
                    case '\u0093':
                        sb.Append("«STS»");
                        break;
                    case '\u0094':
                        sb.Append("«CCH»");
                        break;
                    case '\u0095':
                        sb.Append("«MW»");
                        break;
                    case '\u0096':
                        sb.Append("«SPA»");
                        break;
                    case '\u0097':
                        sb.Append("«EPA»");
                        break;
                    case '\u0098':
                        sb.Append("«SOS»");
                        break;
                    case '\u009A':
                        sb.Append("«SCI»");
                        break;
                    case '\u009B':
                        sb.Append("«CSI»");
                        break;
                    case '\u009C':
                        sb.Append("«ST»");
                        break;
                    case '\u009D':
                        sb.Append("«OCS»");
                        break;
                    case '\u009E':
                        sb.Append("«PM»");
                        break;
                    case '\u009F':
                        sb.Append("«APC»");
                        break;
                    case '\u00A0':
                        sb.Append("«NBSP»");
                        break;
                    case '\u2028':
                        sb.Append("«LSEP»");
                        break;
                    case '\u2029':
                        sb.Append("«PSEP»");
                        break;
                    case '«':
                        sb.Append("««»");
                        break;
                    case '»':
                        sb.Append("«»»");
                        break;
                    default:
                        if (Char.IsControl(c) || Char.IsWhiteSpace(c))
                            sb.Append("\\u").Append(((int)c).ToString("x4"));
                        else
                            sb.Append(c);
                        break;
                }
            }
            return sb.Append("\"").ToString();
        }
    }
}