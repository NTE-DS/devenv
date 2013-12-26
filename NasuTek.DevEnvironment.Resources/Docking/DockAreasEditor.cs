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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace NasuTek.DevEnvironment.Resources.Docking
{	
	internal class DockAreasEditor : UITypeEditor
	{
		private class DockAreasEditorControl : System.Windows.Forms.UserControl
		{
			private CheckBox checkBoxFloat;
			private CheckBox checkBoxDockLeft;
			private CheckBox checkBoxDockRight;
			private CheckBox checkBoxDockTop;
			private CheckBox checkBoxDockBottom;
			private CheckBox checkBoxDockFill;
			private DockAreas m_oldDockAreas;

			public DockAreas DockAreas
			{
				get
				{
					DockAreas dockAreas = 0;
					if (checkBoxFloat.Checked)
						dockAreas |= DockAreas.Float;
					if (checkBoxDockLeft.Checked)
						dockAreas |= DockAreas.DockLeft;
					if (checkBoxDockRight.Checked)
						dockAreas |= DockAreas.DockRight;
					if (checkBoxDockTop.Checked)
						dockAreas |= DockAreas.DockTop;
					if (checkBoxDockBottom.Checked)
						dockAreas |= DockAreas.DockBottom;
					if (checkBoxDockFill.Checked)
						dockAreas |= DockAreas.Document;

					if (dockAreas == 0)
						return m_oldDockAreas;
					else
						return dockAreas;
				}
			}

			public DockAreasEditorControl()
			{
				checkBoxFloat = new CheckBox();
				checkBoxDockLeft = new CheckBox();
				checkBoxDockRight = new CheckBox();
				checkBoxDockTop = new CheckBox();
				checkBoxDockBottom = new CheckBox();
				checkBoxDockFill = new CheckBox();

				SuspendLayout();

				checkBoxFloat.Appearance = Appearance.Button;
				checkBoxFloat.Dock = DockStyle.Top;
				checkBoxFloat.Height = 24;
				checkBoxFloat.Text = Strings.DockAreaEditor_FloatCheckBoxText;
				checkBoxFloat.TextAlign = ContentAlignment.MiddleCenter;
				checkBoxFloat.FlatStyle = FlatStyle.System;
			
				checkBoxDockLeft.Appearance = System.Windows.Forms.Appearance.Button;
				checkBoxDockLeft.Dock = System.Windows.Forms.DockStyle.Left;
				checkBoxDockLeft.Width = 24;
				checkBoxDockLeft.FlatStyle = FlatStyle.System;

				checkBoxDockRight.Appearance = System.Windows.Forms.Appearance.Button;
				checkBoxDockRight.Dock = System.Windows.Forms.DockStyle.Right;
				checkBoxDockRight.Width = 24;
				checkBoxDockRight.FlatStyle = FlatStyle.System;

				checkBoxDockTop.Appearance = System.Windows.Forms.Appearance.Button;
				checkBoxDockTop.Dock = System.Windows.Forms.DockStyle.Top;
				checkBoxDockTop.Height = 24;
				checkBoxDockTop.FlatStyle = FlatStyle.System;

				checkBoxDockBottom.Appearance = System.Windows.Forms.Appearance.Button;
				checkBoxDockBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
				checkBoxDockBottom.Height = 24;
				checkBoxDockBottom.FlatStyle = FlatStyle.System;
			
				checkBoxDockFill.Appearance = System.Windows.Forms.Appearance.Button;
				checkBoxDockFill.Dock = System.Windows.Forms.DockStyle.Fill;
				checkBoxDockFill.FlatStyle = FlatStyle.System;

				this.Controls.AddRange(new Control[] {
														 checkBoxDockFill,
														 checkBoxDockBottom,
														 checkBoxDockTop,
														 checkBoxDockRight,
														 checkBoxDockLeft,
														 checkBoxFloat});

				Size = new System.Drawing.Size(160, 144);
				BackColor = SystemColors.Control;
				ResumeLayout();
			}

			public void SetStates(DockAreas dockAreas)
			{
				m_oldDockAreas = dockAreas;
				if ((dockAreas & DockAreas.DockLeft) != 0)
					checkBoxDockLeft.Checked = true;
				if ((dockAreas & DockAreas.DockRight) != 0)
					checkBoxDockRight.Checked = true;
				if ((dockAreas & DockAreas.DockTop) != 0)
					checkBoxDockTop.Checked = true;
				if ((dockAreas & DockAreas.DockTop) != 0)
					checkBoxDockTop.Checked = true;
				if ((dockAreas & DockAreas.DockBottom) != 0)
					checkBoxDockBottom.Checked = true;
				if ((dockAreas & DockAreas.Document) != 0)
					checkBoxDockFill.Checked = true;
				if ((dockAreas & DockAreas.Float) != 0)
					checkBoxFloat.Checked = true;
			}
		}

		private DockAreasEditor.DockAreasEditorControl m_ui = null;

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider sp, object value)
		{
			if (m_ui == null)
				m_ui = new DockAreasEditor.DockAreasEditorControl();

			m_ui.SetStates((DockAreas)value);

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)sp.GetService(typeof(IWindowsFormsEditorService));
			edSvc.DropDownControl(m_ui);

			return m_ui.DockAreas;
		}
	}
}
