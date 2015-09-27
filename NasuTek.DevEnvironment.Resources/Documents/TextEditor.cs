using NasuTek.DevEnvironment.Extensibility.Workbench;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NasuTek.DevEnvironment.Extensibility;

namespace NasuTek.DevEnvironment.Documents
{
    public partial class TextEditor : DevEnvDocument
    {
        public TextEditor()
        {
            InitializeComponent();
        }

        public override void Open(DocumentMetadata openObj)
        {
            if (openObj.IsFile)
                richTextBox1.LoadFile(openObj.FilePath);
            else
                richTextBox1.Text = openObj.DataObject.ToString();
        }
    }
}
