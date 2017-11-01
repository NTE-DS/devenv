using NasuTek.DevEnvironment.Extensibility;
using NasuTek.DevEnvironment.Extensibility;
using NasuTek.DevEnvironment.Extensibility.Workbench;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NasuTek.DevEnvironment
{
    public partial class DevEnv
    {
        public partial class DEExtensibility
        {
            public List<DevEnvPane> DevEnvPanes { get; private set; }
            public List<MenuItem> MenuItems { get; private set; }
            public List<Update> InstalledUpdates { get; private set; }
            public List<Product> InstalledProducts { get; private set; }
            internal List<PlugIn> PluginsInt { get; private set; }
            public List<ToolBar> Toolbars { get; private set; }
            public Dictionary<Guid, Tuple<string, Type>> DocumentTypes { get; private set; }
            public Dictionary<string, List<AbstractCommand>> Commands { get; private set; }
            public List<IPackage> UnloadablePlugins { get; private set; }
#if DEPROTOCOLSUPPORT
            public List<IProtocol> WebProtocols { get; private set; }
#endif

            internal ToolBar GetToolbar(string name)
            {
                return Toolbars.FirstOrDefault(v => v.Name == name);
            }

            internal DEExtensibility()
            {
                DevEnvPanes = new List<DevEnvPane>();
                PluginsInt = new List<PlugIn>();
                MenuItems = new List<MenuItem>();
                InstalledProducts = new List<Product>();
                InstalledUpdates = new List<Update>();
                DocumentTypes = new Dictionary<Guid, Tuple<string, Type>>();
                Commands = new Dictionary<string, List<AbstractCommand>>();
                Toolbars = new List<ToolBar>();
                UnloadablePlugins = new List<IPackage>();
#if DEPROTOCOLSUPPORT
                WebProtocols = new List<IProtocol>();
#endif

                Commands.Add("BeforeEnvironmentInitialized", new List<AbstractCommand>());
                Commands.Add("BeforeInitialization", new List<AbstractCommand>());
                Commands.Add("AfterInitialization", new List<AbstractCommand>());
                Commands.Add("Finalize", new List<AbstractCommand>());
                Commands.Add("OnInstall", new List<AbstractCommand>());
            }

            public DevEnvPane GetPane(string id)
            {
                return DevEnvPanes.FirstOrDefault(v => v.Name == id);
            }

            public MenuItem GetMenuItem(string id)
            {
                return MenuItems.FirstOrDefault(v => v.Id == id);
            }

            public int GetMenuPosition(string id)
            {
                return MenuItems.FindIndex(v => v.Id == id);
            }
        }
    }
}
