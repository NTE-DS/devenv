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
using System.Drawing;
using System.Windows.Forms;

namespace NasuTek.DevEnvironment.Extensibility.Addins.WinForms
{
	sealed class CustomDialog : System.Windows.Forms.Form
	{
		System.Windows.Forms.Label label;
		System.Windows.Forms.Panel panel;
		int      acceptButton;
		int      cancelButton;
		int result = -1;
		
		/// <summary>
		/// Gets the index of the button pressed.
		/// </summary>
		public int Result {
			get {
				return result;
			}
		}
		
		public CustomDialog(string caption, string message, int acceptButton, int cancelButton, string[] buttonLabels)
		{
			this.SuspendLayout();
			MyInitializeComponent();
			
			this.Icon = null;
			this.acceptButton = acceptButton;
			this.cancelButton = cancelButton;
			
			message = StringParser.Parse(message);
			this.Text = StringParser.Parse(caption);
			
			using (Graphics g = this.CreateGraphics()) {
				Rectangle screen = Screen.PrimaryScreen.WorkingArea;
				SizeF size = g.MeasureString(message, label.Font, screen.Width - 20);
				Size clientSize = size.ToSize();
				Button[] buttons = new Button[buttonLabels.Length];
				int[] positions = new int[buttonLabels.Length];
				int pos = 0;
				for (int i = 0; i < buttons.Length; i++) {
					Button newButton = new Button();
					newButton.FlatStyle = FlatStyle.System;
					newButton.Tag = i;
					string buttonLabel = StringParser.Parse(buttonLabels[i]);
					newButton.Text = buttonLabel;
					newButton.Click += new EventHandler(ButtonClick);
					SizeF buttonSize = g.MeasureString(buttonLabel, newButton.Font);
					newButton.Width = Math.Max(newButton.Width, ((int)Math.Ceiling(buttonSize.Width / 8.0) + 1) * 8);
					positions[i] = pos;
					buttons[i] = newButton;
					pos += newButton.Width + 4;
				}
				if (acceptButton >= 0) {
					AcceptButton = buttons[acceptButton];
				}
				if (cancelButton >= 0) {
					CancelButton = buttons[cancelButton];
				}
				
				pos += 4; // add space before first button
				// (we don't start with pos=4 because this space doesn't belong to the button panel)
				
				if (pos > clientSize.Width) {
					clientSize.Width = pos;
				}
				clientSize.Height += panel.Height + 6;
				this.ClientSize = clientSize;
				int start = (clientSize.Width - pos) / 2;
				for (int i = 0; i < buttons.Length; i++) {
					buttons[i].Location = new Point(start + positions[i], 4);
				}
				panel.Controls.AddRange(buttons);
			}
			label.Text = message;
			
			RightToLeftConverter.ConvertRecursive(this);
			this.ResumeLayout(false);
		}
		
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (cancelButton == -1 && e.KeyCode == Keys.Escape) {
				this.Close();
			}
		}
		
		void ButtonClick(object sender, EventArgs e) 
		{
			result = (int)((Control)sender).Tag;
			this.Close();
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		void MyInitializeComponent()
		{
			this.panel = new System.Windows.Forms.Panel();
			this.label = new System.Windows.Forms.Label();
			// 
			// panel
			// 
			this.panel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel.Location = new System.Drawing.Point(4, 80);
			this.panel.Name = "panel";
			this.panel.Size = new System.Drawing.Size(266, 32);
			this.panel.TabIndex = 0;
			// 
			// label
			// 
			this.label.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label.Location = new System.Drawing.Point(4, 4);
			this.label.Name = "label";
			this.label.Size = new System.Drawing.Size(266, 76);
			this.label.TabIndex = 1;
			this.label.UseMnemonic = false;
			// 
			// CustomDialog
			// 
			this.ClientSize = new System.Drawing.Size(274, 112);
			this.Controls.Add(this.label);
			this.Controls.Add(this.panel);
			this.DockPadding.Left = 4;
			this.DockPadding.Right = 4;
			this.DockPadding.Top = 4;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.ShowInTaskbar = false;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CustomDialog";
			this.KeyPreview = true;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "CustomDialog";
		}
	}
}
