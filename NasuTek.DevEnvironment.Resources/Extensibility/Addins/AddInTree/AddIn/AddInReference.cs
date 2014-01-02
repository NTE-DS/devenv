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
using System.Diagnostics;
using System.IO;

namespace NasuTek.DevEnvironment.Extensibility.Addins
{
	/// <summary>
	/// Represents a versioned reference to an AddIn. Used by <see cref="AddInManifest"/>.
	/// </summary>
	public class AddInReference : ICloneable
	{
		string name;
		Version minimumVersion;
		Version maximumVersion;
		bool requirePreload;
		
		public Version MinimumVersion {
			get {
				return minimumVersion;
			}
		}
		
		public Version MaximumVersion {
			get {
				return maximumVersion;
			}
		}
		
		public bool RequirePreload {
			get { return requirePreload; }
		}
		
		
		public string Name {
			get {
				return name;
			}
			set {
				if (value == null) throw new ArgumentNullException("name");
				if (value.Length == 0) throw new ArgumentException("name cannot be an empty string", "name");
				name = value;
			}
		}
		
		/// <returns>Returns true when the reference is valid.</returns>
		public bool Check(Dictionary<string, Version> addIns, out Version versionFound)
		{
			if (addIns.TryGetValue(name, out versionFound)) {
				return CompareVersion(versionFound, minimumVersion) >= 0
					&& CompareVersion(versionFound, maximumVersion) <= 0;
			} else {
				return false;
			}
		}
		
		/// <summary>
		/// Compares two versions and ignores unspecified fields (unlike Version.CompareTo)
		/// </summary>
		/// <returns>-1 if a &lt; b, 0 if a == b, 1 if a &gt; b</returns>
		int CompareVersion(Version a, Version b)
		{
			if (a.Major != b.Major) {
				return a.Major > b.Major ? 1 : -1;
			}
			if (a.Minor != b.Minor) {
				return a.Minor > b.Minor ? 1 : -1;
			}
			if (a.Build < 0 || b.Build < 0)
				return 0;
			if (a.Build != b.Build) {
				return a.Build > b.Build ? 1 : -1;
			}
			if (a.Revision < 0 || b.Revision < 0)
				return 0;
			if (a.Revision != b.Revision) {
				return a.Revision > b.Revision ? 1 : -1;
			}
			return 0;
		}
		
		public static AddInReference Create(Properties properties, string hintPath)
		{
			AddInReference reference = new AddInReference(properties["addin"]);
			string version = properties["version"];
			if (version != null && version.Length > 0) {
				int pos = version.IndexOf('-');
				if (pos > 0) {
					reference.minimumVersion = ParseVersion(version.Substring(0, pos), hintPath);
					reference.maximumVersion = ParseVersion(version.Substring(pos + 1), hintPath);
				} else {
					reference.maximumVersion = reference.minimumVersion = ParseVersion(version, hintPath);
				}
				
				if (reference.Name == "SharpDevelop") {
					// HACK: SD 4.1/4.2 AddIns work with SharpDevelop 4.3
					// Because some 4.1 AddIns restrict themselves to SD 4.1, we extend the
					// supported SD range.
					if (reference.maximumVersion == new Version("4.1") || reference.maximumVersion == new Version("4.2")) {
						reference.maximumVersion = new Version("4.3");
					}
				}
			}
			reference.requirePreload = string.Equals(properties["requirePreload"], "true", StringComparison.OrdinalIgnoreCase);
			return reference;
		}
		
		static Version entryVersion;
		
		internal static Version ParseVersion(string version, string hintPath)
		{
			if (version == null || version.Length == 0)
				return new Version(0,0,0,0);
			if (version.StartsWith("@")) {
				if (version == "@SharpDevelopCoreVersion") {
					if (entryVersion == null)
                        entryVersion = new Version(DevEnvVersion.MajorCodebase + "." + DevEnvVersion.MinorCodebase + "." + DevEnvVersion.BuildCodebase + "." + DevEnvVersion.RevisionCodebase);
					return entryVersion;
				}
				if (hintPath != null) {
					string fileName = Path.Combine(hintPath, version.Substring(1));
					try {
						FileVersionInfo info = FileVersionInfo.GetVersionInfo(fileName);
						return new Version(info.FileMajorPart, info.FileMinorPart, info.FileBuildPart, info.FilePrivatePart);
					} catch (FileNotFoundException ex) {
						throw new AddInLoadException("Cannot get version '" + version + "': " + ex.Message);
					}
				}
				return new Version(0,0,0,0);
			} else {
				return new Version(version);
			}
		}
		
		public AddInReference(string name) : this(name, new Version(0,0,0,0), new Version(int.MaxValue, int.MaxValue)) { }
		
		public AddInReference(string name, Version specificVersion) : this(name, specificVersion, specificVersion) { }
		
		public AddInReference(string name, Version minimumVersion, Version maximumVersion)
		{
			this.Name = name;
			if (minimumVersion == null) throw new ArgumentNullException("minimumVersion");
			if (maximumVersion == null) throw new ArgumentNullException("maximumVersion");
			
			this.minimumVersion = minimumVersion;
			this.maximumVersion = maximumVersion;
		}
		
		public override bool Equals(object obj)
		{
			if (!(obj is AddInReference)) return false;
			AddInReference b = (AddInReference)obj;
			return name == b.name && minimumVersion == b.minimumVersion && maximumVersion == b.maximumVersion;
		}
		
		public override int GetHashCode()
		{
			return name.GetHashCode() ^ minimumVersion.GetHashCode() ^ maximumVersion.GetHashCode();
		}
		
		public override string ToString()
		{
			if (minimumVersion.ToString() == "0.0.0.0") {
				if (maximumVersion.Major == int.MaxValue) {
					return name;
				} else {
					return name + ", version <" + maximumVersion.ToString();
				}
			} else {
				if (maximumVersion.Major == int.MaxValue) {
					return name + ", version >" + minimumVersion.ToString();
				} else if (minimumVersion == maximumVersion) {
					return name + ", version " + minimumVersion.ToString();
				} else {
					return name + ", version " + minimumVersion.ToString() + "-" + maximumVersion.ToString();
				}
			}
		}
		
		public AddInReference Clone()
		{
			return new AddInReference(name, minimumVersion, maximumVersion);
		}
		
		object ICloneable.Clone()
		{
			return Clone();
		}
	}
}
