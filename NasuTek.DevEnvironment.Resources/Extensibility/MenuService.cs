using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MenuItem = NasuTek.DevEnvironment.Extensibility.MenuItem;

namespace NasuTek.DevEnvironment
{
    public partial class DevEnv
    {
        public partial class DEExtensibility
        {
            public void GenerateMenu(MenuStrip menu)
            {
                foreach (var i in MenuItems)
                {
                    var mi = new ToolStripMenuItem(i.Text);
                    mi.Owner = menu;
                    mi.Tag = i;
                    if (i.Type != MenuItem.MenuType.Menu)
                        mi.Click += Mi_Click;
                    menu.Items.Add(mi);

                    if (i.Type == MenuItem.MenuType.Menu)
                        GenerateMenu(menu, mi.DropDownItems, i);
                }
            }

            private void Mi_Click(object sender, EventArgs e)
            {
                var menuData = (MenuItem)((ToolStripMenuItem)sender).Tag;
                menuData.Command.Run();
            }

            public void GenerateMenu(MenuStrip menu, ToolStripItemCollection coll, MenuItem active)
            {
                foreach (var i in active.SubItems)
                {
                    if (i.Type == MenuItem.MenuType.Seperator)
                    {
                        coll.Add(new ToolStripSeparator());
                        continue;
                    }

                    var mi = new ToolStripMenuItem(i.Text);
                    mi.Owner = menu;
                    mi.Tag = i;
                    mi.Image = i.Icon;

                    if (i.Type != MenuItem.MenuType.Menu)
                        mi.Click += Mi_Click;
                    coll.Add(mi);

                    if (i.Type == MenuItem.MenuType.Menu)
                        GenerateMenu(menu, mi.DropDownItems, i);
                }
            }
        }
    }
}