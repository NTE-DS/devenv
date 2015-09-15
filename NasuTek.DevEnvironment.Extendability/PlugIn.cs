using NasuTek.DevEnvironment.Extendability;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NasuTek.DevEnvironment.Extendability
{
    public class PlugIn
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public IPlugin PluginObject { get; private set; }

        public PlugIn(Guid id, string name, IPlugin plugin)
        {
            Id = id;
            Name = name;
            PluginObject = plugin;
        }
    }
}
