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
	/// <summary>
	/// Allows converting forms to right-to-left layout.
	/// </summary>
	public static class RightToLeftConverter
	{
		public static bool IsRightToLeft { get; set; }
		
		static AnchorStyles Mirror(AnchorStyles anchor)
		{
			bool right = (anchor & AnchorStyles.Right) == AnchorStyles.Right;
			bool left  = (anchor & AnchorStyles.Left ) == AnchorStyles.Left ;
			if (right) {
				anchor = anchor | AnchorStyles.Left;
			} else {
				anchor = anchor & ~AnchorStyles.Left;
			}
			if (left) {
				anchor = anchor | AnchorStyles.Right;
			} else {
				anchor = anchor & ~AnchorStyles.Right;
			}
			return anchor;
		}
		
		static Point MirrorLocation(Control control)
		{
			return new Point(control.Parent.ClientSize.Width - control.Left - control.Width,
			                 control.Top);
		}
		
		/// <summary>
		/// Mirrors a control and its child controls if right to left is activated.
		/// Call this only for controls that aren't mirrored automatically by .NET!
		/// </summary>
		static void Mirror(Control control)
		{
			if (!(control.Parent is SplitContainer)) {
				switch (control.Dock) {
					case DockStyle.Left:
						control.Dock = DockStyle.Right;
						break;
					case DockStyle.Right:
						control.Dock = DockStyle.Left;
						break;
					case DockStyle.None:
						control.Anchor = Mirror(control.Anchor);
						control.Location = MirrorLocation(control);
						break;
				}
			}
			// Panels with RightToLeft = No won't have their children mirrored
			if (control.RightToLeft != RightToLeft.Yes)
				return;
			foreach (Control child in control.Controls) {
				Mirror(child);
			}
		}
		
		public static void Convert(Control control)
		{
			bool isRTL = IsRightToLeft;
			if (isRTL) {
				if (control.RightToLeft != RightToLeft.Yes)
					control.RightToLeft = RightToLeft.Yes;
			} else {
				if (control.RightToLeft == RightToLeft.Yes)
					control.RightToLeft = RightToLeft.No;
			}
			ConvertLayout(control);
		}
		
		static void ConvertLayout(Control control)
		{
			bool isRTL = IsRightToLeft;
			
			DateTimePicker picker = control as DateTimePicker;
			Form form = control as Form;
			ListView listView = control as ListView;
			ProgressBar pg = control as ProgressBar;
			TabControl tc = control as TabControl;
			TrackBar trb = control as TrackBar;
			TreeView treeView = control as TreeView;
			if (form != null && form.RightToLeftLayout != isRTL)
				form.RightToLeftLayout = isRTL;
			if (listView != null && listView.RightToLeftLayout != isRTL)
				listView.RightToLeftLayout = isRTL;
			if (pg != null && pg.RightToLeftLayout != isRTL)
				pg.RightToLeftLayout = isRTL;
			if (tc != null && tc.RightToLeftLayout != isRTL)
				tc.RightToLeftLayout = isRTL;
			if (trb != null && trb.RightToLeftLayout != isRTL)
				trb.RightToLeftLayout = isRTL;
			if (treeView != null && treeView.RightToLeftLayout != isRTL)
				treeView.RightToLeftLayout = isRTL;
		}
		
		static void ConvertLayoutRecursive(Control control)
		{
			bool isRTL = IsRightToLeft;
			if (isRTL == (control.RightToLeft == RightToLeft.Yes)) {
				ConvertLayout(control);
				foreach (Control child in control.Controls) {
					ConvertLayoutRecursive(child);
				}
			}
		}
		
		public static void ConvertRecursive(Control control)
		{
			if (IsRightToLeft == (control.RightToLeft == RightToLeft.Yes)) {
				// already converted
				return;
			}
			ReConvertRecursive(control);
		}
		
		public static void ReConvertRecursive(Control control)
		{
			Convert(control);
			foreach (Control child in control.Controls) {
				ConvertLayoutRecursive(child);
			}
			if (IsRightToLeft) {
				if (control is Form) {
					// direct children seem to be mirrored by .NET
					foreach (Control child in control.Controls) {
						foreach (Control subChild in child.Controls) {
							Mirror(subChild);
						}
					}
				} else {
					foreach (Control child in control.Controls) {
						Mirror(child);
					}
				}
			}
		}
	}
}
