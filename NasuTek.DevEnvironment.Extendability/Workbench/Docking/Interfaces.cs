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
using System.Drawing;
using System.Windows.Forms;

namespace NasuTek.DevEnvironment.Extendability.Workbench.Docking
{
	public interface IDockContent
	{
		DockContentHandler DockHandler	{	get;	}
		void OnActivated(EventArgs e);
		void OnDeactivate(EventArgs e);
	}

	public interface INestedPanesContainer
	{
		DockState DockState	{	get;	}
		Rectangle DisplayingRectangle	{	get;	}
		NestedPaneCollection NestedPanes	{	get;	}
		VisibleNestedPaneCollection VisibleNestedPanes	{	get;	}
		bool IsFloat	{	get;	}
	}

    internal interface IDragSource
    {
        Control DragControl { get; }
    }

    internal interface IDockDragSource : IDragSource
    {
        Rectangle BeginDrag(Point ptMouse);
        void EndDrag();
        bool IsDockStateValid(DockState dockState);
        bool CanDockTo(DockPane pane);
        void FloatAt(Rectangle floatWindowBounds);
        void DockTo(DockPane pane, DockStyle dockStyle, int contentIndex);
        void DockTo(DockPanel panel, DockStyle dockStyle);
    }

    internal interface ISplitterDragSource : IDragSource
    {
        void BeginDrag(Rectangle rectSplitter);
        void EndDrag();
        bool IsVertical { get; }
        Rectangle DragLimitBounds { get; }
        void MoveSplitter(int offset);
    }
}
