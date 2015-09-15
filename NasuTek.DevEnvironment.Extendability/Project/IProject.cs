using System;
using System.Collections.Generic;
using System.Linq;

namespace NasuTek.DevEnvironment.Extensibility.Project
{
    public interface IProject
    {
        string ProjectName { get; set; }
        IFolder RootFolder { get; }
        object[] PropertyObjects { get; }

        bool MoveObject(IObject objToMove, IFolder oldFolder, IFolder folderToMoveInto);
        bool MoveObject(IFolder folderToMove, IFolder oldFolder, IFolder folderToMoveInto);

        void OpenObject(IObject objToOpen);

        void Save();
        void SaveAs(string path);
    }
}
