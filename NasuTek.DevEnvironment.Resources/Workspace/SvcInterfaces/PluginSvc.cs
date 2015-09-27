using NasuTek.DevEnvironment.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NasuTek.DevEnvironment.Svcs
{
    class PluginSvc : IDevEnvPackageSvc
    {
        public void AddProduct(Product prod)
        {
            DevEnv.GetActiveInstance().Extensibility.InstalledProducts.Add(prod);
        }

        public void AddUpdate(Update upd)
        {
            DevEnv.GetActiveInstance().Extensibility.InstalledUpdates.Add(upd);
        }

        public void AttachCommand(string commGroup, AbstractCommand command)
        {
            if (!DevEnv.GetActiveInstance().Extensibility.Commands.ContainsKey(commGroup))
                DevEnv.GetActiveInstance().Extensibility.Commands.Add(commGroup, new List<AbstractCommand>());

            DevEnv.GetActiveInstance().Extensibility.Commands[commGroup].Add(command);
        }

        public void UnloadablePlugin(IPackage plugin)
        {
            DevEnv.GetActiveInstance().Extensibility.UnloadablePlugins.Add(plugin);
        }
    }
}
