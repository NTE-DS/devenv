using NasuTek.DevEnvironment.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NasuTek.DevEnvironment.Extensibility.Workbench;

namespace NasuTek.DevEnvironment.Svcs
{
    class UiSvc : IDevEnvUISvc
    {
        public void AddRootMenuItem(MenuItem menuItem)
        {
            DevEnv.GetActiveInstance().Extensibility.MenuItems.Add(menuItem);
        }

        public void AddToolbar(ToolBar toolbar)
        {
            DevEnv.GetActiveInstance().Extensibility.Toolbars.Add(toolbar);
        }

        public DevEnvPane GetPane(string id)
        {
            return DevEnv.GetActiveInstance().Extensibility.GetPane(id);
        }

        public MenuItem GetRootMenuItem(string id)
        {
            return DevEnv.GetActiveInstance().Extensibility.GetMenuItem(id);
        }

        public ToolBar GetToolBar(string name)
        {
            return DevEnv.GetActiveInstance().Extensibility.GetToolbar(name);
        }

        public void RegisterPane(DevEnvPane pane)
        {
            DevEnv.GetActiveInstance().Extensibility.DevEnvPanes.Add(pane);
        }
    }
}
