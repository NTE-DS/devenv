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
	/// Creates associations between file types or node types in the project browser and
	/// icons in the resource service.
	/// </summary>
	/// <attribute name="resource" use="required">
	/// The name of a bitmap resource in the resource service.
	/// </attribute>
	/// <attribute name="language">
	/// This attribute is specified when a project icon association should be created.
	/// It specifies the language of the project types that use the icon.
	/// </attribute>
	/// <attribute name="extensions">
	/// This attribute is specified when a file icon association should be created.
	/// It specifies the semicolon-separated list of file types that use the icon.
	/// </attribute>
	/// <usage>Only in /Workspace/Icons</usage>
	/// <returns>
	/// An IconDescriptor object that exposes the attributes.
	/// </returns>
	public class IconDoozer : IDoozer
	{
		/// <summary>
		/// Gets if the doozer handles codon conditions on its own.
		/// If this property return false, the item is excluded when the condition is not met.
		/// </summary>
		public bool HandleConditions {
			get {
				return false;
			}
		}
		
		public object BuildItem(BuildItemArgs args)
		{
			return new IconDescriptor(args.Codon);
		}
	}
}
