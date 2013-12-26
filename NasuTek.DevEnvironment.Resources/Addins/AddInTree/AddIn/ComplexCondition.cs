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
using System.Diagnostics;
using System.Text;
using System.Xml;

namespace NasuTek.DevEnvironment.Resources.Addins
{
	/// <summary>
	/// Negates a condition
	/// </summary>
	public class NegatedCondition : ICondition
	{
		ICondition condition;
		
		public string Name {
			get {
				return "Not " + condition.Name;
			}
		}
		
		ConditionFailedAction action = ConditionFailedAction.Exclude;
		public ConditionFailedAction Action {
			get {
				return action;
			}
			set {
				action = value;
			}
		}
		
		public NegatedCondition(ICondition condition)
		{
			Debug.Assert(condition != null);
			this.condition = condition;
		}
		
		public bool IsValid(object owner)
		{
			return !condition.IsValid(owner);
		}
		
		public static ICondition Read(XmlReader reader)
		{
			return new NegatedCondition(Condition.ReadConditionList(reader, "Not")[0]);
		}
	}

	/// <summary>
	/// Gives back the and result of two conditions.
	/// </summary>
	public class AndCondition : ICondition
	{
		ICondition[] conditions;
		
		public string Name {
			get {
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < conditions.Length; ++i) {
					sb.Append(conditions[i].Name);
					if (i + 1 < conditions.Length) {
						sb.Append(" And ");
					}
				}
				return sb.ToString();
			}
		}
		
		ConditionFailedAction action = ConditionFailedAction.Exclude;
		public ConditionFailedAction Action {
			get {
				return action;
			}
			set {
				action = value;
			}
		}
		
		public AndCondition(ICondition[] conditions)
		{
			Debug.Assert(conditions.Length >= 1);
			this.conditions = conditions;
		}
		
		public bool IsValid(object owner)
		{
			foreach (ICondition condition in conditions) {
				if (!condition.IsValid(owner)) {
					return false;
				}
			}
			return true;
		}
		
		public static ICondition Read(XmlReader reader)
		{
			return new AndCondition(Condition.ReadConditionList(reader, "And"));
		}
	}
	
	/// <summary>
	/// Gives back the or result of two conditions.
	/// </summary>
	public class OrCondition : ICondition
	{
		ICondition[] conditions;
		
		
		public string Name {
			get {
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < conditions.Length; ++i) {
					sb.Append(conditions[i].Name);
					if (i + 1 < conditions.Length) {
						sb.Append(" Or ");
					}
				}
				return sb.ToString();
			}
		}
		
		ConditionFailedAction action = ConditionFailedAction.Exclude;
		public ConditionFailedAction Action {
			get {
				return action;
			}
			set {
				action = value;
			}
		}
		
		public OrCondition(ICondition[] conditions)
		{
			Debug.Assert(conditions.Length >= 1);
			this.conditions = conditions;
		}
		
		public bool IsValid(object owner)
		{
			foreach (ICondition condition in conditions) {
				if (condition.IsValid(owner)) {
					return true;
				}
			}
			return false;
		}
		
		public static ICondition Read(XmlReader reader)
		{
			return new OrCondition(Condition.ReadConditionList(reader, "Or"));
		}
	}
}
