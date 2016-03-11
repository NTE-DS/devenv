using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using NasuTek.DevEnvironment;
using NasuTek.DevEnvironment.Extensibility;
using System.Threading;

namespace DevEnv {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            DevEnvSvc.InitializeDevEnv(new DevEnvSettings(), args, false);
        }
    }
}
