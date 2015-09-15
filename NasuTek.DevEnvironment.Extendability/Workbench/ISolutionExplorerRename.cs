using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NasuTek.DevEnvironment.Extendability.Workbench
{
    public interface ISolutionExplorerRename {
        bool Rename(string newName);
    }
}
