using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NasuTek.DevEnvironment.Extensibility
{
    public class DevEnvMarshal : MarshalByRefObject
    {
        public void InitializeDevEnv(DevEnvSettings settings, string[] args)
        {
            Debug.Write(AppDomain.CurrentDomain.FriendlyName);

            var defRes = AppDomain.CurrentDomain.Load("ntedevenv");
            defRes.GetType("NasuTek.DevEnvironment.DevEnv").GetMethod("InitializeNewDevEnv").Invoke(null, new object[] { settings, args });
        }
    }
}
