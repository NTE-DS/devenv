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

namespace NasuTek.DevEnvironment.Extendability.Workbench.Docking
{
	public sealed class NestedPaneCollection : ReadOnlyCollection<DockPane>
	{
		private INestedPanesContainer m_container;
		private VisibleNestedPaneCollection m_visibleNestedPanes;

		internal NestedPaneCollection(INestedPanesContainer container)
            : base(new List<DockPane>())
		{
			m_container = container;
			m_visibleNestedPanes = new VisibleNestedPaneCollection(this);
		}

		public INestedPanesContainer Container
		{
			get	{	return m_container;	}
		}
		
		public VisibleNestedPaneCollection VisibleNestedPanes
		{
			get	{	return m_visibleNestedPanes;	}
		}

		public DockState DockState
		{
			get	{	return Container.DockState;	}
		}

		public bool IsFloat
		{
			get	{	return DockState == DockState.Float;	}
		}

		internal void Add(DockPane pane)
		{
			if (pane == null)
				return;

			NestedPaneCollection oldNestedPanes = (pane.NestedPanesContainer == null) ? null : pane.NestedPanesContainer.NestedPanes;
			if (oldNestedPanes != null)
				oldNestedPanes.InternalRemove(pane);
			Items.Add(pane);
			if (oldNestedPanes != null)
				oldNestedPanes.CheckFloatWindowDispose();
		}

		private void CheckFloatWindowDispose()
		{
			if (Count == 0 && Container.DockState == DockState.Float)
			{
				FloatWindow floatWindow = (FloatWindow)Container;
				if (!floatWindow.Disposing && !floatWindow.IsDisposed)
                                        if (!Win32Helper.IsRunningOnMono)
					NativeMethods.PostMessage(((FloatWindow)Container).Handle, FloatWindow.WM_CHECKDISPOSE, 0, 0);
			}
		}

        /// <summary>
        /// Switches a pane with its first child in the pane hierarchy. (The actual hiding happens elsewhere.)
        /// </summary>
        /// <param name="pane">Pane to switch</param>
        internal void SwitchPaneWithFirstChild(DockPane pane)
        {
            if (!Contains(pane))
                return;

            NestedDockingStatus statusPane = pane.NestedDockingStatus;
            DockPane lastNestedPane = null;
            for (int i = Count - 1; i > IndexOf(pane); i--)
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
                Items[IndexOf(pane)] = lastNestedPane;
                Items[indexLastNestedPane] = pane;
                NestedDockingStatus lastNestedDock = lastNestedPane.NestedDockingStatus;

                DockAlignment newAlignment;
                if (lastNestedDock.Alignment == DockAlignment.Left)
                    newAlignment = DockAlignment.Right;
                else if (lastNestedDock.Alignment == DockAlignment.Right)
                    newAlignment = DockAlignment.Left;
                else if (lastNestedDock.Alignment == DockAlignment.Top)
                    newAlignment = DockAlignment.Bottom;
                else
                    newAlignment = DockAlignment.Top;
                double newProportion = 1 - lastNestedDock.Proportion;

                lastNestedDock.SetStatus(this, statusPane.PreviousPane, statusPane.Alignment, statusPane.Proportion);
                for (int i = indexLastNestedPane - 1; i > IndexOf(lastNestedPane); i--)
                {
                    NestedDockingStatus status = this[i].NestedDockingStatus;
                    if (status.PreviousPane == pane)
                        status.SetStatus(this, lastNestedPane, status.Alignment, status.Proportion);
                }

                statusPane.SetStatus(this, lastNestedPane, newAlignment, newProportion);
            }
        }

		internal void Remove(DockPane pane)
		{
			InternalRemove(pane);
			CheckFloatWindowDispose();
		}

		private void InternalRemove(DockPane pane)
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
				lastNestedDock.SetStatus(this, statusPane.PreviousPane, statusPane.Alignment, statusPane.Proportion);
				for (int i=indexLastNestedPane - 1; i>IndexOf(lastNestedPane); i--)
				{
					NestedDockingStatus status = this[i].NestedDockingStatus;
					if (status.PreviousPane == pane)
						status.SetStatus(this, lastNestedPane, status.Alignment, status.Proportion);
				}
			}
			else
				Items.Remove(pane);

			statusPane.SetStatus(null, null, DockAlignment.Left, 0.5);
			statusPane.SetDisplayingStatus(false, null, DockAlignment.Left, 0.5);
			statusPane.SetDisplayingBounds(Rectangle.Empty, Rectangle.Empty, Rectangle.Empty);
		}

		public DockPane GetDefaultPreviousPane(DockPane pane)
		{
			for (int i=Count-1; i>=0; i--)
				if (this[i] != pane)
					return this[i];

			return null;
		}
	}
}
