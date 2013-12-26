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
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace NasuTek.DevEnvironment.Resources
{
	public class Arguments
	{
		private readonly StringDictionary _parameters;
		public string this[string param]
		{
			get
			{
				return this._parameters[param];
			}
		}
		public Arguments(string[] args)
		{
			this._parameters = new StringDictionary();
			Regex regex = new Regex("^-{1,2}|^/|=|:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			Regex regex2 = new Regex("^['\"]?(.*?)['\"]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			string text = null;
			for (int i = 0; i < args.Length; i++)
			{
				string input = args[i];
				string[] array = regex.Split(input, 3);
				switch (array.Length)
				{
				case 1:
					if (text != null)
					{
						if (!this._parameters.ContainsKey(text))
						{
							array[0] = regex2.Replace(array[0], "$1");
							this._parameters.Add(text, array[0]);
						}
						text = null;
					}
					break;
				case 2:
					if (text != null && !this._parameters.ContainsKey(text))
					{
						this._parameters.Add(text, "true");
					}
					text = array[1];
					break;
				case 3:
					if (text != null && !this._parameters.ContainsKey(text))
					{
						this._parameters.Add(text, "true");
					}
					text = array[1];
					if (!this._parameters.ContainsKey(text))
					{
						array[2] = regex2.Replace(array[2], "$1");
						this._parameters.Add(text, array[2]);
					}
					text = null;
					break;
				}
			}
			if (text != null && !this._parameters.ContainsKey(text))
			{
				this._parameters.Add(text, "true");
			}
		}
	}
}
