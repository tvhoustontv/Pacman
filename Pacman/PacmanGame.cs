using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using Pacman.GameStates.Menu;
using Pacman.GameStates.World;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;

namespace Pacman
{
    public class PacmanGame: Game
    {
        PacmanGameState _gameState;
        IMenu _menu;
        IWorld _world;
        Clock clock;

        public PacmanGame() : base(new Vector2u(448, 496), "Pacman")
        {
        }
        protected override void Initialize()
        {
            _gameState = PacmanGameState.Menu;

            _menu = new Menu();
            _menu.Initialize(Window);
            _menu.MenuItemSelected += MenuItemSelected;

            _world = new World();
            _world.Initialize(Window);
            _world.WorldStateChanged += WorldFieldStateChanged;

            clock = new Clock();
        }
        protected override void Render()
        {
            switch (_gameState)
            {
                case PacmanGameState.Game:
                    _world.DrawAllLayers(Window);
                    break;
                case PacmanGameState.Menu:
                    _menu.DrawAllLayers(Window);
                    break;
            }
        }
        protected override void Update()
        {
            float time = clock.ElapsedTime.AsSeconds();

            switch (_gameState)
            {
                case PacmanGameState.Game:
                    _world.Update(Window, time);
                    break;
                case PacmanGameState.Menu:
                    // for menu animation
                    break;
            }
        }
        protected override void KeyPressed(object sender, KeyEventArgs e)
        {
            switch (_gameState)
            {
                case PacmanGameState.Game:
                    _world.KeyPressed(sender, e);
                    break;
                case PacmanGameState.Menu:
                    _menu.KeyPressed(sender, e);
                    break;
            }
        }
        private void MenuItemSelected(object sender, MenuItemType e)
        {
            switch (e)
            {
                case MenuItemType.NewGame:
                    _world._worldState = WorldState.NewGame;
                    _gameState = PacmanGameState.Game;
                    break;
                case MenuItemType.Continue:
                    _world._worldState = WorldState.Continue;
                    _gameState = PacmanGameState.Game;
                    break;
                case MenuItemType.Quit:
                    Window.Close();
                    break;
            }
        }
        private void WorldFieldStateChanged(object sender, WorldState e)
        {
            switch (e)
            {
                case WorldState.Quit:
                    // for next commit
                    break;
                case WorldState.Pause:
                    _menu.EnableMenuItem(MenuItemType.Continue, true);
                    _gameState = PacmanGameState.Menu;
                    break;
            }
        }

    }
}
