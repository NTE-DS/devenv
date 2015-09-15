using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NasuTek.DevEnvironment.Extendability
{
    public static class DevEnvSvc
    {
        static Dictionary<string, object> services = new Dictionary<string, object>();

        public static object GetService(string serviceId)
        {
            return services[serviceId];
        }

        public static void RegisterService(string serviceId, object svcObj)
        {
            services.Add(serviceId, svcObj);
        }
    }
}
