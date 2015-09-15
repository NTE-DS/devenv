using NasuTek.DevEnvironment.Extendability;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NasuTek.DevEnvironment.Extendability.Workbench;

namespace NasuTek.DevEnvironment.Extensibility
{
    class UiSvc : IDevEnvUISvc
    {
        public void AddRootMenuItem(MenuItem menuItem)
        {
            DevEnv.Instance.Extendability.MenuItems.Add(menuItem);
        }

        public DevEnvPane GetPane(string id)
        {
            throw new NotImplementedException();
        }

        public MenuItem GetRootMenuItem(string id)
        {
            return DevEnv.Instance.Extendability.GetMenuItem(id);
        }

        public void RegisterPane(DevEnvPane pane)
        {
            DevEnv.Instance.Extendability.DevEnvPanes.Add(pane);
        }
    }
}
