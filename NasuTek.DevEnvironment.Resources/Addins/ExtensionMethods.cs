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

namespace NasuTek.DevEnvironment.Resources.Addins
{
	internal static class ExtensionMethods
	{
		/// <summary>
		/// Converts a recursive data structure into a flat list.
		/// </summary>
		/// <param name="input">The root elements of the recursive data structure.</param>
		/// <param name="recursion">The function that gets the children of an element.</param>
		/// <returns>Iterator that enumerates the tree structure in preorder.</returns>
		public static IEnumerable<T> Flatten<T>(this IEnumerable<T> input, Func<T, IEnumerable<T>> recursion)
		{
			Stack<IEnumerator<T>> stack = new Stack<IEnumerator<T>>();
			try {
				stack.Push(input.GetEnumerator());
				while (stack.Count > 0) {
					while (stack.Peek().MoveNext()) {
						T element = stack.Peek().Current;
						yield return element;
						IEnumerable<T> children = recursion(element);
						if (children != null) {
							stack.Push(children.GetEnumerator());
						}
					}
					stack.Pop().Dispose();
				}
			} finally {
				while (stack.Count > 0) {
					stack.Pop().Dispose();
				}
			}
		}
	}
}
