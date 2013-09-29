﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NasuTek.DevEnvironment.Resources.Addins;

namespace NasuTek.DevEnvironment.Resources
{
    public class CustomizeEnvironmentMenuItem : ICommand
    {
        public object Owner { get; set; }

        public void Run()
        {
            var customizeForm = new CustomizeEnvironment();
            if (customizeForm.ShowDialog() != DialogResult.OK) return;
        }

        public event EventHandler OwnerChanged;
    }
}
