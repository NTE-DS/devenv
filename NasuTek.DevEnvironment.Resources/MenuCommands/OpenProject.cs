using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NasuTek.DevEnvironment.Extensibility;
using NasuTek.DevEnvironment.Extensibility.Project;
using NasuTek.DevEnvironment.Pads;

namespace NasuTek.DevEnvironment.MenuCommands {
    public class OpenProject : AbstractCommand {
        public override void Run() {
            var proj = new OpenFileDialog {Filter = ProjectService.FilterString};
            if (proj.ShowDialog() != DialogResult.OK) return;
            
            ((SolutionExplorer) DevEnv.GetActiveInstance().Extensibility.GetPane("SolutionExplorer")).Extension = Path.GetExtension(proj.FileName);
            ((SolutionExplorer)DevEnv.GetActiveInstance().Extensibility.GetPane("SolutionExplorer")).ActiveSolution = ProjectService.CreateTempSolutionFromProject(ProjectService.OpenProject(proj.FileName));
        }
    }

    public class OpenSolution : AbstractCommand {
        public override void Run() {
            var proj = new OpenFileDialog { Filter = "NasuTek Solution (*.nsl)|*.nsl" };
            if (proj.ShowDialog() != DialogResult.OK) return;

            ((SolutionExplorer)DevEnv.GetActiveInstance().Extensibility.GetPane("SolutionExplorer")).ActiveSolution = ProjectService.OpenSolution(proj.FileName);
        }
    }

    public class IntermediateMenu : AbstractCommand
    {
        public override void Run()
        {
            DevEnv.GetActiveInstance().Extensibility.DevEnvPanes.First(v => v.Name == "IntermediateWindow").Show(DevEnv.GetActiveInstance().WorkspaceEnvironment.DockPanel);
        }
    }

        public class SaveProject : AbstractCommand {
        public override void Run() {
        //    if (((SolutionExplorer)DevEnv.GetActiveInstance().Extensibility.GetPane("SolutionExplorer")).ActiveProject == null) return;

//            ((SolutionExplorer) DevEnv.GetActiveInstance().Extensibility.GetPane("SolutionExplorer")).ActiveProject.Save();
        }
    }

    public class SaveAsProject : AbstractCommand {
        public override void Run() {
    //        if (((SolutionExplorer) DevEnv.GetActiveInstance().Extensibility.GetPane("SolutionExplorer")).ActiveProject == null) return;
      //      var proj = new SaveFileDialog { Filter = ProjectService.SaveFilterString(((SolutionExplorer)DevEnv.GetActiveInstance().Extensibility.GetPane("SolutionExplorer")).Extension) };
        //    if (proj.ShowDialog() != DialogResult.OK) return;

          //  ((SolutionExplorer) DevEnv.GetActiveInstance().Extensibility.GetPane("SolutionExplorer")).ActiveProject.SaveAs(proj.FileName);
            //((SolutionExplorer) DevEnv.GetActiveInstance().Extensibility.GetPane("SolutionExplorer")).Extension = Path.GetExtension(proj.FileName);
        }
    }
}
