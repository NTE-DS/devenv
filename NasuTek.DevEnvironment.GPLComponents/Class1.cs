using NasuTek.DevEnvironment.Extendability;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NasuTek.DevEnvironment.GPLComponents
{
    public class Core : IPlugin
    {
        public void Load()
        {
            DevEnv.Instance.Extendability.InstalledProducts.Add(new Product("DevEnv GPL Components", "Includes any GPL components. Not required for base IDE functions.", null));
        }

        public void Unload()
        {
            throw new NotImplementedException();
        }
    }
}
