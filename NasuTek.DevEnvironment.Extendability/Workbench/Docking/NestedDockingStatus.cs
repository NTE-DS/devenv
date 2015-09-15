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

namespace NasuTek.DevEnvironment.Extendability.Workbench.Docking
{
	public sealed class NestedDockingStatus
	{
		internal NestedDockingStatus(DockPane pane)
		{
			m_dockPane = pane;
		}

		private DockPane m_dockPane = null;
		public DockPane DockPane
		{
			get	{	return m_dockPane;	}
		}
		
		private NestedPaneCollection m_nestedPanes = null;
		public NestedPaneCollection NestedPanes
		{
			get	{	return m_nestedPanes;	}
		}
		
		private DockPane m_previousPane = null;
		public DockPane PreviousPane
		{
			get	{	return m_previousPane;	}
		}

		private DockAlignment m_alignment = DockAlignment.Left;
		public DockAlignment Alignment
		{
			get	{	return m_alignment;	}
		}

		private double m_proportion = 0.5;
		public double Proportion
		{
			get	{	return m_proportion;	}
		}

		private bool m_isDisplaying = false;
		public bool IsDisplaying
		{
			get	{	return m_isDisplaying;	}
		}

		private DockPane m_displayingPreviousPane = null;
		public DockPane DisplayingPreviousPane
		{
			get	{	return m_displayingPreviousPane;	}
		}

		private DockAlignment m_displayingAlignment = DockAlignment.Left;
		public DockAlignment DisplayingAlignment
		{
			get	{	return m_displayingAlignment;	}
		}

		private double m_displayingProportion = 0.5;
		public double DisplayingProportion
		{
			get	{	return m_displayingProportion;	}
		}

		private Rectangle m_logicalBounds = Rectangle.Empty; 
		public Rectangle LogicalBounds
		{
			get	{	return m_logicalBounds;	}
		}

		private Rectangle m_paneBounds = Rectangle.Empty;
		public Rectangle PaneBounds
		{
			get	{	return m_paneBounds;	}
		}

		private Rectangle m_splitterBounds = Rectangle.Empty;
		public Rectangle SplitterBounds
		{
			get	{	return m_splitterBounds;	}
		}

		internal void SetStatus(NestedPaneCollection nestedPanes, DockPane previousPane, DockAlignment alignment, double proportion)
		{
			m_nestedPanes = nestedPanes;
			m_previousPane = previousPane;
			m_alignment = alignment;
			m_proportion = proportion;
		}

		internal void SetDisplayingStatus(bool isDisplaying, DockPane displayingPreviousPane, DockAlignment displayingAlignment, double displayingProportion)
		{
			m_isDisplaying = isDisplaying;
			m_displayingPreviousPane = displayingPreviousPane;
			m_displayingAlignment = displayingAlignment;
			m_displayingProportion = displayingProportion;
		}

		internal void SetDisplayingBounds(Rectangle logicalBounds, Rectangle paneBounds, Rectangle splitterBounds)
		{
			m_logicalBounds = logicalBounds;
			m_paneBounds = paneBounds;
			m_splitterBounds = splitterBounds;
		}
	}
}
