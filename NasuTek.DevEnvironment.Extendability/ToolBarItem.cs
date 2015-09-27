using NasuTek.DevEnvironment.Extensibility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NasuTek.DevEnvironment.Extensibility
{
    public class ToolBarItem
    {
        public enum ToolBarItemType
        {
            Button,
            Menu,
            Seperator,
        }

        public string Text { get; private set; }
        public Image Image { get; private set; }
        public AbstractCommand Command { get; private set; }
        public MenuItem SubMenu { get; private set; }
        public ToolBarItemType Type { get; private set; }

        public ToolBarItem(string id) : this(id, null, null, null, null) { }
        public ToolBarItem(string id, string text, Image icon, AbstractCommand command) : this(id, text, icon, command, null) { }

        public ToolBarItem(string id, string text, Image icon, AbstractCommand command, MenuItem subMenu)
        {
            Text = text;
            Image = icon;
            Command = command;
            SubMenu = subMenu;

            Type = ToolBarItemType.Button;

            if (subMenu != null)
                Type = ToolBarItemType.Menu;

            if (text == null && icon == null && command == null && subMenu == null)
                Type = ToolBarItemType.Seperator;
        }
    }
}
