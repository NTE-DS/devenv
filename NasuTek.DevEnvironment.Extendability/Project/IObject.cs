using System;
using System.Collections.Generic;
using System.Linq;

namespace NasuTek.DevEnvironment.Extensibility.Project
{
    public interface IObject
    {
        string Name { get; set; }
        object Object { get; set; }
        object[] PropertyObjects { get; }
        Guid DocumentType { get; }
    }
}
