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
    public partial class IntermediateWindow : DevEnvPane
    {
        public IntermediateWindow()
        {
            InitializeComponent();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                try {
                    textBox1.AppendText("> " + textBox2.Text + "\n");
                    e.Handled = true;
                    var tld = textBox2.Text.Split(' ').ToList();
                    var cmd = tld[0];
                    tld.RemoveAt(0);

                    switch (cmd)
                    {
                        case "OpenWindow":
                            DevEnv.Instance.Extendability.DevEnvPanes.First(v => v.Name == tld[0]);
                            break;
                        case "SaveWindowSettings":
                            DevEnv.Instance.WorkspaceEnvironment.DockPanel.SaveAsXml(String.Join(" ", tld.ToArray()));
                            break;
                    }

                }
                catch(Exception ed)
                {
                    textBox1.AppendText(ed.ToString() + "\n");
                }
                textBox2.Text = "";
            }
        }
    }
}
