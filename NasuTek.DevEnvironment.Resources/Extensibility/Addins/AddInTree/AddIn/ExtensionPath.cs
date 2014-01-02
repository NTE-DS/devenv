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
using System.Linq;
using System.Xml;

namespace NasuTek.DevEnvironment.Extensibility.Addins
{
	/// <summary>
	/// Represents all contributions to a Path in a single .addin file.
	/// </summary>
	public class ExtensionPath
	{
		string name;
		AddIn addIn;
		List<List<Codon>> codons = new List<List<Codon>>();

		public AddIn AddIn {
			get {
				return addIn;
			}
		}
		
		public string Name {
			get {
				return name;
			}
		}

		public IEnumerable<Codon> Codons {
			get {
				return
					from list in codons
					from c in list
					select c;
			}
		}

		/// <summary>
		/// Gets the codons separated by the groups they were created in.
		/// i.e. if two addins add the codons to the same path they will be in diffrent group.
		/// if the same addin adds the codon in diffrent path elements they will be in diffrent groups.
		/// </summary>
		public IEnumerable<IEnumerable<Codon>> GroupedCodons {
			get {
				return codons.AsReadOnly();
			}
		}

		public ExtensionPath(string name, AddIn addIn)
		{
			this.addIn = addIn;
			this.name = name;
		}

		public static void SetUp(ExtensionPath extensionPath, XmlReader reader, string endElement)
		{
			extensionPath.DoSetUp(reader, endElement);
		}
		
		void DoSetUp(XmlReader reader, string endElement)
		{
			Stack<ICondition> conditionStack = new Stack<ICondition>();
			List<Codon> innerCodons = new List<Codon>();
			while (reader.Read()) {
				switch (reader.NodeType) {
					case XmlNodeType.EndElement:
						if (reader.LocalName == "Condition" || reader.LocalName == "ComplexCondition") {
							conditionStack.Pop();
						} else if (reader.LocalName == endElement) {
							if (innerCodons.Count > 0)
								this.codons.Add(innerCodons);
							return;
						}
						break;
					case XmlNodeType.Element:
						string elementName = reader.LocalName;
						if (elementName == "Condition") {
							conditionStack.Push(Condition.Read(reader));
						} else if (elementName == "ComplexCondition") {
							conditionStack.Push(Condition.ReadComplexCondition(reader));
						} else {
							Codon newCodon = new Codon(this.AddIn, elementName, Properties.ReadFromAttributes(reader), conditionStack.ToArray());
							innerCodons.Add(newCodon);
							if (!reader.IsEmptyElement) {
								ExtensionPath subPath = this.AddIn.GetExtensionPath(this.Name + "/" + newCodon.Id);
								subPath.DoSetUp(reader, elementName);
							}
						}
						break;
				}
			}
			if (innerCodons.Count > 0)
				this.codons.Add(innerCodons);
		}
	}
}
