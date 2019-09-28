using System;
using SFML.Graphics;
using SFML.System;

namespace Pacman.GameStates.World.WorldObjects
{
    class Player: BaseObjectState
    {
        public event EventHandler<WorldState> GameOver;
        public event EventHandler<int> CoinContact;

        private int lives;
        private CircleShape liveRectangle;
        public Player(Color color, float x, float y) :base(color, x, y)
        {
            _objectSpeed = 0.7f;
            lives = 3;

            liveRectangle = new CircleShape(8f)
            {
                FillColor = color
            };

        }
        public void Update(TileMap tileMap)
        {
            Move(tileMap);
            CheckCoinsContact(tileMap, _moveDirection);
        }
        private void CheckCoinsContact(TileMap tileMap, MoveDirection moveDirection)
        {
            if (InTile())
            {
                if (tileMap.ReturnTile((int)(point2F.y / 16f),(int)(point2F.x / 16f)) == Tile.Coin)
                {
                    CoinContact(this, 10);
                    tileMap.ChangeTileToNone((int)(point2F.y / 16f), (int)(point2F.x / 16f));
                    tileMap.Coins--;
                }
                if (tileMap.ReturnTile((int)(point2F.y / 16f), (int)(point2F.x / 16f)) == Tile.Energizer)
                {
                    CoinContact(this, 50);
                    tileMap.ChangeTileToNone((int)(point2F.y / 16f), (int)(point2F.x / 16f));
                    tileMap.Coins--;
                }
            }

        }
        public void GhostContact()
        {
            if (lives < 1)
            {
                GameOver(this, WorldState.GameOver);
            }
            lives -= 1;
            point2F.x = 14f * 16f;
            point2F.y = 23f * 16f;
            ChangeDirection(MoveDirection.Freeze);
        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            _object.Position = new Vector2f(point2F.x, point2F.y);
            target.Draw(_object);
            for (int i = 1; i <= lives; i++)
            {
                liveRectangle.Position = new Vector2f(240+(i*16f), 0f);
                target.Draw(liveRectangle);
            }
        }
    }
}
