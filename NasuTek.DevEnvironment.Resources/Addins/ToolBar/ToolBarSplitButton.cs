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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace NasuTek.DevEnvironment.Resources.Addins.WinForms
{
	public class ToolBarSplitButton : ToolStripSplitButton , IStatusUpdate
	{
		object caller;
		Codon codon;
		ArrayList subItems;
		ICommand menuCommand = null;
		Image imgButtonEnabled = null;
		Image imgButtonDisabled = null;
		bool buttonEnabled = true;
		bool dropDownEnabled = true;
		IEnumerable<ICondition> conditions;
		
		public ToolBarSplitButton(Codon codon, object caller, ArrayList subItems, IEnumerable<ICondition> conditions)
		{
			this.RightToLeft = RightToLeft.Inherit;
			this.caller      = caller;
			this.codon       = codon;
			this.subItems	 = subItems;
			this.conditions  = conditions;

			if (codon.Properties.Contains("label")){
				Text = StringParser.Parse(codon.Properties["label"]);
			}
			if (imgButtonEnabled == null && codon.Properties.Contains("icon")) {
				imgButtonEnabled = WinFormsResourceService.GetBitmap(StringParser.Parse(codon.Properties["icon"]));
			}
			if (imgButtonDisabled == null && codon.Properties.Contains("disabledIcon")) {
				imgButtonDisabled = WinFormsResourceService.GetBitmap(StringParser.Parse(codon.Properties["disabledIcon"]));
			}
			if (imgButtonDisabled == null) {
				imgButtonDisabled = imgButtonEnabled;
			}
			menuCommand = codon.AddIn.CreateObject(codon.Properties["class"]) as ICommand;
			menuCommand.Owner = this;
			UpdateStatus();
			UpdateText();
		}

		void CreateDropDownItems()
		{
			ToolStripItem[] itemsToAdd = null;
			
			DropDownItems.Clear();
			foreach (object item in subItems)
			{
				if (item is ToolStripItem)
				{
					DropDownItems.Add((ToolStripItem)item);
					if (item is IStatusUpdate)
					{
						((IStatusUpdate)item).UpdateStatus();
						((IStatusUpdate)item).UpdateText();
					}
				}
				else
				{
					ISubmenuBuilder submenuBuilder = (ISubmenuBuilder)item;
					itemsToAdd = submenuBuilder.BuildSubmenu(codon, caller);
					if (itemsToAdd!=null) {
						DropDownItems.AddRange(itemsToAdd);
					}
				}
			}
		}
		protected override void OnDropDownShow(EventArgs e)
		{
			if (!dropDownEnabled) {
				return;
			}
			if (codon != null && !this.DropDown.Visible)
			{
				CreateDropDownItems();
			}
			base.OnDropDownShow(e);
		}

		protected override void OnButtonClick(EventArgs e)
		{
			if (!buttonEnabled) {
				return;
			}
			base.OnButtonClick(e);
			menuCommand.Run();
		}
		
		public override bool Enabled {
			get {
				if (codon == null) {
					return base.Enabled;
				}
				ConditionFailedAction failedAction = Condition.GetFailedAction(conditions, caller);
				
				bool isEnabled = failedAction != ConditionFailedAction.Disable;
				
				if (menuCommand != null && menuCommand is IMenuCommand) {
					
					// menuCommand.IsEnabled is checked first so that it's get method can set dropDownEnabled as needed
					isEnabled &= (((IMenuCommand)menuCommand).IsEnabled || dropDownEnabled);
				}
				
				return isEnabled;
			}
		}
		
		public bool ButtonEnabled {
			get {
				return buttonEnabled;
			}
			set {
				buttonEnabled = value;
				UpdateButtonImage();
			}
		}
		
		private void UpdateButtonImage()
		{
			//LoggingService.Info("UpdatingButtonImage: buttonEnabled=="+buttonEnabled.ToString());
			Image = buttonEnabled ? imgButtonEnabled : imgButtonDisabled;
		}
		
		public bool DropDownEnabled {
			get {
				return dropDownEnabled;
			}
			set {
				dropDownEnabled = value;
			}
		}
		
		public virtual void UpdateStatus()
		{
			if (codon != null) {
				ConditionFailedAction failedAction = Condition.GetFailedAction(conditions, caller);
				bool isVisible = failedAction != ConditionFailedAction.Exclude;
				if (base.Visible != isVisible) {
					base.Visible = isVisible;
				}
				
				if (this.Visible) {
					if (buttonEnabled && imgButtonEnabled!=null) {
						Image = imgButtonEnabled;
					} else if (imgButtonDisabled!=null) {
						Image = imgButtonDisabled;
					}
				}
				base.Enabled = this.Enabled; // fix for SD2-938 suggested by Matt Ward
			}
		}
		
		public virtual void UpdateText()
		{
			if (codon != null) {
				if (codon.Properties.Contains("tooltip")) {
					ToolTipText = StringParser.Parse(codon.Properties["tooltip"]);
				}
				
				if (codon.Properties.Contains("label")){
					Text = StringParser.Parse(codon.Properties["label"]);
				}
			}
		}
	}
}
