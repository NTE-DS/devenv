using NasuTek.DevEnvironment.Extensibility;
using NasuTek.DevEnvironment.Extensibility.Workbench;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NasuTek.DevEnvironment.Pads
{
    public partial class OutputPad : DevEnvPane
    {
        public OutputPad()
        {
            InitializeComponent();
            RefreshLogs();
        }

        internal void RefreshLogs()
        {
            comboBox1.Items.Clear();
            foreach (var i in DevEnv.GetActiveInstance().Logs)
                comboBox1.Items.Add(i.Key);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshActiveLog(comboBox1.Text);
        }

        internal void RefreshActiveLog(string log)
        {
            if (comboBox1.Text != log) return;
            textBox1.Text = DevEnv.GetActiveInstance().Logs[log].ToString();
            textBox1.SelectionStart = textBox1.TextLength;
            textBox1.ScrollToCaret();
        }
    }
}
