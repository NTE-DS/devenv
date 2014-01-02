using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NasuTek.DevEnvironment.Extensibility.Addins;
using NasuTek.DevEnvironment.Forms;

namespace NasuTek.DevEnvironment.MenuCommands
{
    public class AboutDevEnv : AbstractMenuCommand
    {
        public override void Run()
        {
            new AboutEnvironment().ShowDialog();
        }
    }

}
