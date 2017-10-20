using System;
using System.Collections.Generic;
using System.Linq;

namespace NasuTek.DevEnvironment.Extensibility.Project {
    public interface ISolution {
        string SolutionName { get; set; }
        string SolutionPath { get; }
        Version SolutionVersion { get; }
        ISolutionFolder RootFolder { get; }

        void Open(string solutionFilePath);
        void Save();
    }
}
