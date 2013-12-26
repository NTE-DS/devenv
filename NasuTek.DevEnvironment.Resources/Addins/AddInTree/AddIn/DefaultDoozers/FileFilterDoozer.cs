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
	/// Creates file filter entries for OpenFileDialogs or SaveFileDialogs.
	/// </summary>
	/// <attribute name="name" use="required">
	/// The name of the file filter entry.
	/// </attribute>
	/// <attribute name="extensions" use="required">
	/// The extensions associated with this file filter entry.
	/// </attribute>
	/// <usage>Only in /SharpDevelop/Workbench/FileFilter</usage>
	/// <returns>
	/// <see cref="FileFilterDescriptor"/> in the format "name|extensions".
	/// </returns>
	public class FileFilterDoozer : IDoozer
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
			Codon codon = args.Codon;
			return new FileFilterDescriptor {
				Name = StringParser.Parse(codon.Properties["name"]),
				Extensions = codon.Properties["extensions"],
				MimeType = codon.Properties["mimeType"]
			};
		}
	}
	
	public sealed class FileFilterDescriptor
	{
		public string Name { get; set; }
		public string Extensions { get; set; }
		public string MimeType { get; set; }
		
		/// <summary>
		/// Gets whether this descriptor matches the specified file extension.
		/// </summary>
		/// <param name="extension">File extension starting with '.'</param>
		public bool ContainsExtension(string extension)
		{
			if (string.IsNullOrEmpty(extension))
				return false;
			int index = this.Extensions.IndexOf("*" + extension, StringComparison.OrdinalIgnoreCase);
			if (index < 0 || index + extension.Length > this.Extensions.Length)
				return false;
			return index + extension.Length < this.Extensions.Length
				|| this.Extensions[index + extension.Length] == ';';
		}
		
		public override string ToString()
		{
			return Name + "|" + Extensions;
		}
	}
}
