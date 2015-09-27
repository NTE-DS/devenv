/***************************************************************************************************
 * NasuTek Developer Studio Development Environment Core DLL
 * Copyright (C) 2005-2014 NasuTek Enterprises
 * Window Docking Portions Copyright (C) 2007-2012 Weifen Luo (email: weifenluo@yahoo.com)
 * Addin Engine Portions Copyright (C) 2001-2012 AlphaSierraPapa for the SharpDevelop Team
 *
 * This library is free software; you can redistribute it and/or modify it under the terms of the 
 * GNU Library General Public License as published by the Free Software Foundation; either
 * version 2 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU Library General Public License for more details.
 *
 * You should have received a copy of the GNU Library General Public License along with this 
 * library; if not, write to the Free Software Foundation, Inc., 59 Temple Place - Suite 330,
 * Boston, MA 02111-1307, USA.
 ***************************************************************************************************/

using NasuTek.DevEnvironment.Extensibility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NasuTek.DevEnvironment.Forms
{
    internal partial class AboutEnvironment : Form
    {
        public AboutEnvironment()
        {
            InitializeComponent();
            Text = "About " + DevEnv.GetActiveInstance().Settings.ProductName;
            label1.Text = String.Format(label1.Text, new object[] { DevEnv.GetActiveInstance().Settings.ProductName, DevEnv.GetActiveInstance().Settings.ProductCopyrightYear, "2005" });
            label2.Text = String.Format(label2.Text, new object[] {DevEnv.GetActiveInstance().Settings.ProductVersionRelease.ToString(2), new Version(DevEnvVersion.ReleaseVersion).ToString(2)});
            label4.Text = DevEnv.GetActiveInstance().Settings.RegisteredUser + "\n" + DevEnv.GetActiveInstance().Settings.RegisteredCompany;
            label8.Text = String.Format(label8.Text, DevEnv.GetActiveInstance().Settings.ProductVersionCodebase + "-" + DevEnv.GetActiveInstance().Settings.ProductBuildStage + " (" + DevEnv.GetActiveInstance().Settings.ProductBuildLab + ")", DevEnvVersion.FullVersion + " (" + DevEnvVersion.BuildLab + ")");

            foreach(var prod in DevEnv.GetActiveInstance().Extensibility.InstalledProducts)
            {
                listBox1.Items.Add(prod);
            }

            foreach (var upd in DevEnv.GetActiveInstance().Extensibility.InstalledUpdates)
            {
                listBox2.Items.Add(upd);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {
            if (listBox1.SelectedItem == null) return;

            var prod = (Product)listBox1.SelectedItem;
            textBox1.Text = prod.Description;
            pictureBox2.Image = prod.Icon;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fs = new StringBuilder();

            fs.AppendLine(String.Format(@"{0}
Copyright © {1}-2016 NasuTek Enterprises

NasuTek Developer Studio
Copyright © {2}-2016 NasuTek Enterprises

{3}
", DevEnv.GetActiveInstance().Settings.ProductName, DevEnv.GetActiveInstance().Settings.ProductCopyrightYear, "2005", label8.Text));

            fs.AppendLine("Installed Products:");
            foreach (Product i in listBox1.Items)
            {
                fs.AppendLine(i.Name);
                fs.AppendLine(i.Description);
                fs.AppendLine();
            }

            fs.AppendLine("Installed Updates:");
            foreach (Update i in listBox2.Items)
            {
                fs.AppendLine(i.Name);
            }

            Clipboard.SetText(fs.ToString());
        }
    }
}
