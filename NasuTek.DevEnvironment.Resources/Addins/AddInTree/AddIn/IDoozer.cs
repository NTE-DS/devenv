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

namespace NasuTek.DevEnvironment.Resources.Addins
{
	/// <summary>
	/// Interface for classes that can build objects out of codons.
	/// </summary>
	/// <remarks>http://en.wikipedia.org/wiki/Fraggle_Rock#Doozers</remarks>
	public interface IDoozer
	{
		/// <summary>
		/// Gets if the doozer handles codon conditions on its own.
		/// If this property return false, the item is excluded when the condition is not met.
		/// </summary>
		bool HandleConditions { get; }
		
		/// <summary>
		/// Construct the item.
		/// </summary>
		/// <returns>
		/// The constructed item.
		/// May return an object implementing <see cref="IBuildItemsModifier"/> for returning
		/// multiple arguments.
		/// </returns>
		object BuildItem(BuildItemArgs args);
	}
}
