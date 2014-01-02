using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NasuTek.DevEnvironment
{
	public partial class DevEnvVersion {
		public const string Codename = "Melissa";
		public const string GitBranch = "master";
		public const string GitRevision = "cf8079f1b061";
		public const string DateStamp = "20131229_0004";
		public const string LabID = "lab06_devenvinterop";
		public const string BuildLab = GitBranch + "-" + GitRevision + "_" + Codename + "-" + LabID + "_" + DateStamp;
    }
}
