using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NasuTek.DevEnvironment.Extendability.Workbench
{
    public interface ISolutionExplorerRightClick {
        ContextMenuStrip RightClick(TreeNode node);
    }
}
