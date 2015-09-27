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

namespace NasuTek.DevEnvironment.Extensibility.Workbench.Docking
{
	public class DockWindowCollection : ReadOnlyCollection<DockWindow>
	{
		internal DockWindowCollection(DockPanel dockPanel)
            : base(new List<DockWindow>())
		{
			Items.Add(new DockWindow(dockPanel, DockState.Document));
			Items.Add(new DockWindow(dockPanel, DockState.DockLeft));
			Items.Add(new DockWindow(dockPanel, DockState.DockRight));
			Items.Add(new DockWindow(dockPanel, DockState.DockTop));
			Items.Add(new DockWindow(dockPanel, DockState.DockBottom));
		}

		public DockWindow this [DockState dockState]
		{
			get
			{
				if (dockState == DockState.Document)
					return Items[0];
				else if (dockState == DockState.DockLeft || dockState == DockState.DockLeftAutoHide)
					return Items[1];
				else if (dockState == DockState.DockRight || dockState == DockState.DockRightAutoHide)
					return Items[2];
				else if (dockState == DockState.DockTop || dockState == DockState.DockTopAutoHide)
					return Items[3];
				else if (dockState == DockState.DockBottom || dockState == DockState.DockBottomAutoHide)
					return Items[4];

				throw (new ArgumentOutOfRangeException());
			}
		}
	}
}
