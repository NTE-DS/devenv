using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NasuTek.DevEnvironment.Extendability
{
    public interface IPlugin
    {
        void Load();
        void Unload();
    }
}
