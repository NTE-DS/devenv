using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NasuTek.DevEnvironment.Extensibility.Project;
using NasuTek.DevEnvironment.Extensibility;
using System.IO;

namespace NasuTek.DevEnvironment.Project.BasicProjects
{
    public class FolderProjectGenerator : IProjectGenerator
    {
        public IProject Open(string projectFilePath)
        {
            var proj = new FolderProject(projectFilePath);
            return proj;
        }
    }

    public class FolderProject : IProject
    {
        public string ProjectName { get; set; }

        public IFolder RootFolder { get; private set; }

        public FolderProject(string projectPath)
        {
            ProjectName = File.ReadAllLines(projectPath)[0];
            RootFolder = new FolderProjectFolder(Path.GetDirectoryName(projectPath));
        }

        public bool MoveObject(IObject objToMove, IFolder oldFolder, IFolder folderToMoveInto)
        {
            try
            {
                oldFolder.RemoveObject(objToMove);
                folderToMoveInto.AddObject(objToMove);
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }


        public bool MoveObject(IFolder folderToMove, IFolder oldFolder, IFolder folderToMoveInto)
        {
            try
            {
                oldFolder.RemoveFolder(folderToMove);
                folderToMoveInto.AddFolder(folderToMove);
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }


        public object[] PropertyObjects
        {
            get { return ProjectService.EmptyObjects; }
        }


        public void OpenObject(IObject objToOpen)
        {
            var docData = new DocumentMetadata { FilePath = ((FolderProjectObject)objToOpen).FilePath, RequestedFormat = objToOpen.DocumentType, IsFile = true };
            DevEnv.GetActiveInstance().WorkspaceEnvironment.OpenDocument(docData);
        }


        public void Save() { }

        public void SaveAs(string path) { }

        public DocumentMetadata OpenObjectAs(IObject objToOpen) {
            return new DocumentMetadata { FilePath = ((FolderProjectObject)objToOpen).FilePath, IsFile = true };
        }
    }

    public class FolderProjectFolder : IFolder
    {
        public FolderProjectFolder(string folderPath)
        {
            Name = Path.GetFileName(folderPath);
            FolderPath = folderPath;

            foreach(var fp in Directory.GetDirectories(folderPath))
            {
                m_Folders.Add(new FolderProjectFolder(fp));
            }

            foreach (var file in Directory.GetFiles(folderPath))
            {
                m_Objects.Add(new FolderProjectObject(file));
            }
        }

        private List<FolderProjectFolder> m_Folders = new List<FolderProjectFolder>();
        private List<FolderProjectObject> m_Objects = new List<FolderProjectObject>();
        private string m_FolderPath;

        public string Name { get; set; }
        public string FolderPath { get; set; }

        public IFolder[] SubFolders
        {
            get { return m_Folders.Cast<IFolder>().ToArray(); }
        }

        public IObject[] Objects
        {
            get { return m_Objects.Cast<IObject>().ToArray(); }
        }

        public void AddObject(IObject objectd)
        {
            if (m_Objects.Contains(objectd)) return;
            m_Objects.Add((FolderProjectObject)objectd);
        }

        public void AddFolder(IFolder folder)
        {
            if (m_Folders.Contains(folder)) return;
            m_Folders.Add((FolderProjectFolder)folder);
        }

        public void RemoveObject(IObject objectd)
        {
            if (!m_Objects.Contains(objectd)) throw new InvalidOperationException("Object does not exist.");
            m_Objects.Remove((FolderProjectObject)objectd);
        }

        public void RemoveFolder(IFolder folder)
        {
            if (!m_Folders.Contains(folder)) throw new InvalidOperationException("Object does not exist.");
            m_Folders.Remove((FolderProjectFolder)folder);
        }


        public object[] PropertyObjects
        {
            get { return ProjectService.EmptyObjects; }
        }
    }

    [DeletableObject]
    public class FolderProjectObject : IObject
    {
        public FolderProjectObject(string filePath)
        {
            FilePath = filePath;
            Name = Path.GetFileName(filePath);
        }

        public Guid DocumentType
        {
            get
            {
                return Guid.Parse("{E10C70C1-3507-45EC-99D2-151BA920D23A}");
            }
        }

        public string Name { get; set; }

        public object Object { get; set; }

        public string FilePath { get; set; }

        public object[] PropertyObjects
        {
            get
            {
                return ProjectService.EmptyObjects;
            }
        }
    }
}