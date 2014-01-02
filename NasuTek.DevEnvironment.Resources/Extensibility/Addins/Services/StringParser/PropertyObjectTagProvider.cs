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
using System.Reflection;

namespace NasuTek.DevEnvironment.Extensibility.Addins
{
	/// <summary>
	/// Provides properties by using Reflection on an object.
	/// </summary>
	public sealed class PropertyObjectTagProvider : IStringTagProvider
	{
		readonly object obj;
		
		public PropertyObjectTagProvider(object obj)
		{
			if (obj == null)
				throw new ArgumentNullException("obj");
			this.obj = obj;
		}
		
		public string ProvideString(string tag, StringTagPair[] customTags)
		{
			Type type = obj.GetType();
			PropertyInfo prop = type.GetProperty(tag);
			if (prop != null) {
				return prop.GetValue(obj, null).ToString();
			}
			FieldInfo field = type.GetField(tag);
			if (field != null) {
				return field.GetValue(obj).ToString();
			}
			return null;
		}
	}
}
