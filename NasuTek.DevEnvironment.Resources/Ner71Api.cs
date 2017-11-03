using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace NasuTek.DevEnvironment
{
    public class Ner71Api {
        private static string m_AppName;

        public static void Attach(string appName) {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
            m_AppName = appName;

            if (File.Exists(Path.Combine(Application.StartupPath, "ner71.exe"))) {
                var ner = new Ner71Api();
                ner.AttachHandler();
                ner.SetAppName(m_AppName);
            } else {
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
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

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
                return;

            MessageBox.Show("ner71.exe is missing, defaulting to backup exception handler\n\n" + e.ExceptionObject, m_AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
