using NasuTek.DevEnvironment.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NasuTek.DevEnvironment.GPLComponents
{
    public class Core : IPackage
    {
        public void Load()
        {
            var plugSvc = (IDevEnvPackageSvc)DevEnvSvc.GetService(DevEnvSvc.PackageSvc);
            var uiSvc = (IDevEnvUISvc)DevEnvSvc.GetService(DevEnvSvc.UISvc);

            plugSvc.AddProduct(new Product("DevEnv LGPL Components", "Includes any LGPL components. Not required for base IDE functions.", null));
        }
    }
}
