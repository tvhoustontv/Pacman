using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Pacman.GameStates.World.WorldObjects;
using Pacman.AssetManager;

namespace Pacman.GameStates.World
{
    public class World: BaseGameState, IWorld
    {
        public WorldState _worldState { get; set; }
        public event EventHandler<WorldState> WorldStateChanged;

        private MainAssetManager manager;
        private static Font mainFont;

        private Player _player;
        private TileMap _tileMap;
        private Score _score;

        private Ghost _blinky;
        private Ghost _pinky;
        private Ghost _inky;
        private Ghost _clyde;

        protected override void LoadContent() // пометка Я перезаписываю...изменить в меню
        {
            manager = new MainAssetManager();
            mainFont = manager.LoadFont("BadaboomBB_Reg.ttf");
            _worldState = WorldState.NewGame;
        }
        public void Initialize(RenderWindow target)
        {
            _score = new Score();
            _tileMap = new TileMap();
            _player = new Player(Color.Yellow, (14f * 16f), (23f * 16f));
            _blinky = new Ghost(Color.Red, (15f * 16f), (14f * 16f), _tileMap);
            _pinky = new Ghost(Color.Magenta, (14f * 16f), (14f * 16f), _tileMap);
            _inky = new Ghost(Color.Blue, (13f * 16f), (14f * 16f), _tileMap);
            _clyde = new Ghost(Color.Yellow, (12f * 16f), (14f * 16f), _tileMap);

            _player.GameOver += GameOver;
            _player.CoinContact += UpdateScore;
        }
        public void Update(RenderWindow target, float time)
        {
            if (_tileMap.Coins < 1)
            {
                _worldState = WorldState.Win;
            }

            if ((_worldState != WorldState.GameOver) && (_worldState != WorldState.Win))
            {
                _player.Update(_tileMap);
                _blinky.Update(_tileMap, _player, time);
                _pinky.Update(_tileMap, _player, time);
                _inky.Update(_tileMap, _player, time);
                _clyde.Update(_tileMap, _player, time);
            }
        }
        public void DrawAllLayers(RenderWindow target)
        {
            if (_worldState == WorldState.NewGame)
            {
                Initialize(target);
            }

             DrawMap(target);
             DrawGhosts(target);
             DrawPlayer(target);
             DrawScore(target);
            if(_worldState == WorldState.GameOver)
            {
                DrawText(target, mainFont, "GAME OVER", 224, 224, Color.Red, 50, true, false);
            }
            if (_worldState == WorldState.Win)
            {
                DrawText(target, mainFont, "WIN !!!", 224, 224, Color.Red, 50, true, false);
            }
        }
        private void DrawMap(RenderWindow target)
        {
            target.Draw(_tileMap);
        }
        private void DrawGhosts(RenderWindow target)
        {
            target.Draw(_blinky);
            target.Draw(_pinky);
            target.Draw(_inky);
            target.Draw(_clyde);
        }
        private void DrawPlayer(RenderWindow target)
        {
            target.Draw(_player);
        }
        private void DrawScore(RenderTarget target, int x = 60, int y = 0, int size = 16, bool bold = true, bool textshadow = false)
        {
           DrawText(target, mainFont, "score:"+_score.SCORE, x, y, Color.Red, size, bold, textshadow);
        }
        public void KeyPressed(object sender, KeyEventArgs e)
        {
                if (e.Code == Keyboard.Key.D || e.Code == Keyboard.Key.Right ||
                e.Code == Keyboard.Key.A || e.Code == Keyboard.Key.Left ||
                e.Code == Keyboard.Key.W || e.Code == Keyboard.Key.Up ||
                e.Code == Keyboard.Key.S || e.Code == Keyboard.Key.Down)
                {
                    if (_worldState == WorldState.NewGame || _worldState == WorldState.Continue)
                    {
                        _worldState = WorldState.Playing;
                    }

                    if (e.Code == Keyboard.Key.D || e.Code == Keyboard.Key.Right)
                    {
                    _player.ChangeDirection(MoveDirection.Right);
                    }
                    if (e.Code == Keyboard.Key.A || e.Code == Keyboard.Key.Left)
                    {
                    _player.ChangeDirection(MoveDirection.Left);
                    }
                    if (e.Code == Keyboard.Key.W || e.Code == Keyboard.Key.Up)
                    {
                    _player.ChangeDirection(MoveDirection.Up);
                    }
                    if (e.Code == Keyboard.Key.S || e.Code == Keyboard.Key.Down)
                    {
                    _player.ChangeDirection(MoveDirection.Down);
                    }
                }
                if (e.Code == Keyboard.Key.Escape)
                {
                    _worldState = WorldState.Pause;
                    WorldStateChanged(this, _worldState);
                } 
        }
        private void UpdateScore(object sender, int value)
        {
            _score.SCORE += value;
        }
        private void GameOver(object sender, WorldState worldState)
        {
            _worldState = worldState;
        }
    }
}
