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
    public class Mainform : Form
    {
        private readonly IContainer components;
        private readonly TabControl mainTabControl;
        private readonly BinaryCompareTabPage binaryCompareTabpage;
        private readonly TextCompareTabPage textCompareTabPage;

        public Mainform()
        {
            components = new Container();
            SuspendLayout();
            mainTabControl = new TabControl();
            mainTabControl.SuspendLayout();
            Controls.Add(mainTabControl);
            FileInfo[] files = Environment.GetCommandLineArgs().Skip(1).Select(f => new FileInfo(f)).ToArray();
            if (files.Length == 0)
                files = new FileInfo[] { new FileInfo(@"C:\Users\lerwi\Git\FileDataView\LICENSE"), new FileInfo(@"C:\Users\lerwi\Git\FileDataView\README.md") };
            binaryCompareTabpage = new BinaryCompareTabPage(files);
            textCompareTabPage = new TextCompareTabPage(files);
            mainTabControl.TabPages.Add(binaryCompareTabpage);
            mainTabControl.TabPages.Add(textCompareTabPage);
            mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            mainTabControl.Name = "mainTabControl";
            mainTabControl.SelectedIndex = 0;
            mainTabControl.TabIndex = 1;
            int tabIndex = 2;
            tabIndex = binaryCompareTabpage.InitializeComponents(tabIndex);
            tabIndex = textCompareTabPage.InitializeComponents(tabIndex);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1024, 450);
            Text = "File Data Viewer";
            binaryCompareTabpage.ResumeComponentLayout();
            textCompareTabPage.ResumeComponentLayout();
            mainTabControl.ResumeLayout(false);
            mainTabControl.PerformLayout();
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
