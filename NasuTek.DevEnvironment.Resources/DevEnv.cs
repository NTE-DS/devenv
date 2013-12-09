/***************************************************************************************************
 * NasuTek Developer Studio
 * Copyright (C) 2005-2013 NasuTek Enterprises
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 ***************************************************************************************************/

using NasuTek.DevEnvironment.Resources.Addins;
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

namespace NasuTek.DevEnvironment.Resources {
    public class DevEnv {
        public static DevEnv Instance { get; private set; }

        public Workspace WorkspaceEnvironment { get; private set; }
        public string ProductName { get; set; }
        public Icon WindowIcon { get; set; }
        public Version ProductVersion { get; set; }
        public string ProductCopyrightYear { get; set; }
        public string RegisteredUser { get; set; }
        public string RegisteredCompany { get; set; }
        public Dictionary<string, object> DevEnvStorage { get; private set; }
        public Arguments DevEnvArguments { get; private set; }

        public DevEnv() {
            Instance = this;

            ProductName = "NasuTek Development Environment";
            ProductVersion = new Version(DevEnvVersion.FullVersion);
            ProductCopyrightYear = "2005";
            RegisteredUser = "Unregistered User";
            RegisteredCompany = "";
            DevEnvStorage = new Dictionary<string, object>();
        }

        public void InitializeEnvironment(Arguments args) {
            DevEnvArguments = args;

            LoggingService.Info("Application start");

            // Get a reference to the entry assembly (Startup.exe)
            Assembly exe = typeof (DevEnv).Assembly;

            // Set the root path of our application. 
            // ICSharpCode.Core looks for some other
            // paths relative to the application root:
            // "data/resources" for language resources, 
            // "data/options" for default options
            FileUtility.ApplicationRootPath = Path.GetDirectoryName(exe.Location);

            LoggingService.Info("Starting core services...");

            // CoreStartup is a helper class 
            // making starting the Core easier.
            // The parameter is used as the application 
            // name, e.g. for the default title of
            // MessageService.ShowMessage() calls.
            CoreStartup coreStartup = new CoreStartup(ProductName);
            // It is also used as default storage 
            // location for the application settings:
            // "%Application Data%\%Application Name%", but you 
            // can override that by setting c.ConfigDirectory

            // Specify the name of the application settings 
            // file (.xml is automatically appended)
            coreStartup.PropertiesName = "AppProperties";

            // Initializes the Core services 
            // (ResourceService, PropertyService, etc.)
            coreStartup.StartCoreServices();

            // Registeres the default (English) strings 
            // and images. They are compiled as
            // "EmbeddedResource" into Startup.exe.
            // Localized strings are automatically 
            // picked up when they are put into the
            // "data/resources" directory.
            ResourceService.RegisterNeutralStrings(
                new ResourceManager("NasuTek.DevEnvironment.Resources.StringResources", exe));
            ResourceService.RegisterNeutralImages(
                new ResourceManager("NasuTek.DevEnvironment.Resources.ImageResources", exe));

            LoggingService.Info("Looking for AddIns...");
            // Searches for ".addin" files in the 
            // application directory.
            coreStartup.AddAddInsFromDirectory(
                Path.Combine(FileUtility.ApplicationRootPath, "AddIns"));

            if (args["DisableUserAddons"] != "true") {
                // Searches for a "AddIns.xml" in the user 
                // profile that specifies the names of the
                // AddIns that were deactivated by the 
                // user, and adds "external" AddIns.
                coreStartup.ConfigureExternalAddIns(
                    Path.Combine(PropertyService.ConfigDirectory, "AddIns.xml"));

                // Searches for AddIns installed by the 
                // user into his profile directory. This also
                // performs the job of installing, 
                // uninstalling or upgrading AddIns if the user
                // requested it the last time this application was running.
                coreStartup.ConfigureUserAddIns(
                    Path.Combine(PropertyService.ConfigDirectory, "AddInInstallTemp"),
                    Path.Combine(PropertyService.ConfigDirectory, "AddIns"));
            }

            LoggingService.Info("Loading AddInTree...");
            // Now finally initialize the application. 
            // This parses the ".addin" files and
            // creates the AddIn tree. It also 
            // automatically runs the commands in
            // "/Workspace/Autostart"
            coreStartup.RunInitialization();

            LoggingService.Info("Initializing Workbench...");
            // Workbench is our class from the base 
            // project, this method creates an instance
            // of the main form.
            WorkspaceEnvironment = new Workspace {Text = ProductName, Icon = WindowIcon};

            if (AddInTree.ExistsTreeNode("/DevEnv/WebSchemes")) {
                foreach (Codon i in AddInTree.GetTreeNode("/DevEnv/WebSchemes").Codons) {
                    var obj = (IProtocol) i.AddIn.CreateObject(i.Properties["class"]);
                    Protocol.RegisterProtocol(i.Properties["scheme"], obj);
                }
            }

            try {
                LoggingService.Info("Running application...");
                // Workbench.Instance is the instance of 
                // the main form, run the message loop.
                Application.Run(WorkspaceEnvironment);
            } finally {
                LoggingService.Info("Running finalize commands...");
                foreach (ICommand command in AddInTree.BuildItems<ICommand>("/DevEnv/FinalizeCommands", null, false)) {
                    try {
                        command.Run();
                    } catch (Exception ex) {
                        // allow startup to continue if some commands fail
                        MessageService.ShowException(ex);
                    }
                }
                try {
                    // Save changed properties
                    PropertyService.Save();
                } catch (Exception ex) {
                    MessageService.ShowException(ex, "Error storing properties");
                }
            }
            LoggingService.Info("Application shutdown");
        }
    }
}