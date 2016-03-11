using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace NasuTek.DevEnvironment
{
    public class Ner71Api
    {
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

        public void SendObject(string name, object obj)
        {

        }
    }
}
