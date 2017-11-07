using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace ner71
{
    public class Ner71Api {

        public static void Attach(string appName, Version appVersion) {
            if (Debugger.IsAttached)
                return;

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);

            if (File.Exists(Path.Combine(Application.StartupPath, "ner71.exe"))) {
                var ner = new Ner71Api();
                ner.SetAppName(appName);
                ner.SetAppVersion(appVersion);
                ner.AttachHandler();
            } else {
                AppDomain.CurrentDomain.UnhandledException += (sender, args) => {
                    MessageBox.Show("ner71.exe is missing, defaulting to backup exception handler\n\n" + args.ExceptionObject, appName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                };
            }
        }

        Assembly nerAssembly;

        public Ner71Api()
        {
            nerAssembly = Assembly.LoadFile(Path.Combine(Application.StartupPath, "ner71.exe"));
        }

        public void AttachHandler()
        {
            nerAssembly.GetType("ner71.ExceptionHandle").GetMethod("AttachHandler").Invoke(null, new object[] { });
        }

        public void SetAppName(string appName)
        {
            nerAssembly.GetType("ner71.ExceptionHandle").GetMethod("SetAppName").Invoke(null, new object[] { appName });
        }

        public void SetAppVersion(Version appVersion)
        {
            nerAssembly.GetType("ner71.ExceptionHandle").GetMethod("SetAppVersion").Invoke(null, new object[] { appVersion });
        }
    }
}
