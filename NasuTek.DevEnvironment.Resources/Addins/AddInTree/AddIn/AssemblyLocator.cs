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
using System.Reflection;

namespace NasuTek.DevEnvironment.Resources.Addins
{
	// Based on http://ayende.com/Blog/archive/2006/05/22/SolvingTheAssemblyLoadContextProblem.aspx
	// This class ensures that assemblies loaded into the LoadFrom context are also available
	// in the Load context.
	static class AssemblyLocator
	{
		static Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();
		static bool initialized;
		
		public static void Init()
		{
			lock (assemblies) {
				if (initialized)
					return;
				initialized = true;
				AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;
				AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
			}
		}
		
		static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			lock (assemblies) {
				Assembly assembly = null;
				assemblies.TryGetValue(args.Name, out assembly);
				return assembly;
			}
		}
		
		static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
		{
			Assembly assembly = args.LoadedAssembly;
			lock (assemblies) {
				assemblies[assembly.FullName] = assembly;
			}
		}
	}
}
