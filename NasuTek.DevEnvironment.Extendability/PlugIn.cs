using NasuTek.DevEnvironment.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NasuTek.DevEnvironment.Extensibility
{
    public class PlugIn
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public IPackage PluginObject { get; private set; }

        public PlugIn(Guid id, string name, IPackage plugin)
        {
            Id = id;
            Name = name;
            PluginObject = plugin;
        }
    }
}
