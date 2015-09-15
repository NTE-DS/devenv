﻿using NasuTek.DevEnvironment.Extendability;
using NasuTek.DevEnvironment.Extendability;
using NasuTek.DevEnvironment.Extendability.Workbench;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NasuTek.DevEnvironment
{
    public partial class DevEnv
    {
        public partial class DEExtendability
        {
            public List<DevEnvPane> DevEnvPanes { get; private set; }
            public List<MenuItem> MenuItems { get; private set; }
            public List<Update> InstalledUpdates { get; private set; }
            public List<Product> InstalledProducts { get; private set; }
            internal List<PlugIn> PluginsInt { get; private set; }
            public Dictionary<Guid, Tuple<string, Type>> DocumentTypes { get; private set; }
            public Dictionary<string, List<AbstractCommand>> Commands { get; private set; }

            internal DEExtendability()
            {
                DevEnvPanes = new List<DevEnvPane>();
                PluginsInt = new List<PlugIn>();
                MenuItems = new List<MenuItem>();
                InstalledProducts = new List<Product>();
                InstalledUpdates = new List<Update>();
                DocumentTypes = new Dictionary<Guid, Tuple<string, Type>>();
                Commands = new Dictionary<string, List<AbstractCommand>>();

                Commands.Add("BeforeEnvironmentInitialized", new List<AbstractCommand>());
                Commands.Add("BeforeInitialization", new List<AbstractCommand>());
                Commands.Add("AfterInitialization", new List<AbstractCommand>());
                Commands.Add("Finalize", new List<AbstractCommand>());
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
