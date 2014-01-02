using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NasuTek.DevEnvironment.Resources;
using NasuTek.DevEnvironment.Resources.Addins;
using NasuTek.DevEnvironment.Resources.ProjectAPI;

namespace NasuTek.DevEnvironment
{
    public class Class1 : AbstractCommand
    {
        public override void Run() {
            ((SolutionExplorer)DevEnv.Instance.WorkspaceEnvironment.GetPane("SolutionExplorer")).SetActiveProject(ProjectService.OpenProject("", "SampleProjectGenerator"));
        }
    }
}
