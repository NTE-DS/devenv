using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NasuTek.DevEnvironment.Extensibility
{
    [Serializable]
    public class DevEnvSettings
    {
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
        public string ProductID { get; set; }
        public bool ShowIDEOnStartup { get; set; }
        public bool ExitThreadOnIDEExit { get; set; }

        public DevEnvSettings()
        {
            WindowIcon = Properties.Resources.DevEnvMain;
            ProductName = "NasuTek Development Environment";
            ProductVersionCodebase = new Version(DevEnvVersion.CodebaseVersion);
            ProductVersionRelease = new Version(DevEnvVersion.ReleaseVersion);
            ProductBuildStage = DevEnvVersion.BuildStage;
            ProductBuildLab = DevEnvVersion.BuildLab;
            ProductCopyrightYear = "2005";
            RegisteredUser = "Unregistered User";
            RegisteredCompany = "";
            ProductID = "DeveloperStudio";
            ShowIDEOnStartup = true;
            ExitThreadOnIDEExit = true;
        }

    }
}
