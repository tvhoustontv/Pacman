namespace Pacman.GameStates.Menu
{
    class MenuItem
    {
        public string Name { get; }
        public int Y { get; set; }
        public int Position { get; }
        public MenuItemType Item { get; }
        public bool Enable { get; set; }

        public MenuItem(string name, MenuItemType item, int position, bool enable = true)
        {
            Name = name;
            Item = item;
            Position = position;
            Enable = enable;
            Y = 0;
        }

        public MenuItem(MenuItemType item, int position, bool enable)
            : this(item.ToString(), item, position, enable)
        {
        }
    }
}
