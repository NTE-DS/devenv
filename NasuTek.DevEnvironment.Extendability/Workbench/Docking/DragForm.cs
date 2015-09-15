/***************************************************************************************************
 * NasuTek Developer Studio Development Environment Core DLL
 * Copyright (C) 2005-2014 NasuTek Enterprises
 * Window Docking Portions Copyright (C) 2007-2012 Weifen Luo (email: weifenluo@yahoo.com)
 * Addin Engine Portions Copyright (C) 2001-2012 AlphaSierraPapa for the SharpDevelop Team
 *
 * This library is free software; you can redistribute it and/or modify it under the terms of the 
 * GNU Library General Public License as published by the Free Software Foundation; either
 * version 2 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU Library General Public License for more details.
 *
 * You should have received a copy of the GNU Library General Public License along with this 
 * library; if not, write to the Free Software Foundation, Inc., 59 Temple Place - Suite 330,
 * Boston, MA 02111-1307, USA.
 ***************************************************************************************************/

using System;
using System.Windows.Forms;

namespace NasuTek.DevEnvironment.Extendability.Workbench.Docking
{
    // Inspired by Chris Sano's article:
    // http://msdn.microsoft.com/smartclient/default.aspx?pull=/library/en-us/dnwinforms/html/colorpicker.asp
    // In Sano's article, the DragForm needs to meet the following criteria:
    // (1) it was not to show up in the task bar;
    //     ShowInTaskBar = false
    // (2) it needed to be the top-most window;
    //     TopMost = true (not necessary here)
    // (3) its icon could not show up in the ALT+TAB window if the user pressed ALT+TAB during a drag-and-drop;
    //     FormBorderStyle = FormBorderStyle.None;
    //     Create with WS_EX_TOOLWINDOW window style.
    //     Compares with the solution in the artile by setting FormBorderStyle as FixedToolWindow,
    //     and then clip the window caption and border, this way is much simplier.
    // (4) it was not to steal focus from the application when displayed.
    //     User Win32 ShowWindow API with SW_SHOWNOACTIVATE
    // In addition, this form should only for display and therefore should act as transparent, otherwise
    // WindowFromPoint will return this form, instead of the control beneath. Need BOTH of the following to
    // achieve this (don't know why, spent hours to try it out :( ):
    //  1. Enabled = false;
    //  2. WM_NCHITTEST returns HTTRANSPARENT
    internal class DragForm : Form
    {
        public DragForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            SetStyle(ControlStyles.Selectable, false);
            Enabled = false;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= (int)(Win32.WindowExStyles.WS_EX_NOACTIVATE | Win32.WindowExStyles.WS_EX_TOOLWINDOW);
                return createParams;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)Win32.Msgs.WM_NCHITTEST)
            {
                m.Result = (IntPtr)Win32.HitTest.HTTRANSPARENT;
                return;
            }

            base.WndProc(ref m);
        }
        //The form can be still activated by explicity calling Activate
        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }
        public virtual void Show(bool bActivate)
        {
                Show();

                if (bActivate)
                    Activate();
        }
    }
}
