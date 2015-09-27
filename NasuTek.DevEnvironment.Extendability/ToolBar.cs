using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NasuTek.DevEnvironment.Extensibility
{
    public class ToolBar
    {
        public string Name { get; private set; }
        public List<ToolBarItem> Items { get; private set; }

        public ToolBar(string name)
        {
            Name = name;
            Items = new List<ToolBarItem>();
        }
    }
}
