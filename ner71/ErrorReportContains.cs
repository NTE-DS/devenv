using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ner71
{
    public partial class ErrorReportContains : Form
    {
        public ErrorReportContains(string binPath)
        {
            InitializeComponent();

            foreach (var i in Directory.GetFiles(binPath)) {
                textBox2.AppendText(i + Environment.NewLine);
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            Close();
        }
    }
}
