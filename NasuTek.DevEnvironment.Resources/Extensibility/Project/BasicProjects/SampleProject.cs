using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NasuTek.DevEnvironment.Extensibility.Project;

namespace NasuTek.DevEnvironment.ProjectAPI.BasicProjects
{
    public class SampleProjectGenerator : IProjectGenerator
    {
        public IProject Open(string projectFilePath) {
            var proj = new SampleProject {ProjectName = "Demo Project"};
            proj.RootFolder.AddFolder(new SampleProjectFolder {Name = "Demo Folder A"});
            proj.RootFolder.SubFolders.First(v => v.Name == "Demo Folder A").AddObject(new SampleProjectObject {Name = "Sample Object A"});
            proj.RootFolder.SubFolders.First(v => v.Name == "Demo Folder A").AddObject(new SampleProjectObject {Name = "Sample Object B"});
            proj.RootFolder.SubFolders.First(v => v.Name == "Demo Folder A").AddObject(new SampleProjectObject {Name = "Sample Object C"});
            proj.RootFolder.AddFolder(new SampleProjectFolder {Name = "Demo Folder B"});
            proj.RootFolder.SubFolders.First(v => v.Name == "Demo Folder B").AddObject(new SampleProjectObject {Name = "Sample Object D"});
            proj.RootFolder.SubFolders.First(v => v.Name == "Demo Folder B").AddObject(new SampleProjectObject {Name = "Sample Object E"});
            proj.RootFolder.SubFolders.First(v => v.Name == "Demo Folder B").AddObject(new SampleProjectObject {Name = "Sample Object F"});
            proj.RootFolder.AddFolder(new SampleProjectFolder {Name = "Demo Folder C"});
            proj.RootFolder.SubFolders.First(v => v.Name == "Demo Folder C").AddObject(new SampleProjectObject {Name = "Sample Object G"});
            proj.RootFolder.SubFolders.First(v => v.Name == "Demo Folder C").AddObject(new SampleProjectObject {Name = "Sample Object H"});
            proj.RootFolder.SubFolders.First(v => v.Name == "Demo Folder C").AddObject(new SampleProjectObject {Name = "Sample Object I"});
            return proj;
        }
    }

    public class SampleProject : IProject {
        public string ProjectName { get; set; }

        public IFolder RootFolder { get; private set; }

        public SampleProject() {
            RootFolder = new SampleProjectFolder();
        }

        public bool MoveObject(IObject objToMove, IFolder oldFolder, IFolder folderToMoveInto) {
            try {
                oldFolder.RemoveObject(objToMove);
                folderToMoveInto.AddObject(objToMove);
                return true;
            } catch (InvalidOperationException) {
                return false;
            }
        }


        public bool MoveObject(IFolder folderToMove, IFolder oldFolder, IFolder folderToMoveInto) {
            try {
                oldFolder.RemoveFolder(folderToMove);
                folderToMoveInto.AddFolder(folderToMove);
                return true;
            } catch (InvalidOperationException) {
                return false;
            }
        }


        public object[] PropertyObjects {
            get { return ProjectService.EmptyObjects; }
        }


        public void OpenObject(IObject objToOpen) {
            var docData = new DocumentMetadata {FilePath = objToOpen.Name, RequestedFormat = objToOpen.DocumentType};
            docData.Metadata.Add(((SampleProjectObject) objToOpen).Data);
            DevEnv.Instance.WorkspaceEnvironment.OpenDocument(docData);
        }


        public void Save() {}

        public void SaveAs(string path) {}
    }

    public class SampleProjectFolder : IFolder {
        public class SampleObjectSettings {
            public string DemoA { get; set; }
            public string DemoB { get; set; }
            public string DemoC { get; set; }
        }

        private List<SampleProjectFolder> m_Folders = new List<SampleProjectFolder>(); 
        private List<SampleProjectObject> m_Objects = new List<SampleProjectObject>();
        private SampleObjectSettings demo = new SampleObjectSettings();
        public string Name { get; set; }

        public IFolder[] SubFolders
        {
            get { return m_Folders.Cast<IFolder>().ToArray(); }
        }

        public IObject[] Objects
        {
            get { return m_Objects.Cast<IObject>().ToArray(); }
        }

        public void AddObject(IObject objectd) {
            if (m_Objects.Contains(objectd)) return;
            m_Objects.Add((SampleProjectObject) objectd);
        }

        public void AddFolder(IFolder folder) {
            if (m_Folders.Contains(folder)) return;
            m_Folders.Add((SampleProjectFolder) folder);
        }

        public void RemoveObject(IObject objectd) {
            if (!m_Objects.Contains(objectd)) throw new InvalidOperationException("Object does not exist.");
            m_Objects.Remove((SampleProjectObject) objectd);
        }

        public void RemoveFolder(IFolder folder) {
            if (!m_Folders.Contains(folder)) throw new InvalidOperationException("Object does not exist.");
            m_Folders.Remove((SampleProjectFolder) folder);
        }


        public object[] PropertyObjects
        {
            get { return new object[] { demo }; }
        }
    }

    [DeletableObject]
    public class SampleProjectObject : IObject
    {
        public class SampleObjectData
        {
            public string DemoA { get; set; }
            public string DemoB { get; set; }
            public string DemoC { get; set; }
        }

        public string Name { get; set; }
        public object Object { get; set; }
        public SampleObjectData Data { get; private set; }

        public object[] PropertyObjects
        {
            get { return ProjectService.EmptyObjects; }
        }

        public string DocumentType
        {
            get { return "SampleObject"; }
        }

        public SampleProjectObject() {
            Data = new SampleObjectData();
        }
    }
}
