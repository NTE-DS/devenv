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

namespace NasuTek.DevEnvironment.Extensibility.Addins.WinForms
{
	public static class MenuService
	{
		public static void AddItemsToMenu(ToolStripItemCollection collection, object owner, string addInTreePath)
		{
			AddItemsToMenu(collection, AddInTree.BuildItems<MenuItemDescriptor>(addInTreePath, owner, false));
		}
		
		static void AddItemsToMenu(ToolStripItemCollection collection, List<MenuItemDescriptor> descriptors)
		{
			foreach (MenuItemDescriptor descriptor in descriptors) {
				object item = CreateMenuItemFromDescriptor(descriptor);
				if (item is ToolStripItem) {
					collection.Add((ToolStripItem)item);
					if (item is IStatusUpdate)
						((IStatusUpdate)item).UpdateStatus();
				} else {
					ISubmenuBuilder submenuBuilder = (ISubmenuBuilder)item;
					collection.AddRange(submenuBuilder.BuildSubmenu(descriptor.Codon, descriptor.Caller));
				}
			}
		}
		
		static object CreateMenuItemFromDescriptor(MenuItemDescriptor descriptor)
		{
			Codon codon = descriptor.Codon;
			string type = codon.Properties.Contains("type") ? codon.Properties["type"] : "Command";
			bool createCommand = codon.Properties["loadclasslazy"] == "false";
			
			switch (type) {
				case "Separator":
					return new MenuSeparator(codon, descriptor.Caller, descriptor.Conditions);
				case "CheckBox":
					return new MenuCheckBox(codon, descriptor.Caller, descriptor.Conditions);
				case "Item":
				case "Command":
					return new MenuCommand(codon, descriptor.Caller, createCommand, descriptor.Conditions);
				case "Menu":
					return new Menu(codon, descriptor.Caller, ConvertSubItems(descriptor.SubItems), descriptor.Conditions);
				case "Builder":
					return codon.AddIn.CreateObject(codon.Properties["class"]);
				default:
					throw new System.NotSupportedException("unsupported menu item type : " + type);
			}
		}
		
		internal static ArrayList ConvertSubItems(IList items)
		{
			ArrayList r = new ArrayList();
			if (items != null) {
				foreach (MenuItemDescriptor descriptor in items) {
					r.Add(CreateMenuItemFromDescriptor(descriptor));
				}
			}
			return r;
		}
		
		public static ContextMenuStrip CreateContextMenu(object owner, string addInTreePath)
		{
			if (addInTreePath == null) {
				return null;
			}
			try {
				List<MenuItemDescriptor> descriptors = AddInTree.BuildItems<MenuItemDescriptor>(addInTreePath, owner, true);
				ContextMenuStrip contextMenu = new ContextMenuStrip();
				contextMenu.Items.Add(new ToolStripMenuItem("dummy"));
				contextMenu.Opening += delegate {
					contextMenu.Items.Clear();
					AddItemsToMenu(contextMenu.Items, descriptors);
				};
				contextMenu.Opened += ContextMenuOpened;
				contextMenu.Closed += ContextMenuClosed;
				return contextMenu;
			} catch (TreePathNotFoundException) {
				MessageService.ShowError("Warning tree path '" + addInTreePath +"' not found.");
				return null;
			}
		}
		
		static bool isContextMenuOpen;
		
		public static bool IsContextMenuOpen {
			get {
				return isContextMenuOpen;
			}
		}
		
		static void ContextMenuOpened(object sender, EventArgs e)
		{
			isContextMenuOpen = true;
			ContextMenuStrip contextMenu = (ContextMenuStrip)sender;
			foreach (object o in contextMenu.Items) {
				if (o is IStatusUpdate) {
					((IStatusUpdate)o).UpdateStatus();
				}
			}
		}
		
		static void ContextMenuClosed(object sender, EventArgs e)
		{
			isContextMenuOpen = false;
		}
		
		public static void ShowContextMenu(object owner, string addInTreePath, Control parent, int x, int y)
		{
			ContextMenuStrip menu = CreateContextMenu(owner, addInTreePath);
			if (menu != null) {
				menu.Show(parent, new Point(x, y));
			}
		}
		
		class QuickInsertMenuHandler
		{
			TextBoxBase targetControl;
			string      text;
			
			public QuickInsertMenuHandler(TextBoxBase targetControl, string text)
			{
				this.targetControl = targetControl;
				this.text          = text;
			}
			
			public EventHandler EventHandler {
				get {
					return new EventHandler(PopupMenuHandler);
				}
			}
			void PopupMenuHandler(object sender, EventArgs e)
			{
				targetControl.SelectedText += text;
			}
		}
		
		class QuickInsertHandler
		{
			Control               popupControl;
			ContextMenuStrip      quickInsertMenu;
			
			public QuickInsertHandler(Control popupControl, ContextMenuStrip quickInsertMenu)
			{
				this.popupControl    = popupControl;
				this.quickInsertMenu = quickInsertMenu;
				
				popupControl.Click += new EventHandler(showQuickInsertMenu);
			}
			
			void showQuickInsertMenu(object sender, EventArgs e)
			{
				Point cords = new Point(popupControl.Width, 0);
				quickInsertMenu.Show(popupControl, cords);
			}
		}
		
		public static void CreateQuickInsertMenu(TextBoxBase targetControl, Control popupControl, string[,] quickInsertMenuItems)
		{
			ContextMenuStrip contextMenu = new ContextMenuStrip();
			for (int i = 0; i < quickInsertMenuItems.GetLength(0); ++i) {
				if (quickInsertMenuItems[i, 0] == "-") {
					contextMenu.Items.Add(new MenuSeparator());
				} else {
					MenuCommand cmd = new MenuCommand(quickInsertMenuItems[i, 0],
					                                  new QuickInsertMenuHandler(targetControl, quickInsertMenuItems[i, 1]).EventHandler);
					contextMenu.Items.Add(cmd);
				}
			}
			new QuickInsertHandler(popupControl, contextMenu);
		}
	}
}
