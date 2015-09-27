using NasuTek.DevEnvironment.Extensibility;
using NasuTek.DevEnvironment.Extensibility.Project;
using NasuTek.DevEnvironment.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NasuTek.DevEnvironment.MenuCommands
{
    public class NewProject : AbstractCommand
    {
        public override void Run()
        {
            var obj = (IProject)new NewObject(NewObject.ObjectType.Project).Create();
        }
    }

    public class NewFile : AbstractCommand
    {
        public override void Run()
        {
            var obj = (IObject)new NewObject(NewObject.ObjectType.FileNoAttach).Create();
        }
    }
}
