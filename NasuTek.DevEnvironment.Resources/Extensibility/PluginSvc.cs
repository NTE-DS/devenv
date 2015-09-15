using NasuTek.DevEnvironment.Extendability;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NasuTek.DevEnvironment.Extensibility
{
    class PluginSvc : IDevEnvPluginSvc
    {
        public void AddProduct(Product prod)
        {
            DevEnv.Instance.Extendability.InstalledProducts.Add(prod);
        }

        public void AddUpdate(Update upd)
        {
            DevEnv.Instance.Extendability.InstalledUpdates.Add(upd);
        }

        public void AttachCommand(string commGroup, AbstractCommand command)
        {
            if (DevEnv.Instance.Extendability.Commands.ContainsKey(commGroup))
                DevEnv.Instance.Extendability.Commands.Add(commGroup, new List<AbstractCommand>());

            DevEnv.Instance.Extendability.Commands[commGroup].Add(command);
        }

        public void ChangeDevEnvRegistry(IDevEnvReg reg)
        {
            if (DevEnv.Instance.EnvironmentInitialized)
                throw new Exception("DevEnvRegistry cannot be changed if the DevEnv was already initialized.");

            DevEnv.Instance.DevEnvRegistry = reg;
        }
    }
}
