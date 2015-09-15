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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace NasuTek.DevEnvironment.Extendability.Workbench.Docking
{
    partial class DockPanel
    {
        /// <summary>
        /// DragHandlerBase is the base class for drag handlers. The derived class should:
        ///   1. Define its public method BeginDrag. From within this public BeginDrag method,
        ///      DragHandlerBase.BeginDrag should be called to initialize the mouse capture
        ///      and message filtering.
        ///   2. Override the OnDragging and OnEndDrag methods.
        /// </summary>
        private abstract class DragHandlerBase : NativeWindow, IMessageFilter
        {
            protected DragHandlerBase()
            {
            }

            protected abstract Control DragControl
            {
                get;
            }

            private Point m_startMousePosition = Point.Empty;
            protected Point StartMousePosition
            {
                get { return m_startMousePosition; }
                private set { m_startMousePosition = value; }
            }

            protected bool BeginDrag()
            {
                // Avoid re-entrance;
                lock (this)
                {
                    if (DragControl == null)
                        return false;

                    StartMousePosition = Control.MousePosition;

                    if (!Win32Helper.IsRunningOnMono)
                    if (!NativeMethods.DragDetect(DragControl.Handle, StartMousePosition))
                        return false;

                    DragControl.FindForm().Capture = true;
                    AssignHandle(DragControl.FindForm().Handle);
                    Application.AddMessageFilter(this);
                    return true;
                }
            }

            protected abstract void OnDragging();

            protected abstract void OnEndDrag(bool abort);

            private void EndDrag(bool abort)
            {
                ReleaseHandle();
                Application.RemoveMessageFilter(this);
                DragControl.FindForm().Capture = false;

                OnEndDrag(abort);
            }

            bool IMessageFilter.PreFilterMessage(ref Message m)
            {
                if (m.Msg == (int)Win32.Msgs.WM_MOUSEMOVE)
                    OnDragging();
                else if (m.Msg == (int)Win32.Msgs.WM_LBUTTONUP)
                    EndDrag(false);
                else if (m.Msg == (int)Win32.Msgs.WM_CAPTURECHANGED)
                    EndDrag(true);
                else if (m.Msg == (int)Win32.Msgs.WM_KEYDOWN && (int)m.WParam == (int)Keys.Escape)
                    EndDrag(true);

                return OnPreFilterMessage(ref m);
            }

            protected virtual bool OnPreFilterMessage(ref Message m)
            {
                return false;
            }

            protected sealed override void WndProc(ref Message m)
            {
                if (m.Msg == (int)Win32.Msgs.WM_CANCELMODE || m.Msg == (int)Win32.Msgs.WM_CAPTURECHANGED)
                    EndDrag(true);

                base.WndProc(ref m);
            }
        }

        private abstract class DragHandler : DragHandlerBase
        {
            private DockPanel m_dockPanel;

            protected DragHandler(DockPanel dockPanel)
            {
                m_dockPanel = dockPanel;
            }

            public DockPanel DockPanel
            {
                get { return m_dockPanel; }
            }

            private IDragSource m_dragSource;
            protected IDragSource DragSource
            {
                get { return m_dragSource; }
                set { m_dragSource = value; }
            }

            protected sealed override Control DragControl
            {
                get { return DragSource == null ? null : DragSource.DragControl; }
            }

            protected sealed override bool OnPreFilterMessage(ref Message m)
            {
                if ((m.Msg == (int)Win32.Msgs.WM_KEYDOWN || m.Msg == (int)Win32.Msgs.WM_KEYUP) &&
                    ((int)m.WParam == (int)Keys.ControlKey || (int)m.WParam == (int)Keys.ShiftKey))
                    OnDragging();

                return base.OnPreFilterMessage(ref m);
            }
        }
    }
}
