using System;
using System.Collections.Generic;
using System.Linq;

namespace NasuTek.DevEnvironment.Extensibility.Project
{
    public interface IProjectGenerator {
        IProject Open(string projectFilePath);
    }
}
