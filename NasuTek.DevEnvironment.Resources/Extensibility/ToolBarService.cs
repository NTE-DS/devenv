using NasuTek.DevEnvironment.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NasuTek.DevEnvironment
{
    public partial class DevEnv
    {
        public partial class DEExtensibility
        {
            public ToolStrip[] GenerateToolbars()
            {
                List<ToolStrip> tbs = new List<ToolStrip>();
                foreach (var i in Toolbars)
                {
                    var ts = new ToolStrip();
                    ts.GripStyle = ToolStripGripStyle.Hidden;
                    ts.Renderer = new Workbench.WorkspaceWindow.BorderRenderer();
                    ts.Name = i.Name;
                    ts.Text = i.Name;

                    foreach (var item in i.Items)
                    {
                        if (item.Type == ToolBarItem.ToolBarItemType.Button)
                        {
                            ts.Items.Add(new ToolStripButton(item.Text, item.Image, new EventHandler((sender, e) => { ((ToolBarItem)((ToolStripButton)sender).Tag).Command.Run(); })) { Tag = item, DisplayStyle = ToolStripItemDisplayStyle.Image });
                        }
                        else if (item.Type == ToolBarItem.ToolBarItemType.Seperator)
                        {
                            ts.Items.Add(new ToolStripSeparator());
                        }
                    }

                    tbs.Add(ts);
                }

                return tbs.ToArray();
            }
        }
    }
}