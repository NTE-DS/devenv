﻿/***************************************************************************************************
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

using NasuTek.DevEnvironment.Resources.Addins;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NasuTek.DevEnvironment.Resources
{
    public partial class AboutEnvironment : Form
    {
        public AboutEnvironment()
        {
            InitializeComponent();
            Text = "About " + DevEnv.Instance.ProductName;
            label1.Text = String.Format(label1.Text, new object[] { DevEnv.Instance.ProductName, DevEnv.Instance.ProductCopyrightYear, "2005" });
            label2.Text = String.Format(label2.Text, new object[] { DevEnv.Instance.ProductVersion.ToString(3), DevEnvVersion.ShortVersion });
            label4.Text = DevEnv.Instance.RegisteredUser + "\n" + DevEnv.Instance.RegisteredCompany;

            foreach(AddIn i in AddInTree.AddIns.Where(a => a.IsPreinstalled)){
                listBox1.Items.Add(i.Name);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {
            if (listBox1.SelectedItem == null) return;
            var addIn = AddInTree.AddIns.Where(a => a.Name == listBox1.SelectedItem).First();
            textBox1.Text = addIn.Properties["description"];
            if (addIn.Properties["icon"] != "")
                pictureBox2.Image = ResourceService.GetImageResource(addIn.Properties["icon"]) is Icon ? new Icon((Icon)ResourceService.GetImageResource(addIn.Properties["icon"]), new Size(48,48)).ToBitmap() : (Bitmap)ResourceService.GetImageResource(addIn.Properties["icon"]);

        }
    }

    public class AboutDevEnv : AbstractMenuCommand {
        public override void Run() {
            new AboutEnvironment().ShowDialog();
        }
    }
}
