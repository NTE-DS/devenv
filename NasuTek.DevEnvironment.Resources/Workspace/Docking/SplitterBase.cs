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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NasuTek.DevEnvironment.Workspace.Docking
{
	internal class SplitterBase : Control
	{
		public SplitterBase()
		{
			SetStyle(ControlStyles.Selectable, false);
		}

		public override DockStyle Dock
		{
			get	{	return base.Dock;	}
			set
			{
				SuspendLayout();
				base.Dock = value;

				if (Dock == DockStyle.Left || Dock == DockStyle.Right)
					Width = SplitterSize;
				else if (Dock == DockStyle.Top || Dock == DockStyle.Bottom)
					Height = SplitterSize;
				else
					Bounds = Rectangle.Empty;

				if (Dock == DockStyle.Left || Dock == DockStyle.Right)
					Cursor = Cursors.VSplit;
				else if (Dock == DockStyle.Top || Dock == DockStyle.Bottom)
					Cursor = Cursors.HSplit;
				else
					Cursor = Cursors.Default;
					
				ResumeLayout();
			}
		}

		protected virtual int SplitterSize
		{
			get	{	return 0;	}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (e.Button != MouseButtons.Left)
				return;

			StartDrag();
		}

		protected virtual void StartDrag()
		{
		}

		protected override void WndProc(ref Message m)
		{
            // eat the WM_MOUSEACTIVATE message
			if (m.Msg == (int)Win32.Msgs.WM_MOUSEACTIVATE)
				return;

			base.WndProc(ref m);
		}
	}
}
