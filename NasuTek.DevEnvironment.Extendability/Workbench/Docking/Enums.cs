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
using System.ComponentModel;
using System.Windows.Forms;

namespace NasuTek.DevEnvironment.Extensibility.Workbench.Docking
{
	[Flags]
	[Serializable]
	[Editor(typeof(DockAreasEditor), typeof(System.Drawing.Design.UITypeEditor))]
	public enum DockAreas
	{
		Float = 1,
		DockLeft = 2,
		DockRight = 4,
		DockTop = 8,
		DockBottom = 16,
		Document = 32
	}

	public enum DockState
	{
		Unknown = 0,
		Float = 1,
		DockTopAutoHide = 2,
		DockLeftAutoHide = 3,
		DockBottomAutoHide = 4,
		DockRightAutoHide = 5,
		Document = 6,
		DockTop = 7,
		DockLeft = 8,
		DockBottom = 9,
		DockRight = 10,
		Hidden = 11
	}

	public enum DockAlignment
	{
		Left,
		Right,
		Top,
		Bottom
	}

	public enum DocumentStyle
	{
		DockingMdi,
		DockingWindow,
		DockingSdi,
		SystemMdi,
	}

    /// <summary>
    /// The location to draw the DockPaneStrip for Document style windows.
    /// </summary>
    public enum DocumentTabStripLocation
    {
        Top,
        Bottom
    }
}
