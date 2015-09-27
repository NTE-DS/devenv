using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NasuTek.DevEnvironment.Extensibility;
using NasuTek.DevEnvironment.Forms;

namespace NasuTek.DevEnvironment.MenuCommands
{
    public class AboutDevEnv : AbstractCommand
    {
        public override void Run()
        {
            new AboutEnvironment().ShowDialog();
        }
    }

}
