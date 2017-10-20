using NasuTek.DevEnvironment.Extensibility.Project;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NasuTek.DevEnvironment.Project
{
    public class DefaultSolution : ISolution {
        public DefaultSolution() {
            RootFolder = new SolutionFolder();
            RootFolder.Name = "ROOT";
        }

        public string SolutionName {
            get; set;
        }

        public Version SolutionVersion {
            get {
                return new Version("7.1");
            }
        }

        public ISolutionFolder RootFolder { get; private set; }

        public string SolutionPath { get; private set; }

        public void Open(string solutionFilePath) {
            SolutionPath = solutionFilePath;

            var solution = XDocument.Load(SolutionPath);
            SolutionName = solution.Element("DevEnvSolution").Attribute("name").Value;

            string actFolder = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(Path.GetDirectoryName(SolutionPath));

            LoadProjects(solution.Element("DevEnvSolution"), RootFolder, Path.GetDirectoryName(SolutionPath));

            LoadFolders(solution.Element("DevEnvSolution"), RootFolder, Path.GetDirectoryName(SolutionPath));

            Directory.SetCurrentDirectory(actFolder);
        }

        private static void LoadFolders(XElement xElement, ISolutionFolder rootFolder, string rootPath) {
            foreach (var folder in xElement.Elements("SolutionFolder")) {
                ISolutionFolder folderObj = rootFolder.CreateFolder(folder.Attribute("name").Value);
                LoadProjects(folder, folderObj, rootPath);

                foreach (var subFolder in folder.Elements("SolutionFolder")) {
                    ISolutionFolder subFolderObj = folderObj.CreateFolder(subFolder.Attribute("name").Value);
                    LoadProjects(subFolder, subFolderObj, rootPath);

                    LoadFolders(subFolder, subFolderObj, rootPath);
                }
            }
        }

        private static void LoadProjects(XElement xElement, ISolutionFolder folder, string rootPath) {
            foreach (var proj in xElement.Elements("Project")) {
                folder.AddProject(ProjectService.OpenProject(Path.GetFullPath(proj.Attribute("file").Value)));
            }
        }

        public void Save() {
            throw new NotImplementedException();
        }
    }
}
