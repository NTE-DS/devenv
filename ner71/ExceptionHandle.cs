using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace ner71
{
    [Serializable]
    public class ExceptionBinder
    {
        public Exception ExceptionObject { get; set; }
        public int ProcessID { get; set; }
        public string AppName { get; set; }
    }

    public class ExceptionHandle
    {
        static string appName;

        public static void AttachHandler()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        public static void SetAppName(string appName)
        {
            ExceptionHandle.appName = appName;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Handle((Exception)e.ExceptionObject);
        }

        private static void Handle(Exception e)
        {
            var eb = new ExceptionBinder
            {
                ExceptionObject = e,
                ProcessID = Process.GetCurrentProcess().Id,
                AppName = appName,
            };

            var bf = new BinaryFormatter();
            var tf = Path.GetTempFileName();

            using(var file = File.Open(tf, FileMode.Create))
            {
                bf.Serialize(file, eb);
            }

            var wt = Process.Start(Path.Combine(Application.StartupPath, "ner71.exe"), tf);
            wt.WaitForExit();
        }
    }
}
