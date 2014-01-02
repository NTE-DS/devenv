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
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;

namespace NasuTek.DevEnvironment.Workspace.Docking
{
	public sealed class VisibleNestedPaneCollection : ReadOnlyCollection<DockPane>
	{
		private NestedPaneCollection m_nestedPanes;

        internal VisibleNestedPaneCollection(NestedPaneCollection nestedPanes)
            : base(new List<DockPane>())
        {
            m_nestedPanes = nestedPanes;
        }

		public NestedPaneCollection NestedPanes
		{
			get	{	return m_nestedPanes;	}
		}

		public INestedPanesContainer Container
		{
			get	{	return NestedPanes.Container;	}
		}

		public DockState DockState
		{
			get	{	return NestedPanes.DockState;	}
		}

		public bool IsFloat
		{
			get	{	return NestedPanes.IsFloat;	}
		}

		internal void Refresh()
		{
			Items.Clear();
			for (int i=0; i<NestedPanes.Count; i++)
			{
				DockPane pane = NestedPanes[i];
				NestedDockingStatus status = pane.NestedDockingStatus;
				status.SetDisplayingStatus(true, status.PreviousPane, status.Alignment, status.Proportion);
				Items.Add(pane);
			}

			foreach (DockPane pane in NestedPanes)
				if (pane.DockState != DockState || pane.IsHidden)
				{
					pane.Bounds = Rectangle.Empty;
					pane.SplitterBounds = Rectangle.Empty;
					Remove(pane);
				}

			CalculateBounds();

			foreach (DockPane pane in this)
			{
				NestedDockingStatus status = pane.NestedDockingStatus;
				pane.Bounds = status.PaneBounds;
				pane.SplitterBounds = status.SplitterBounds;
				pane.SplitterAlignment = status.Alignment;
			}
		}

		private void Remove(DockPane pane)
		{
			if (!Contains(pane))
				return;

			NestedDockingStatus statusPane = pane.NestedDockingStatus;
			DockPane lastNestedPane = null;
			for (int i=Count - 1; i> IndexOf(pane); i--)
			{
				if (this[i].NestedDockingStatus.PreviousPane == pane)
				{
					lastNestedPane = this[i];
					break;
				}
			}

			if (lastNestedPane != null)
			{
				int indexLastNestedPane = IndexOf(lastNestedPane);
				Items.Remove(lastNestedPane);
				Items[IndexOf(pane)] = lastNestedPane;
				NestedDockingStatus lastNestedDock = lastNestedPane.NestedDockingStatus;
				lastNestedDock.SetDisplayingStatus(true, statusPane.DisplayingPreviousPane, statusPane.DisplayingAlignment, statusPane.DisplayingProportion);
				for (int i=indexLastNestedPane - 1; i>IndexOf(lastNestedPane); i--)
				{
					NestedDockingStatus status = this[i].NestedDockingStatus;
					if (status.PreviousPane == pane)
						status.SetDisplayingStatus(true, lastNestedPane, status.DisplayingAlignment, status.DisplayingProportion);
				}
			}
			else
				Items.Remove(pane);

			statusPane.SetDisplayingStatus(false, null, DockAlignment.Left, 0.5);
		}

		private void CalculateBounds()
		{
			if (Count == 0)
				return;

			this[0].NestedDockingStatus.SetDisplayingBounds(Container.DisplayingRectangle, Container.DisplayingRectangle, Rectangle.Empty);

			for (int i=1; i<Count; i++)
			{
				DockPane pane = this[i];
				NestedDockingStatus status = pane.NestedDockingStatus;
				DockPane prevPane = status.DisplayingPreviousPane;
				NestedDockingStatus statusPrev = prevPane.NestedDockingStatus;

				Rectangle rect = statusPrev.PaneBounds;
				bool bVerticalSplitter = (status.DisplayingAlignment == DockAlignment.Left || status.DisplayingAlignment == DockAlignment.Right);

				Rectangle rectThis = rect;
				Rectangle rectPrev = rect;
				Rectangle rectSplitter = rect;
				if (status.DisplayingAlignment == DockAlignment.Left)
				{
					rectThis.Width = (int)((double)rect.Width * status.DisplayingProportion) - (Measures.SplitterSize / 2);
					rectSplitter.X = rectThis.X + rectThis.Width;
					rectSplitter.Width = Measures.SplitterSize;
					rectPrev.X = rectSplitter.X + rectSplitter.Width;
					rectPrev.Width = rect.Width - rectThis.Width - rectSplitter.Width;
				}
				else if (status.DisplayingAlignment == DockAlignment.Right)
				{
					rectPrev.Width = (rect.Width - (int)((double)rect.Width * status.DisplayingProportion)) - (Measures.SplitterSize / 2);
					rectSplitter.X = rectPrev.X + rectPrev.Width;
					rectSplitter.Width = Measures.SplitterSize;
					rectThis.X = rectSplitter.X + rectSplitter.Width;
					rectThis.Width = rect.Width - rectPrev.Width - rectSplitter.Width;
				}
				else if (status.DisplayingAlignment == DockAlignment.Top)
				{
					rectThis.Height = (int)((double)rect.Height * status.DisplayingProportion) - (Measures.SplitterSize / 2);
					rectSplitter.Y = rectThis.Y + rectThis.Height;
					rectSplitter.Height = Measures.SplitterSize;
					rectPrev.Y = rectSplitter.Y + rectSplitter.Height;
					rectPrev.Height = rect.Height - rectThis.Height - rectSplitter.Height;
				}
				else if (status.DisplayingAlignment == DockAlignment.Bottom)
				{
					rectPrev.Height = (rect.Height - (int)((double)rect.Height * status.DisplayingProportion)) - (Measures.SplitterSize / 2);
					rectSplitter.Y = rectPrev.Y + rectPrev.Height;
					rectSplitter.Height = Measures.SplitterSize;
					rectThis.Y = rectSplitter.Y + rectSplitter.Height;
					rectThis.Height = rect.Height - rectPrev.Height - rectSplitter.Height;
				}
				else
					rectThis = Rectangle.Empty;

				rectSplitter.Intersect(rect);
				rectThis.Intersect(rect);
				rectPrev.Intersect(rect);
				status.SetDisplayingBounds(rect, rectThis, rectSplitter);
				statusPrev.SetDisplayingBounds(statusPrev.LogicalBounds, rectPrev, statusPrev.SplitterBounds);
			}
		}
	}
}
