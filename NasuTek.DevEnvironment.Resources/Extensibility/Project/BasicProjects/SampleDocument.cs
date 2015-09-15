using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NasuTek.DevEnvironment.Extendability.Workbench;
using NasuTek.DevEnvironment.Extendability;

namespace NasuTek.DevEnvironment.Extensibility.Project.BasicProjects
{
    public partial class SampleDocument : DevEnvDocument
    {
        public SampleDocument()
        {
            InitializeComponent();
        }

        public override void Open(DocumentMetadata openObj) {
            Text = openObj.FilePath;
            propertyGrid1.SelectedObject = openObj.DataObject;
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e) {
            Dirty = true;
        }

        public override bool IsSameDocument(DocumentMetadata newDocument) {
            return newDocument.FilePath == Text;
        }
    }
}
