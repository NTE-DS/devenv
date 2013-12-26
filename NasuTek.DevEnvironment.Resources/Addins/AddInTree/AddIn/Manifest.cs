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
using System.Xml;

namespace NasuTek.DevEnvironment.Resources.Addins
{
	/// <summary>
	/// Stores information about the manifest of an AddIn.
	/// </summary>
	public class AddInManifest
	{
		List<AddInReference> dependencies = new List<AddInReference>();
		List<AddInReference> conflicts = new List<AddInReference>();
		Dictionary<string, Version> identities = new Dictionary<string, Version>();
		Version primaryVersion;
		string primaryIdentity;
		
		public string PrimaryIdentity {
			get {
				return primaryIdentity;
			}
		}
		
		public Version PrimaryVersion {
			get {
				return primaryVersion;
			}
		}
		
		public Dictionary<string, Version> Identities {
			get {
				return identities;
			}
		}
		
		public ReadOnlyCollection<AddInReference> Dependencies {
			get {
				return dependencies.AsReadOnly();
			}
		}
		
		public ReadOnlyCollection<AddInReference> Conflicts {
			get {
				return conflicts.AsReadOnly();
			}
		}
		
		void AddIdentity(string name, string version, string hintPath)
		{
			if (name.Length == 0)
				throw new AddInLoadException("Identity needs a name");
			foreach (char c in name) {
				if (!char.IsLetterOrDigit(c) && c != '.' && c != '_') {
					throw new AddInLoadException("Identity name contains invalid character: '" + c + "'");
				}
			}
			Version v = AddInReference.ParseVersion(version, hintPath);
			if (primaryVersion == null) {
				primaryVersion = v;
			}
			if (primaryIdentity == null) {
				primaryIdentity = name;
			}
			identities.Add(name, v);
		}
		
		public void ReadManifestSection(XmlReader reader, string hintPath)
		{
			if (reader.AttributeCount != 0) {
				throw new AddInLoadException("Manifest node cannot have attributes.");
			}
			if (reader.IsEmptyElement) {
				throw new AddInLoadException("Manifest node cannot be empty.");
			}
			while (reader.Read()) {
				switch (reader.NodeType) {
					case XmlNodeType.EndElement:
						if (reader.LocalName == "Manifest") {
							return;
						}
						break;
					case XmlNodeType.Element:
						string nodeName = reader.LocalName;
						Properties properties = Properties.ReadFromAttributes(reader);
						switch (nodeName) {
							case "Identity":
								AddIdentity(properties["name"], properties["version"], hintPath);
								break;
							case "Dependency":
								dependencies.Add(AddInReference.Create(properties, hintPath));
								break;
							case "Conflict":
								conflicts.Add(AddInReference.Create(properties, hintPath));
								break;
							default:
								throw new AddInLoadException("Unknown node in Manifest section:" + nodeName);
						}
						break;
				}
			}
		}
	}
}
