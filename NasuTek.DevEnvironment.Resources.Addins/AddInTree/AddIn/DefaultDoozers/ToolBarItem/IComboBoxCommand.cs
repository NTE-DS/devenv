// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;

namespace NasuTek.DevEnvironment.Resources.Addins
{
	public interface IComboBoxCommand : ICommand
	{
		bool IsEnabled {
			get;
			set;
		}
		
		object ComboBox {
			get;
			set;
		}
	}
}
