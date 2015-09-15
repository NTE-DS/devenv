using NasuTek.DevEnvironment.Extendability.Workbench;
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
            foreach (var i in DevEnv.Instance.Logs)
                comboBox1.Items.Add(i.Key);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshActiveLog(comboBox1.Text);
        }

        internal void RefreshActiveLog(string log)
        {
            if (comboBox1.Text != log) return;
            textBox1.Text = DevEnv.Instance.Logs[log].ToString();
            textBox1.SelectionStart = textBox1.TextLength;
            textBox1.ScrollToCaret();
        }
    }
}
