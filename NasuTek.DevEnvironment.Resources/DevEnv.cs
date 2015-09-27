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

using NasuTek.DevEnvironment.Extensibility;
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
using NasuTek.DevEnvironment.Svcs;

namespace NasuTek.DevEnvironment
{
    class DebugLog : TraceListener
    {
        public override void Write(string message)
        {
            DevEnv.GetActiveInstance().LogWrite("DevEnv Application Log", message);
        }

        public override void WriteLine(string message)
        {
            DevEnv.GetActiveInstance().LogWriteLine("DevEnv Application Log", message);
        }
    }

    public partial class DevEnv
    {
        public static void InitializeNewDevEnv(DevEnvSettings settings, string[] args)
        {
            var devEnv = new DevEnv() { Settings = settings };
            devEnv.InitializeServices();
            devEnv.InitializeEnvironment(new Arguments(args));
        }

        public DevEnvSettings Settings { get; set; }
        public WorkspaceWindow WorkspaceEnvironment { get; private set; }
        public Arguments DevEnvArguments { get; private set; }
        internal Dictionary<string, StringBuilder> Logs { get; private set; }
        public DEExtensibility Extensibility { get; private set; }
        public bool EnvironmentInitialized { get; private set; }
        public byte[] ActiveWorkbenchSettings { get; internal set; }

        public DevEnv()
        {
            Logs = new Dictionary<string, StringBuilder>();
            Extensibility = new DEExtensibility();
            ActiveWorkbenchSettings = Encoding.UTF8.GetBytes(Properties.Resources.InitialUi);
        }

        public void RegisterOutputLog(string name)
        {
            Logs.Add(name, new StringBuilder());
            if (WorkspaceEnvironment != null && Extensibility.GetPane("Output") != null)
                ((OutputPad)Extensibility.GetPane("Output")).RefreshLogs();
        }

        public void UnregisterOutputLog(string name)
        {
            Logs.Remove(name);
            if (WorkspaceEnvironment != null && Extensibility.GetPane("Output") != null)
                ((OutputPad)Extensibility.GetPane("Output")).RefreshLogs();
        }

        public void LogWrite(string log, string message)
        {
            Logs[log].Append(message);
            if (WorkspaceEnvironment != null && Extensibility.GetPane("Output") != null)
                ((OutputPad)Extensibility.GetPane("Output")).RefreshActiveLog(log);
        }

        public void LogWriteLine(string log, string message)
        {
            Logs[log].AppendLine(message);
            if (WorkspaceEnvironment != null && Extensibility.GetPane("Output") != null)
                ((OutputPad)Extensibility.GetPane("Output")).RefreshActiveLog(log);
        }

        public void InitializeServices()
        {
#if DEBUG
            DevEnvSvc.RegisterService(DevEnvSvc.RegSvc, new DevEnvReg(Settings.ProductID, Settings.ProductVersionCodebase.ToString(2) + "-Debug"));
#else
            DevEnvSvc.RegisterService(DevEnvSvc.RegSvc, new DevEnvReg(ProductID, ProductVersionCodebase.ToString(2)));
#endif      
            DevEnvSvc.RegisterService(DevEnvSvc.UISvc, new UiSvc());
            DevEnvSvc.RegisterService(DevEnvSvc.PackageSvc, new PluginSvc());
            DevEnvSvc.RegisterService(DevEnvSvc.LoggingSvc, new LoggingSvc());
            DevEnvSvc.RegisterService(DevEnvSvc.DevEnvObject, this);
        }

        public void InitializeEnvironment(Arguments args)
        {
            foreach (var i in Extensibility.Commands["BeforeEnvironmentInitialized"])
            {
                i.Run();
            }

            EnvironmentInitialized = true;
            DevEnvArguments = args;

            RegisterOutputLog("DevEnv Application Log");
            var log = new DebugLog();
            Debug.Listeners.Add(log);

            LogInfo("Application start");

            LogInfo("Loading AddInTree...");
            var regSvc = (IDevEnvRegSvc)DevEnvSvc.GetService(DevEnvSvc.RegSvc);

            if (regSvc.SubKeyExists(SettingsReg.Global, "Packages"))
                foreach (var addin in regSvc.OpenSubKey(SettingsReg.Global, "Packages").GetSubKeys())
                {
                    var guid = Guid.Parse(addin.Name);
                    var name = (string)addin.GetValue(null);

                    try
                    {
                        var addInAssemb = Assembly.Load((string)addin.GetValue("Assembly"));

                        if (addin.GetValue("PackageDependencies") != null)
                            foreach (var dep in (string[])addin.GetValue("PackageDependencies"))
                            {
                                Assembly.Load(dep);
                            }

                        var addinPlug = (IPackage)addInAssemb.CreateInstance((string)addin.GetValue("PackageClass"));
                        var plugin = new PlugIn(guid, name, addinPlug);
                        addinPlug.Load();
                        Extensibility.PluginsInt.Add(plugin);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(String.Format("Addin {1} '{0}' Could not load because of the following reason:\n\n{2}", name, guid, e.Message));
                    }
                }

            if (regSvc.SubKeyExists(SettingsReg.Global, "DocumentTypes"))
                foreach (var addin in regSvc.OpenSubKey(SettingsReg.Global, "DocumentTypes").GetSubKeys())
                {
                    var guid = Guid.Parse(addin.Name);
                    var name = (string)addin.GetValue(null);


                    var addInAssemb = AppDomain.CurrentDomain.GetAssemblies().First(v => v.GetName().FullName == (string)addin.GetValue("DocumentAssembly"));

                    var addinPlug = addInAssemb.GetType((string)addin.GetValue("DocumentClass"));

                    Extensibility.DocumentTypes.Add(guid, Tuple.Create(name, addinPlug));
                }

            LogInfo("Initializing Workbench...");
            // Workbench is our class from the base 
            // project, this method creates an instance
            // of the main form.
            WorkspaceEnvironment = new WorkspaceWindow { Text = Settings.ProductName, Icon = Settings.WindowIcon };

#if DEPROTOCOLSUPPORT
            if (regSvc.SubKeyExists(SettingsReg.Global, "WebSchemes"))
                foreach (var addin in regSvc.OpenSubKey(SettingsReg.Global, "WebSchemes").GetSubKeys())
                {
                    var guid = Guid.Parse(addin.Name);
                    var name = (string)addin.GetValue(null);
                    var scheme = (string)addin.GetValue("Scheme");

                    var addInAssemb = AppDomain.CurrentDomain.GetAssemblies().First(v => v.GetName().FullName == (string)addin.GetValue("WebSchemeAssembly"));
                    var addinPlug = (IProtocol)addInAssemb.CreateInstance((string)addin.GetValue("WebSchemeClass"));

                    Protocol.RegisterProtocol(scheme, addinPlug);
                    Extensibility.WebProtocols.Add(addinPlug);
                }
#endif

            LogInfo("Running application...");
            // Workbench.Instance is the instance of 
            // the main form, run the message loop.
            Application.Run(new WorkspaceEnvironmentContext(this));

            LogInfo("Application shutdown");
        }

        internal void LogInfo(string v)
        {
            var logSvc = (IDevEnvLoggingSvc)DevEnvSvc.GetService(DevEnvSvc.LoggingSvc);
            logSvc.Info(v);
        }

        internal static DevEnv GetActiveInstance()
        {
            return ((DevEnv)DevEnvSvc.GetService(DevEnvSvc.DevEnvObject));
        }
    }

    class WorkspaceEnvironmentContext : ApplicationContext
    {
        DevEnv environment;

        public WorkspaceEnvironmentContext(DevEnv env)
        {
            ThreadExit += WorkspaceEnvironmentContext_ThreadExit;
            if (env.Settings.ExitThreadOnIDEExit)
                env.WorkspaceEnvironment.FormClosed += WorkspaceEnvironment_FormClosed;

            if (env.Settings.ShowIDEOnStartup)
                env.WorkspaceEnvironment.Show();

            environment = env;
        }

        private void WorkspaceEnvironment_FormClosed(object sender, FormClosedEventArgs e)
        {
            ExitThread();
        }

        private void WorkspaceEnvironmentContext_ThreadExit(object sender, EventArgs e)
        {
            environment.LogInfo("Running finalize commands...");
            foreach (var i in DevEnv.GetActiveInstance().Extensibility.Commands["Finalize"])
            {
                i.Run();
            }

            environment.WorkspaceEnvironment.SaveWorkbenchData();

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