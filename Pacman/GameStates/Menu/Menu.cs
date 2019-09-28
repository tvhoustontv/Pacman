using System;
using System.Linq;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;
using Pacman.AssetManager;

namespace Pacman.GameStates.Menu
{
    public class Menu : BaseGameState, IMenu
    {
        public event EventHandler<MenuItemType> MenuItemSelected;

        private static Sound _menuSoundBeep;
        private static Sound _menuSoundSelect;
        private MainAssetManager manager;

        private int _centerX;
        private const int MenuFirstItemPositionY = 200;
        private const int MenuNextItemOffsetPositionY = 70;

        private static readonly Color MenuItemsColor = new Color(242, 51, 51, 189);
        private MenuItem _selectedMenuItem;
        private readonly MenuItem[] _menuItems = new MenuItem[]
       {
            new MenuItem(MenuItemType.NewGame, 1, true),
            new MenuItem(MenuItemType.Continue, 2, false),
            new MenuItem(MenuItemType.Quit, 3, true)
       };

        private static Font mainFont;
        public Menu()
        {
            RecalculateMenuItemsPosition(_menuItems);
            _selectedMenuItem = _menuItems[0];
        }
        protected override void LoadContent()
        {
            manager = new MainAssetManager();
            _menuSoundBeep = manager.LoadSound("beep.wav");
            _menuSoundSelect = manager.LoadSound("select.wav");
            mainFont = manager.LoadFont("BadaboomBB_Reg.ttf");
        }
        public void Initialize(RenderWindow target)
        {
            _centerX = (int)target.Size.X / 2;
        }
        public void DrawAllLayers(RenderWindow target)
        {
            DrawMenu(target);
        }
        private void DrawMenu(RenderWindow target)
        {
            foreach (MenuItem menuItem in _menuItems)
            {
                if (menuItem.Enable == true)
                {
                    if (_selectedMenuItem.Item == menuItem.Item)
                    {
                        DrawMenuItem(target, menuItem, _centerX, MenuItemsColor, 50, true, true);
                    }
                    else
                    {
                        DrawMenuItem(target, menuItem, _centerX, MenuItemsColor, 40, false, false);
                    }

                }
            }
        }
        private void RecalculateMenuItemsPosition(MenuItem[] items)
        {
            int index = 0;
            foreach (MenuItem menuItem in _menuItems)
            {
                if (menuItem.Enable)
                {
                    menuItem.Y = MenuFirstItemPositionY + MenuNextItemOffsetPositionY * index++;
                }
            }
        }
        private void DrawMenuItem(RenderTarget target, MenuItem menuItem, int x, Color color, int size, bool bold = true, bool textshadow = false)
        {
            DrawText(target, mainFont, menuItem.Name, x, menuItem.Y, color, size, bold, textshadow);
        }
        public void KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Down || e.Code == Keyboard.Key.S || e.Code == Keyboard.Key.Up || e.Code == Keyboard.Key.W)
            {
                MenuItem nextSelectedMenuItem = _selectedMenuItem;

                if (e.Code == Keyboard.Key.Down || e.Code == Keyboard.Key.S)
                {
                    nextSelectedMenuItem = _menuItems.FirstOrDefault(c => c.Enable && c.Position > _selectedMenuItem.Position);
                }
                else if (e.Code == Keyboard.Key.Up || e.Code == Keyboard.Key.W)
                {
                    nextSelectedMenuItem = _menuItems.OrderByDescending(c => c.Position).FirstOrDefault(c => c.Enable && c.Position < _selectedMenuItem.Position);
                }

                if (nextSelectedMenuItem != null)
                {
                    PlaySound(_menuSoundBeep);
                    _selectedMenuItem = nextSelectedMenuItem;
                }
            }
            if (e.Code == Keyboard.Key.Return)
            {
                if (_selectedMenuItem != null)
                {
                    PlaySound(_menuSoundSelect);
                    MenuItemSelected?.Invoke(this, _selectedMenuItem.Item);
                }
            }

        }
        public void EnableMenuItem(MenuItemType type, bool enable)
        {
            foreach (MenuItem item in _menuItems)
            {
                if (item.Item == type)
                {
                    item.Enable = enable;
                    RecalculateMenuItemsPosition(_menuItems);
                }
            }
        }
    }
}
