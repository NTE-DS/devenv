using System;
using NasuTek.DevEnvironment.Extensibility;
using System.Windows.Forms;

namespace NasuTek.DevEnvironment
{
    internal class SampleCommand : AbstractCommand
    {
        public override void Run()
        {
            MessageBox.Show("HAI!");
        }
    }
}