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
using System.Globalization;
using Microsoft.Win32;

namespace NasuTek.DevEnvironment.Resources.Addins
{
	/// <summary>
	/// RegistryService.
	/// </summary>
	public static class RegistryService
	{
		/// <summary>
		/// Gets the registry value.
		/// </summary>
		/// <param name="hive">Registry hive.</param>
		/// <param name="key">Registry key.</param>
		/// <param name="value">Registry value.</param>
		/// <param name="kind">Registry kind.</param>
		/// <param name="data">Data.</param>
		/// <returns>True, if the data was found, False otherwise.</returns>
		public static bool GetRegistryValue<T>(RegistryHive hive, string key, string value, RegistryValueKind kind, out T data)
		{
			data = default(T);
			
			try {
				using (RegistryKey baseKey = RegistryKey.OpenRemoteBaseKey(hive, String.Empty))
				{
					if (baseKey != null)
					{
						using (RegistryKey registryKey = baseKey.OpenSubKey(key, RegistryKeyPermissionCheck.ReadSubTree))
						{
							if (registryKey != null)
							{
								RegistryValueKind kindFound = registryKey.GetValueKind(value);
								if (kindFound == kind)
								{
									object regValue = registryKey.GetValue(value, null);
									if (regValue != null)
									{
										data = (T)Convert.ChangeType(regValue, typeof(T), CultureInfo.InvariantCulture);
										return true;
									}
								}								
							}
						}
					}
				}
				
				return false;
			} catch {
				return false;
			}
		}
	}
}
