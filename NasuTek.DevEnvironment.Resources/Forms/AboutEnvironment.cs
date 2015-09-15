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

using NasuTek.DevEnvironment.Extendability;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NasuTek.DevEnvironment.Forms
{
    internal partial class AboutEnvironment : Form
    {
        public AboutEnvironment()
        {
            InitializeComponent();
            Text = "About " + DevEnv.Instance.ProductName;
            label1.Text = String.Format(label1.Text, new object[] { DevEnv.Instance.ProductName, DevEnv.Instance.ProductCopyrightYear, "2005" });
            label2.Text = String.Format(label2.Text, new object[] {DevEnv.Instance.ProductVersionRelease.ToString(2), new Version(DevEnvVersion.ReleaseVersion).ToString(2)});
            label4.Text = DevEnv.Instance.RegisteredUser + "\n" + DevEnv.Instance.RegisteredCompany;
            label8.Text = String.Format(label8.Text, DevEnv.Instance.ProductVersionCodebase + "-" + DevEnv.Instance.ProductBuildStage + " (" + DevEnv.Instance.ProductBuildLab + ")", DevEnvVersion.FullVersion + " (" + DevEnvVersion.BuildLab + ")");

            foreach(var prod in DevEnv.Instance.Extendability.InstalledProducts)
            {
                listBox1.Items.Add(prod);
            }

            foreach (var upd in DevEnv.Instance.Extendability.InstalledUpdates)
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

        }
    }
}
