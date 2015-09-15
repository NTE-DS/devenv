using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NasuTek.DevEnvironment.Extendability
{
    public class Update
    {
        public string Name { get; private set; }

        public Update(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Product : Update
    {
        public string Description { get; private set; }
        public Image Icon { get; private set; }

        public Product(string name, string desc, Image icon) : base(name)
        {
            Description = desc;
            Icon = icon;
        }
    }
}
