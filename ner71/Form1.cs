using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ner71
{
    public partial class Form1 : Form
    {
        ExceptionBinder bdr;
        int top = 0;
        int left = 0;

        public Form1(ExceptionBinder binder)
        {
            InitializeComponent();

            Text = binder.AppName;

            List<Exception> ex = new List<Exception>();

            ExceptionRecurse(ex, binder.ExceptionObject);

            bdr = binder;

            foreach (var exception in ex)
            {
                var pb = new PictureBox { Height = 16, Width = 16, Left = (label2.Left + left), Top = (label2.Top + label2.Height) + top, Image = Properties.Resources.arrow, Tag = exception };
                pb.DoubleClick += Pb_DoubleClick;
                var label = new Label { Text = exception.Message, Left = (pb.Left + pb.Width), Top = pb.Top, AutoSize=true };
                top += pb.Height;
                left += pb.Width;

                Controls.Add(pb);
                Controls.Add(label);
            }

            Height += top + 16;
        }

        private void Pb_DoubleClick(object sender, EventArgs e)
        {
            new ExceptionDetails((Exception)((PictureBox)sender).Tag).ShowDialog();
        }

        private void ExceptionRecurse(List<Exception> ex, Exception exceptionObject)
        {
            ex.Add(exceptionObject);

            if (exceptionObject.InnerException != null)
                ExceptionRecurse(ex, exceptionObject.InnerException);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var jit = (string)Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\AeDebug").GetValue("Debugger");
            MessageBox.Show(jit.Replace("%ld", bdr.ProcessID.ToString()));
            Process.Start("cmd.exe", "/c " + jit.Replace("%ld", bdr.ProcessID.ToString()));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Media.SystemSounds.Hand.Play();
        }
    }
}
