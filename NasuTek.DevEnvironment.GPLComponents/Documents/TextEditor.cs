using System.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NasuTek.DevEnvironment.Extensibility.Workbench;
using NasuTek.DevEnvironment.Extensibility;

namespace NasuTek.DevEnvironment.Documents
{
    public partial class TextEditor : DevEnvDocument {
        private string m_Path;
        private bool isFile;

        public TextEditor()
        {
            InitializeComponent();
        }

        public override void Open(DocumentMetadata openObj) {
            if (openObj.IsFile)
            {
                isFile = true;

                Text = Path.GetFileName(openObj.FilePath);
                textEditorControl1.Text = File.ReadAllText(openObj.FilePath);
                m_Path = openObj.FilePath;
            }
        }

        public override void Save() {
            if (isFile)
                File.WriteAllText(m_Path, textEditorControl1.Text);
        }

        public override void SaveAs(string path) {
            if (isFile)
            {
                m_Path = path;
                Text = Path.GetFileName(m_Path);
                Save();
            }
        }
    }
}
