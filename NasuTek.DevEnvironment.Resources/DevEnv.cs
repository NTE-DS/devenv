/***************************************************************************************************
 * NasuTek Developer Studio Development Environment Core DLL
 * Copyright (C) 2005-2014 NasuTek Enterprises
 * Window Docking Portions Copyright (C) 2007-2012 Weifen Luo (email: weifenluo@yahoo.com)
 * Addin Engine Portions Copyright (C) 2001-2012 AlphaSierraPapa for the SharpDevelop Team
 *
 * This library is free software; you can redistribute it and/or modify it under the terms of the 
 * GNU Library General Public License as published by the Free Software Foundation; either
 * version 2 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU Library General Public License for more details.
 *
 * You should have received a copy of the GNU Library General Public License along with this 
 * library; if not, write to the Free Software Foundation, Inc., 59 Temple Place - Suite 330,
 * Boston, MA 02111-1307, USA.
 ***************************************************************************************************/

using NasuTek.DevEnvironment.Extendability;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using NasuTek.DevEnvironment.Pads;
using NasuTek.DevEnvironment.Extensibility;
using Microsoft.Win32;
using NasuTek.DevEnvironment.Workbench;

namespace NasuTek.DevEnvironment {
    class DebugLog : TraceListener
    {
        public override void Write(string message)
        {
            DevEnv.Instance.LogWrite("DevEnv Application Log", message);
        }

        public override void WriteLine(string message)
        {
            DevEnv.Instance.LogWriteLine("DevEnv Application Log", message);
        }
    }

    public partial class DevEnv { 
        public static DevEnv Instance { get; private set; }

        public WorkspaceWindow WorkspaceEnvironment { get; private set; }
        public string ProductName { get; set; }
        public Icon WindowIcon { get; set; }
        public Version ProductVersionRelease { get; set; }
        public Version ProductVersionCodebase { get; set; }
        public string ProductBuildCode { get; set; }
        public string ProductBuildStage { get; set; }
        public string ProductCopyrightYear { get; set; }
        public string ProductBuildLab { get; set; }
        public string RegisteredUser { get; set; }
        public string RegisteredCompany { get; set; }
        public Arguments DevEnvArguments { get; private set; }
        internal Dictionary<string, StringBuilder> Logs { get; private set; }
        public bool ShowIDEOnStartup { get; set; }
        public bool ExitThreadOnIDEExit { get; set; }
        public IDevEnvReg DevEnvRegistry { get; set; }
        public DEExtendability Extendability { get; private set; }
        public bool EnvironmentInitialized { get; private set; }

        public DevEnv(string productId = "DeveloperStudio", string version = "7.1") {
            Instance = this;

            WindowIcon = Properties.Resources.DevEnvMain;
            ProductName = "NasuTek Development Environment";
            ProductVersionCodebase = new Version(DevEnvVersion.CodebaseVersion);
            ProductVersionRelease = new Version(DevEnvVersion.ReleaseVersion);
            ProductBuildStage = DevEnvVersion.BuildStage;
            ProductBuildLab = DevEnvVersion.BuildLab;
            ProductCopyrightYear = "2005";
            RegisteredUser = "Unregistered User";
            RegisteredCompany = "";
            Logs = new Dictionary<string, StringBuilder>();
            ShowIDEOnStartup = true;
            ExitThreadOnIDEExit = true;

#if DEBUG
            DevEnvRegistry = new DevEnvReg(productId, version + "-Debug");
#else
            DevEnvRegistry = new DevEnvReg(productId, version);
#endif

            Extendability = new DEExtendability();
        }

        public void RegisterOutputLog(string name)
        {
            Logs.Add(name, new StringBuilder());
            if (WorkspaceEnvironment != null && WorkspaceEnvironment.GetPane("Output") != null)
                ((OutputPad)WorkspaceEnvironment.GetPane("Output")).RefreshLogs();
        }

        public void UnregisterOutputLog(string name)
        {
            Logs.Remove(name);
            if (WorkspaceEnvironment != null && WorkspaceEnvironment.GetPane("Output") != null)
                ((OutputPad)WorkspaceEnvironment.GetPane("Output")).RefreshLogs();
        }

        public void LogWrite(string log, string message)
        {
            Logs[log].Append(message);
            if (WorkspaceEnvironment != null && WorkspaceEnvironment.GetPane("Output") != null)
                ((OutputPad)WorkspaceEnvironment.GetPane("Output")).RefreshActiveLog(log);
        }

        public void LogWriteLine(string log, string message)
        {
            Logs[log].AppendLine(message);
            if (WorkspaceEnvironment != null && WorkspaceEnvironment.GetPane("Output") != null)
                ((OutputPad)WorkspaceEnvironment.GetPane("Output")).RefreshActiveLog(log);
        }

        public void InitializeServices()
        {
            DevEnvSvc.RegisterService("UISvc", new UiSvc());
            DevEnvSvc.RegisterService("PluginSvc", new PluginSvc());
        }

        public void InitializeEnvironment(Arguments args)
        {
            foreach (var i in DevEnv.Instance.Extendability.Commands["BeforeEnvironmentInitialized"])
            {
                i.Run();
            }

            EnvironmentInitialized = true;
            DevEnvArguments = args;

            RegisterOutputLog("DevEnv Application Log");
            var log = new DebugLog();
            Debug.Listeners.Add(log);

            LoggingService.Info("Application start");
            
            LoggingService.Info("Loading AddInTree...");
            foreach(var i in DevEnvRegistry.OpenSubKey("Packages").GetSubKeyNames())
            {
                var addin = DevEnvRegistry.OpenSubKey("Packages").OpenSubKey(i);
                var guid = Guid.Parse(i);
                var name = (string)addin.GetValue(null);

                try
                {
                    var addInAssemb = Assembly.LoadFile(Path.Combine(Application.StartupPath, (string)addin.GetValue("Assembly")));
                    
                    var addinPlug = (IPlugin)addInAssemb.CreateInstance((string)addin.GetValue("PackageClass"));
                    var plugin = new PlugIn(guid, name, addinPlug);
                    addinPlug.Load();
                    Extendability.PluginsInt.Add(plugin);
                }
                catch (Exception e)
                {
                    MessageBox.Show(String.Format("Addin {1} '{0}' Could not load because of the following reason:\n\n{2}", name, guid, e.Message));
                }
            }

            foreach (var i in DevEnvRegistry.OpenSubKey("DocumentTypes").GetSubKeyNames())
            {
                var addin = DevEnvRegistry.OpenSubKey("DocumentTypes").OpenSubKey(i);
                var guid = Guid.Parse(i);
                var name = (string)addin.GetValue(null);


                var addInAssemb = AppDomain.CurrentDomain.GetAssemblies().First(v => v.GetName().FullName == (string)addin.GetValue("DocumentAssembly"));

                var addinPlug = addInAssemb.GetType((string)addin.GetValue("DocumentClass"));

                Extendability.DocumentTypes.Add(guid, Tuple.Create(name, addinPlug));
            }

            LoggingService.Info("Initializing Workbench...");
            // Workbench is our class from the base 
            // project, this method creates an instance
            // of the main form.
            WorkspaceEnvironment = new WorkspaceWindow { Text = ProductName, Icon = WindowIcon };

#if DEPROTOCOLSUPPORT
            //TODO: WebSchemes
#endif
            
            LoggingService.Info("Running application...");
            // Workbench.Instance is the instance of 
            // the main form, run the message loop.
            Application.Run(new WorkspaceEnvironmentContext(this));

            LoggingService.Info("Application shutdown");
        }
    }

    class WorkspaceEnvironmentContext : ApplicationContext
    {
        DevEnv environment;

        public WorkspaceEnvironmentContext(DevEnv env)
        {
            ThreadExit += WorkspaceEnvironmentContext_ThreadExit;
            if (env.ExitThreadOnIDEExit)
                env.WorkspaceEnvironment.FormClosed += WorkspaceEnvironment_FormClosed;

            if (env.ShowIDEOnStartup)
                env.WorkspaceEnvironment.Show();

            environment = env;
        }

        private void WorkspaceEnvironment_FormClosed(object sender, FormClosedEventArgs e)
        {
            ExitThread();
        }

        private void WorkspaceEnvironmentContext_ThreadExit(object sender, EventArgs e)
        {
            LoggingService.Info("Running finalize commands...");
            foreach (var i in DevEnv.Instance.Extendability.Commands["Finalize"])
            {
                i.Run();
            }
           
            //try
            //{
            //    // Save changed properties
            //    PropertyService.Save();
            //}
            //catch (Exception ex)
            //{
            //    MessageService.ShowException(ex, "Error storing properties");
            //}
        }
    }
}