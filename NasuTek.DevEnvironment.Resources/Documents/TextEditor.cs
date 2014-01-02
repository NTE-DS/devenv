using System.IO;
using NasuTek.DevEnvironment.Extensibility.Workbench;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NasuTek.DevEnvironment.Documents
{
    public partial class TextEditor : DevEnvDocument {
        private string m_Path;

        public TextEditor()
        {
            InitializeComponent();
        }

        public override void Open(DocumentMetadata openObj) {
            Text = Path.GetFileName(openObj.FilePath);
            textEditorControl1.Text = File.ReadAllText(openObj.FilePath);
            m_Path = openObj.FilePath;
        }

        public override void Save() {
            File.WriteAllText(m_Path, textEditorControl1.Text);
        }

        public override void SaveAs(string path) {
            m_Path = path;
            Text = Path.GetFileName(m_Path);
            Save();
        }
    }
}
