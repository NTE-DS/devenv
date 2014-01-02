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
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace NasuTek.DevEnvironment.Workspace.Docking
{
	public class DockPaneCollection : ReadOnlyCollection<DockPane>
	{
        internal DockPaneCollection()
            : base(new List<DockPane>())
        {
        }

		internal int Add(DockPane pane)
		{
			if (Items.Contains(pane))
				return Items.IndexOf(pane);

			Items.Add(pane);
            return Count - 1;
		}

		internal void AddAt(DockPane pane, int index)
		{
			if (index < 0 || index > Items.Count - 1)
				return;
			
			if (Contains(pane))
				return;

			Items.Insert(index, pane);
		}

		internal void Dispose()
		{
			for (int i=Count - 1; i>=0; i--)
				this[i].Close();
		}

		internal void Remove(DockPane pane)
		{
			Items.Remove(pane);
		}
	}
}
