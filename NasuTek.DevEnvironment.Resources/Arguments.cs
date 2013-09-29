/***************************************************************************************************
 * NasuTek Developer Studio
 * Copyright (C) 2005-2013 NasuTek Enterprises
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
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
