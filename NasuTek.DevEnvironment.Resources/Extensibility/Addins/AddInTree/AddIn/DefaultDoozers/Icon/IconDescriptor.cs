﻿/***************************************************************************************************
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

namespace NasuTek.DevEnvironment.Extensibility.Addins
{
	public class IconDescriptor 
	{
		Codon codon;
		
		public string Id {
			get {
				return codon.Id;
			}
		}
		
		public string Language {
			get {
				return codon.Properties["language"];
			}
		}
		
		public string Resource {
			get {
				return codon.Properties["resource"];
			}
		}
		
		public string[] Extensions {
			get {
				return codon.Properties["extensions"].Split(';');
			}
		}
		
		public IconDescriptor(Codon codon)
		{
			this.codon = codon;
		}
	}
}