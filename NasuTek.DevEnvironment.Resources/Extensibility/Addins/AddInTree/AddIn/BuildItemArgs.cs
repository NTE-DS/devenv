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
using System.Linq;

namespace NasuTek.DevEnvironment.Extensibility.Addins
{
	/// <summary>
	/// Argument class used for <see cref="IDoozer.BuildItem"/>.
	/// </summary>
	public class BuildItemArgs
	{
		object caller;
		Codon codon;
		IEnumerable<ICondition> conditions;
		AddInTreeNode subItemNode;
		
		public BuildItemArgs(object caller, Codon codon, IEnumerable<ICondition> conditions, AddInTreeNode subItemNode)
		{
			if (codon == null)
				throw new ArgumentNullException("codon");
			this.caller = caller;
			this.codon = codon;
			this.conditions = conditions ?? Enumerable.Empty<ICondition>();
			this.subItemNode = subItemNode;
		}
		
		/// <summary>
		/// The caller passed to <see cref="AddInTree.BuildItem(string,object)"/>.
		/// </summary>
		public object Caller {
			get { return caller; }
		}
		
		/// <summary>
		/// The codon to build.
		/// </summary>
		public Codon Codon {
			get { return codon; }
		}
		
		/// <summary>
		/// The addin containing the codon.
		/// </summary>
		public AddIn AddIn {
			get { return codon.AddIn; }
		}
		
		/// <summary>
		/// The conditions applied to this item.
		/// </summary>
		public IEnumerable<ICondition> Conditions {
			get { return conditions; }
		}
		
		/// <summary>
		/// The addin tree node containing the sub-items.
		/// Returns null if no sub-items exist.
		/// </summary>
		public AddInTreeNode SubItemNode {
			get { return subItemNode; }
		}
		
		/// <summary>
		/// Builds the sub-items.
		/// Conditions on this node are also applied to the sub-nodes.
		/// </summary>
		public List<T> BuildSubItems<T>()
		{
			if (subItemNode == null)
				return new List<T>();
			else
				return subItemNode.BuildChildItems<T>(caller, conditions);
		}
	}
}
