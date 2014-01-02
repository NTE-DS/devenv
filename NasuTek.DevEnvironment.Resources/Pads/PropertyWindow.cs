using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NasuTek.DevEnvironment.Extensibility.Workbench;

namespace NasuTek.DevEnvironment.Pads
{
    public partial class PropertyWindow : DevEnvPane
    {
        public PropertyWindow()
        {
            InitializeComponent();
        }

        public void SetObjects(object[] objects) {
            comboBox1.Items.Clear();
            propertyGrid1.SelectedObject = null;

            comboBox1.Items.AddRange(objects);

            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            propertyGrid1.SelectedObject = comboBox1.SelectedItem;
        }
    }
}
