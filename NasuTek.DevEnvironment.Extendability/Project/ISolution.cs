using System;
using System.Collections.Generic;
using System.Linq;

namespace NasuTek.DevEnvironment.Extensibility.Project
{
    public interface ISolution
    {
        string SolutionName { get; set; }
        Version SolutionVersion { get; }
        IProject[] Projects { get; }

        void AddProject(IProject project);
        void RemoveProject(IProject project);
    }
}
