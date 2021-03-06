using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NasuTek.DevEnvironment.Extensibility
{
    public class MenuItem
    {
        public enum MenuType
        {
            Command,
            Menu,
            Seperator,
        }

        public List<MenuItem> SubItems { get; private set; }
        public string Id { get; private set; }
        public string Text { get; private set; }
        public AbstractCommand Command { get; private set; }
        public MenuType Type { get; private set; }
        public Image Icon { get; set; }

        public MenuItem(string id) : this(id, null, null) { }

        public MenuItem(string id, string title) : this(id, title, null) { }

        public MenuItem(string id, string title, AbstractCommand command)
        {
            SubItems = new List<MenuItem>();
            Id = id;
            Text = title;
            Command = command;

            if (command == null)
                Type = MenuType.Menu;

            if (title == null)
                Type = MenuType.Seperator;
        }

        public MenuItem GetMenuItem(string id)
        {
            return SubItems.FirstOrDefault(v => v.Id == id);
        }
    }
}
