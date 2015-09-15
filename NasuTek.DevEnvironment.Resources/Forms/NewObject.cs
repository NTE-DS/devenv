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
    public partial class NewObject : Form
    {
        public enum ObjectType
        {
            File,
            FileNoAttach,
            Project,
        }
        
        public NewObject(ObjectType ot)
        {
            InitializeComponent();
            switch (ot)
            {
                case ObjectType.FileNoAttach:
                    label2.Visible = false;
                    textBox1.Visible = false;
                    panel1.Visible = false;
                    Height = 443;
                    break;
                case ObjectType.File:
                    panel1.Visible = false;
                    Height = 475;
                    break;
            }
        }

        public object Create()
        {
            if (ShowDialog() != DialogResult.OK) return null;

            return null;
        }
    }
}
