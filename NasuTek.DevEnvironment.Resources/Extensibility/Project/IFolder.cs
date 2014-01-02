using System;
using System.Collections.Generic;
using System.Linq;

namespace NasuTek.DevEnvironment.Extensibility.Project
{
    public interface IFolder {
        string Name { get; set; }
        IFolder[] SubFolders { get; }
        IObject[] Objects { get; }
        object[] PropertyObjects { get; }

        void AddObject(IObject objectd);
        void RemoveObject(IObject objectd);
        void AddFolder(IFolder folder);
        void RemoveFolder(IFolder folder);
    }
}
