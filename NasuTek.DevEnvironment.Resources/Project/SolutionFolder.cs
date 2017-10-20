using NasuTek.DevEnvironment.Extensibility.Project;
using System.Collections.Generic;
using System;

namespace NasuTek.DevEnvironment.Project {
    public class SolutionFolder : ISolutionFolder {
        List<IProject> m_Projects = new List<IProject>();
        List<ISolutionFolder> m_SubFolders = new List<ISolutionFolder>();

        public IProject[] Projects { get { return m_Projects.ToArray(); } }
        public ISolutionFolder[] SubFolders { get { return m_SubFolders.ToArray(); } }
        public string Name { get; set; }

        public void AddProject(IProject project) {
            m_Projects.Add(project);
        }

        public ISolutionFolder CreateFolder(string name) {
            var newFolder = new SolutionFolder();
            newFolder.Name = name;
            m_SubFolders.Add(newFolder);
            return newFolder;
        }

        public void RemoveProject(IProject project) {
            m_Projects.Remove(project);
        }
    }
}