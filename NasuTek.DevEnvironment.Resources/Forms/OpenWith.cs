using NasuTek.DevEnvironment.Extensibility;
using NasuTek.DevEnvironment.Extensibility.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NasuTek.DevEnvironment.Forms
{
    public partial class OpenWith : Form
    {
        Dictionary<string, Guid> types = new Dictionary<string, Guid>();
        DocumentMetadata mtd;

        public OpenWith(DocumentMetadata obj)
        {
            InitializeComponent();

            foreach(var i in DevEnv.GetActiveInstance().Extensibility.DocumentTypes) {
                types.Add(i.Value.Item1, i.Key);
                listBox1.Items.Add(i.Value.Item1);
            }

            mtd = obj;
        }

        private void button1_Click(object sender, EventArgs e) {
            mtd.RequestedFormat = types[listBox1.Text];
            DialogResult = DialogResult.OK;
        }
    }
}
