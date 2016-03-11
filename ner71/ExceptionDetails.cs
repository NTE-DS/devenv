using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ner71
{
    public partial class ExceptionDetails : Form
    {
        public ExceptionDetails(Exception e)
        {
            InitializeComponent();
            textBox1.Text = e.ToString();
        }
    }
}
