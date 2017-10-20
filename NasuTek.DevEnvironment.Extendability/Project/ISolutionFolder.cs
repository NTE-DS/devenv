using System.Xml.Linq;

namespace NasuTek.DevEnvironment.Extensibility.Project {
    public interface ISolutionFolder {
        IProject[] Projects { get; }
        ISolutionFolder[] SubFolders { get; }
        string Name { get; set; }

        void AddProject(IProject project);
        void RemoveProject(IProject project);

        ISolutionFolder CreateFolder(string name);
    }
}