using NasuTek.DevEnvironment.Extensibility;
using NasuTek.DevEnvironment.Extensibility;
using NasuTek.DevEnvironment.MenuCommands;
using NasuTek.DevEnvironment.Pads;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NasuTek.DevEnvironment
{
    public class DevEnvCore : IPackage
    {
        public void Load()
        {
            var plugSvc = (IDevEnvPackageSvc)DevEnvSvc.GetService(DevEnvSvc.PackageSvc);
            var uiSvc = (IDevEnvUISvc)DevEnvSvc.GetService(DevEnvSvc.UISvc);

            plugSvc.AddProduct(new Product("NasuTek Development Environment", "NasuTek Development Environment", null));
            plugSvc.AddUpdate(new Update("NasuTek DevEnv R2 Addin Engine Update"));

            // Load Pads
            uiSvc.RegisterPane(new OutputPad());
            uiSvc.RegisterPane(new PropertyWindow());
            uiSvc.RegisterPane(new SolutionExplorer());
            uiSvc.RegisterPane(new TaskList());
            uiSvc.RegisterPane(new IntermediateWindow());

            // Create Standard Toolbar
            var standardTb = new ToolBar("Standard");
            uiSvc.AddToolbar(standardTb);

#if DEBUG
            // Create Tester Toolbar
            var debugTb = new ToolBar("IDE Tester Toolbar");
            uiSvc.AddToolbar(debugTb);
#endif

            // Create Basic Menu
            uiSvc.AddRootMenuItem(new MenuItem("File", "File", null));
            var fileMenu = uiSvc.GetRootMenuItem("File");
            fileMenu.SubItems.Add(new MenuItem("New", "New", new NewProject()));
            fileMenu.SubItems.Add(new MenuItem("Seperator2", null, null));
            fileMenu.SubItems.Add(new MenuItem("Open", "Open", null));
            fileMenu.GetMenuItem("Open").SubItems.Add(new MenuItem("Project", "Project", new OpenProject()));
            fileMenu.GetMenuItem("Open").SubItems.Add(new MenuItem("Solution", "Solution", new OpenSolution()));
            fileMenu.SubItems.Add(new MenuItem("Save", "Save", new SaveAsProject()));
            fileMenu.SubItems.Add(new MenuItem("SaveAs", "Save As", new SaveAsProject()));
            fileMenu.SubItems.Add(new MenuItem("Seperator2", null, null));
            fileMenu.SubItems.Add(new MenuItem("Exit", "Exit", new ExitDevEnv()));
            uiSvc.AddRootMenuItem(new MenuItem("Edit", "Edit", null));
            uiSvc.AddRootMenuItem(new MenuItem("View", "View", null));
            var viewMenu = uiSvc.GetRootMenuItem("View");
            viewMenu.SubItems.Add(new MenuItem("Windows", "Windows", null));
            var windowViewMenu = viewMenu.GetMenuItem("Windows");
            windowViewMenu.SubItems.Add(new MenuItem("CommandWindow", "Command Window...", new IntermediateMenu()));
            uiSvc.AddRootMenuItem(new MenuItem("Tools", "Tools", null));
            var toolsMenu = uiSvc.GetRootMenuItem("Tools");
            toolsMenu.SubItems.Add(new MenuItem("Customize", "Customize", new CustomizeEnvironmentMenuItem()));
            toolsMenu.SubItems.Add(new MenuItem("Options", "Options", new EnvironmentOptionsMenuItem()));
            toolsMenu.SubItems.Add(new MenuItem("MacrosIDE", "Macros Designer", new MacrosIDE()));
            uiSvc.AddRootMenuItem(new MenuItem("Window", "Window", null));
            uiSvc.AddRootMenuItem(new MenuItem("Help", "Help", null));
            var helpMenu = uiSvc.GetRootMenuItem("Help");
            helpMenu.SubItems.Add(new MenuItem("About", "About NasuTek Development Environment", new AboutDevEnv()));
        }
    }
}
