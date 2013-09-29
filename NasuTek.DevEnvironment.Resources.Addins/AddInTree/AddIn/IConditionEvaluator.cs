// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;

namespace NasuTek.DevEnvironment.Resources.Addins
{
	/// <summary>
	/// Interface for classes that can evaluate conditions defined in the addin tree.
	/// </summary>
	public interface IConditionEvaluator
	{
		bool IsValid(object owner, Condition condition);
	}
}
