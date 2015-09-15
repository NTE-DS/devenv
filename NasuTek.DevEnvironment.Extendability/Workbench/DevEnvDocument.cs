using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NasuTek.DevEnvironment.Extensibility.Project;
using NasuTek.DevEnvironment.Extendability.Workbench.Docking;

namespace NasuTek.DevEnvironment.Extendability.Workbench
{
    public partial class DevEnvDocument : DevEnvPane {
        public DevEnvDocument()
        {
            InitializeComponent();
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] public new DockAreas DockAreas {
            get { return DockAreas.Document; }
        }

        private bool m_Dirty;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] public bool Dirty { get { return m_Dirty; }
        set {
            if (value) {
                Text += " *";
            } else {
                Text = Text.Substring(0, Text.Length - 2);
            }

            m_Dirty = value;
        }}

        public virtual void Open(DocumentMetadata openObj) {}
        public virtual void Save() {}
        public virtual void SaveAs(string path) {}
        public virtual void RefreshDocument() {}
        public virtual bool IsSameDocument(DocumentMetadata newDocument) {
            return false;
        }
        public virtual bool IsSameDocument(object obj) {
            return false;
        }

        private void DevEnvDocument_FormClosing(object sender, FormClosingEventArgs e) {
            if (!Dirty) return;
            var retVal = MessageBox.Show("Do you want to save \"" + Text.Substring(0, Text.Length - 2) + "\"?", "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

            switch (retVal) {
                case DialogResult.OK:
                    Save();
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
            }
        }
    }
}
