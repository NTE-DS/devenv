using System;
using System.Windows.Forms;

namespace NasuTek.DevEnvironment.Resources.Docking
{
	internal class DummyControl : Control
	{
		public DummyControl()
		{
			SetStyle(ControlStyles.Selectable, false);
		}
	}
}
