/***************************************************************************************************
 * NasuTek Developer Studio
 * Copyright (C) 2005-2013 NasuTek Enterprises
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
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
