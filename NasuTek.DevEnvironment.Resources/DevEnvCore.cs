using NasuTek.DevEnvironment.Extendability;
using NasuTek.DevEnvironment.Extendability;
using NasuTek.DevEnvironment.MenuCommands;
using NasuTek.DevEnvironment.Pads;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NasuTek.DevEnvironment
{
    public class DevEnvCore : IPlugin
    {
        public void Load()
        {
            var plugSvc = (IDevEnvPluginSvc)DevEnvSvc.GetService("PluginSvc");
            var uiSvc = (IDevEnvUISvc)DevEnvSvc.GetService("UISvc");

            plugSvc.AddProduct(new Product("NasuTek Development Environment", "NasuTek Development Environment", new Icon(Properties.Resources.DevEnvMain, new Size(48, 48)).ToBitmap()));
            plugSvc.AddUpdate(new Update("NasuTek DevEnv R2 Addin Engine Update"));

            // Load Pads
            uiSvc.RegisterPane(new OutputPad());
            uiSvc.RegisterPane(new PropertyWindow());
            uiSvc.RegisterPane(new SolutionExplorer());
            uiSvc.RegisterPane(new TaskList());
            uiSvc.RegisterPane(new IntermediateWindow());

            uiSvc.AddRootMenuItem(new MenuItem("File", "File", null));
            var fileMenu = uiSvc.GetRootMenuItem("File");
            fileMenu.SubItems.Add(new MenuItem("New", "New", null));
            fileMenu.SubItems.Add(new MenuItem("Seperator2", null, null));
            fileMenu.SubItems.Add(new MenuItem("Open", "Open", new OpenProject()));
            fileMenu.SubItems.Add(new MenuItem("Save", "Save", new SaveAsProject()));
            fileMenu.SubItems.Add(new MenuItem("SaveAs", "Save As", new SaveAsProject()));
            fileMenu.SubItems.Add(new MenuItem("Seperator2", null, null));
            fileMenu.SubItems.Add(new MenuItem("Exit", "Exit", new ExitDevEnv()));
            uiSvc.AddRootMenuItem(new MenuItem("Edit", "Edit", null));
            uiSvc.AddRootMenuItem(new MenuItem("View", "View", null));
            var viewMenu = uiSvc.GetRootMenuItem("View");
            viewMenu.SubItems.Add(new MenuItem("IntermediateWindow", "Intermediate...", new IntermediateMenu()));
            uiSvc.AddRootMenuItem(new MenuItem("Tools", "Tools", null));
            var toolsMenu = uiSvc.GetRootMenuItem("Tools");
            toolsMenu.SubItems.Add(new MenuItem("Customize", "Customize", new CustomizeEnvironmentMenuItem()));
            toolsMenu.SubItems.Add(new MenuItem("Options", "Options", new EnvironmentOptionsMenuItem()));
            uiSvc.AddRootMenuItem(new MenuItem("Window", "Window", null));
            uiSvc.AddRootMenuItem(new MenuItem("Help", "Help", null));
            var helpMenu = uiSvc.GetRootMenuItem("Help");
            helpMenu.SubItems.Add(new MenuItem("About", "About NasuTek Development Environment", new AboutDevEnv()));
        }

        public void Unload()
        {
            throw new NotImplementedException();
        }
    }
}
