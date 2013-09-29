using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NasuTek.DevEnvironment.Resources.Addins.WinForms;

namespace NasuTek.DevEnvironment.Resources
{
    public class WindowMenuBuilder : ISubmenuBuilder
    {
        public System.Windows.Forms.ToolStripItem[] BuildSubmenu(Addins.Codon codon, object owner) {
            var windowMenu = new ToolStripMenuItem("Window");

            var closeAllDocuments = new ToolStripMenuItem("C&lose All Documents");

            windowMenu.DropDownItems.Add(closeAllDocuments);
            windowMenu.DropDownItems.Add(new ToolBarSeparator());

            ((MenuStrip) owner).MdiWindowListItem = windowMenu;
            return new ToolStripItem[] {windowMenu};
        }
    }
}
