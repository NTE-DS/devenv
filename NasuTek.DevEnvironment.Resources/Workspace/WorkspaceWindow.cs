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

using NasuTek.DevEnvironment.Extensibility.Addins;
using NasuTek.DevEnvironment.Extensibility.Addins.WinForms;
using NasuTek.DevEnvironment.Extensibility.Workbench;
using NasuTek.DevEnvironment.Workspace.Docking;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NasuTek.DevEnvironment.Workspace
{
    public partial class WorkspaceWindow : Form
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

       public ToolBarManager toolBarManager { get; private set; }

        public WorkspaceWindow()
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
            return (DevEnvPane)dockPanel1.Contents.FirstOrDefault(p => p is DevEnvPane && ((DevEnvPane)p).Name == paneId);
        }

        public void CloseAllDocuments() {
            dockPanel1.Documents.OfType<DevEnvDocument>().ToList().ForEach(x => x.Close());
        }

        public void RefreshDocuments() {
            dockPanel1.Documents.OfType<DevEnvDocument>().ToList().ForEach(x => x.RefreshDocument());
        }

        public void OpenDocument(DocumentMetadata documentMetadata) {
            var openedAlreadyObj = dockPanel1.Documents.OfType<DevEnvDocument>().FirstOrDefault(v => v.IsSameDocument(documentMetadata));
            if (openedAlreadyObj != null) {
                openedAlreadyObj.Show();
                return;
            }

            DevEnvDocument docFormat;

            if (AddInTree.ExistsTreeNode("/DevEnv/DocumentTypes")) {
                var docCodon = AddInTree.GetTreeNode("/DevEnv/DocumentTypes").Codons.FirstOrDefault(v => v.Id == documentMetadata.RequestedFormat);
                if (docCodon != null) {
                    docFormat = (DevEnvDocument) docCodon.AddIn.CreateObject(docCodon.Properties["class"]);
                    goto CreateDocFormat;
                }
            }

            if (!documentMetadata.IsFile) return;
            docFormat = new Documents.TextEditor();
            CreateDocFormat:
            docFormat.Show(dockPanel1, DockState.Document);
            docFormat.Open(documentMetadata);
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