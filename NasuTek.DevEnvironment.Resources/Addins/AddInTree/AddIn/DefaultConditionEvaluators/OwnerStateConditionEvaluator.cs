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

namespace NasuTek.DevEnvironment.Resources.Addins
{
	public interface IOwnerState {
		System.Enum InternalState {
			get;
		}
	}
	
	/// <summary>
	/// Condition evaluator that compares the state of the caller/owner with a specified value.
	/// The caller/owner has to implement <see cref="IOwnerState"/>.
	/// </summary>
	public class OwnerStateConditionEvaluator : IConditionEvaluator
	{
		public bool IsValid(object caller, Condition condition)
		{
			if (caller is IOwnerState) {
				try {
					System.Enum state         = ((IOwnerState)caller).InternalState;
					System.Enum conditionEnum = (System.Enum)Enum.Parse(state.GetType(), condition.Properties["ownerstate"]);
					
					int stateInt     = Int32.Parse(state.ToString("D"));
					int conditionInt = Int32.Parse(conditionEnum.ToString("D"));
					
					return (stateInt & conditionInt) > 0;
				} catch (Exception ex) {
					throw new CoreException("can't parse '" + condition.Properties["state"] + "'. Not a valid value.", ex);
				}
			}
			return false;
		}
	}
}
