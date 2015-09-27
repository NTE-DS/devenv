using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NasuTek.DevEnvironment.Extensibility
{
    public static class DevEnvSvc
    {
        /// <summary>
        /// Access the DevEnv UI Services ((IDevEnvUISvc)
        /// </summary>
        public const string UISvc = "UISvc";

        /// <summary>
        /// Access the DevEnv Package Services (IDevEnvPackageSvc)
        /// </summary>
        public const string PackageSvc = "PackageSvc";

        /// <summary>
        /// Access the DevEnv Settings Registry (IDevEnvRegSvc)
        /// </summary>
        public const string RegSvc = "RegSvc";

        /// <summary>
        /// Access the Solution/Project Services (IDevEnvSolutionSvc)
        /// </summary>
        public const string SolutionSvc = "SolutionSvc";

        public const string LoggingSvc = "LoggingSvc";

        public const string CmdBeforeEnvironmentInitialized = "BeforeEnvironmentInitialized";
        public const string CmdFinalize = "Finalize";
        public const string CmdBeforeInitialization = "BeforeInitialization";
        public const string CmdAfterInitialization = "AfterInitialization";

        public const string DefaultDomain = "DefaultDomain";
        /// <summary>
        /// This service allows access to the underlying DevEnv object. It is not recommended to use this service as
        /// the IDevEnvXXXSvc Interfaces give easier access to DevEnv internal functions. Commands in a DevEnv class
        /// are not promised to be in future versions as they are private API's
        /// </summary>
        public const string DevEnvObject = "DevEnvObject";

        static Dictionary<string, Dictionary<string, object>> services = new Dictionary<string, Dictionary<string, object>>();

        public static object GetService(string serviceId)
        {
            return GetService(DefaultDomain, serviceId);
        }

        public static void RegisterService(string serviceId, object svcObj)
        {
            RegisterService(DefaultDomain, serviceId, svcObj);
        }

        public static object GetService(string domain, string serviceId)
        {
            return services[domain][serviceId];
        }

        public static void RegisterService(string domain, string serviceId, object svcObj)
        {
            if (!services.ContainsKey(domain))
                services.Add(domain, new Dictionary<string, object>());

            services[domain].Add(serviceId, svcObj);
        }

        public static Tuple<AppDomain, DevEnvMarshal> InitializeDevEnv(DevEnvSettings settings, string[] args)
        {
            return InitializeDevEnv(settings, args, true);
        }
        public static Tuple<AppDomain, DevEnvMarshal> InitializeDevEnv(DevEnvSettings settings, string[] args, bool seperateAppDomain)
        {
            if (seperateAppDomain)
            {
                var def = AppDomain.CreateDomain(settings.ProductID + "-DevEnvAD", null, new AppDomainSetup { ApplicationBase = Application.StartupPath });

                var init = (DevEnvMarshal)def.CreateInstanceAndUnwrap("devenvapi", "NasuTek.DevEnvironment.Extensibility.DevEnvMarshal");
                init.InitializeDevEnv(settings, args);

                return Tuple.Create(def, init);
            }
            else
            {
                var init = (DevEnvMarshal)AppDomain.CurrentDomain.CreateInstanceAndUnwrap("devenvapi", "NasuTek.DevEnvironment.Extensibility.DevEnvMarshal");
                init.InitializeDevEnv(settings, args);

                return Tuple.Create(AppDomain.CurrentDomain, init);
            }
        }
    }
}
