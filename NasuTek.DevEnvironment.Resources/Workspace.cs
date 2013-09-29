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
using NasuTek.DevEnvironment.Resources.Addins.WinForms;
using NasuTek.DevEnvironment.Resources.Docking;
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
    public partial class Workspace : Form
    {
        public class BorderRenderer : ToolStripProfessionalRenderer
        {
            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(227, 227, 227)), e.AffectedBounds);
            }

            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                e.Graphics.DrawLine(new Pen(Color.FromArgb(227, 227, 227)), new Point(e.AffectedBounds.Left, e.AffectedBounds.Bottom), new Point(e.AffectedBounds.Right, e.AffectedBounds.Bottom));
            }
        }

        ToolBarManager toolBarManager;

        public Workspace()
        {
            if (AddInTree.ExistsTreeNode("/DevEnv/AutoExec/BeforeInitialization")) {
                foreach (Codon i in AddInTree.GetTreeNode("/DevEnv/AutoExec/BeforeInitialization").Codons) {
                    var obj = (ICommand)i.AddIn.CreateObject(i.Properties["class"]);
                    obj.Run();
                }
            }

            InitializeComponent();

            toolBarManager = new ToolBarManager(dockPanel1, this);
            //toolBarManager.AddControl(menuStrip1);

            MenuService.AddItemsToMenu(menuStrip1.Items, menuStrip1, "/DevEnv/Menu");

            //if(menuStrip1.Items)
            //{
                menuStrip1.MdiWindowListItem = menuStrip1.Items.Cast<ToolStripMenuItem>().FirstOrDefault(v=> v.Text == "Window");
            //}

            if (AddInTree.ExistsTreeNode("/DevEnv/Pads"))
            {
                foreach (Codon i in AddInTree.GetTreeNode("/DevEnv/Pads").Codons)
                {
                    var obj = (DevEnvPane)i.AddIn.CreateObject(i.Properties["class"]);
                    obj.Name = i.Id;
                    obj.Text = i.Properties["title"];
                    if (i.Properties["icon"] != "")
                        obj.Icon = ResourceService.GetImageResource(i.Properties["icon"]) is Icon ? (Icon)ResourceService.GetImageResource(i.Properties["icon"]) : Icon.FromHandle(((Bitmap)ResourceService.GetImageResource(i.Properties["icon"])).GetHicon());
                    obj.Show(dockPanel1, i.Properties["defaultPosition"] == "" ? DockState.Document : (DockState)Enum.Parse(typeof(DockState), i.Properties["defaultPosition"]));
                }
            }

            if (AddInTree.ExistsTreeNode("/DevEnv/Toolbars"))
            {
                var toolbars = ToolbarService.CreateToolbars(this, "/DevEnv/Toolbars");

                foreach (var toolbar in toolbars)
                {
                    toolbar.GripStyle = ToolStripGripStyle.Hidden;
                    toolbar.Renderer = new BorderRenderer();
                    
                    //toolbar.RenderMode = ToolStripRenderMode;
                    toolBarManager.AddControl(toolbar);
                }
            }
        }

        public DevEnvPane GetPane(string paneId) {
            return (DevEnvPane)dockPanel1.Contents.Where(p => p is DevEnvPane && ((DevEnvPane)p).Name == paneId).FirstOrDefault();
        }

        private void Workspace_Load(object sender, EventArgs e)
        {
            if (AddInTree.ExistsTreeNode("/DevEnv/AutoExec/AfterInitialization"))
            {
                foreach (Codon i in AddInTree.GetTreeNode("/DevEnv/AutoExec/AfterInitialization").Codons)
                {
                    var obj = (ICommand)i.AddIn.CreateObject(i.Properties["class"]);
                    obj.Run();
                }
            }

            var windows = this.MdiChildren;
        }

        public void ShowPane(DevEnvPane pane)
        { ShowPane(pane, DockState.Document); }
        public void ShowPane(DevEnvPane pane, DockState dockState)
        {
            pane.Show(dockPanel1, dockState);
        }

        private void menuStrip1_MouseUp(object sender, MouseEventArgs e) {

        }

        public DockPanel DockPanel {
            get { return dockPanel1; }
        }
    }
}