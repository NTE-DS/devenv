using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NasuTek.DevEnvironment.Extensibility.Project
{
    public class DefaultSolution : ISolution
    {
        private List<IProject> m_Projects = new List<IProject>();

        public IProject[] Projects
        {
            get
            {
                return m_Projects.ToArray();
            }
        }

        public string SolutionName
        {
            get; set;
        }

        public Version SolutionVersion
        {
            get
            {
                return new Version("1.0");
            }
        }

        public void AddProject(IProject project)
        {
            m_Projects.Add(project);
        }

        public void RemoveProject(IProject project)
        {
            m_Projects.Remove(project);
        }
    }
}
