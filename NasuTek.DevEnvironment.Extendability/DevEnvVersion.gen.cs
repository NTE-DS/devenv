using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NasuTek.DevEnvironment
{
	public partial class DevEnvVersion {
		public const string Codename = "Melinda";
		public const string GitBranch = "master";
		public const string GitRevision = "fe027333c37a";
		public const string DateStamp = "20170406_1719";
		public const string LabID = "ndemain";
		public const string BuildStage = "Retail";
		public const string BuildLab = GitBranch + "-" + GitRevision + "_" + Codename + "-" + LabID + "_" + DateStamp;
    }
}
