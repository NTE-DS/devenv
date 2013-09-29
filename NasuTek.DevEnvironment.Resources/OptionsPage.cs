using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NasuTek.DevEnvironment.Resources
{
    public partial class OptionsPage : UserControl
    {
        public OptionsPage()
        {
            InitializeComponent();
            base.Size = new System.Drawing.Size(539, 417);
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public new Size Size
        {
            get { return base.Size; }
        }
    }
}
