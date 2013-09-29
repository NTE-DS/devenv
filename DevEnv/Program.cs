﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using NasuTek.DevEnvironment.Resources;

namespace DevEnv {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var devEnv = new NasuTek.DevEnvironment.Resources.DevEnv();
            devEnv.InitializeEnvironment(new Arguments(args));
        }
    }
}
