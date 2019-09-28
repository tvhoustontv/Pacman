using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Pacman.GameStates.World.WorldObjects
{
    public enum Tile
    {
        Wall,
        Coin,
        Energizer,
        GhostHouse,
        None
    }

    public class TileMap : Drawable
    {
        private int coins = 244;
        public int Coins
        {
            get 
            {
                return coins;
            }
            set
            {
                coins = value;
            }
        }

        private const int mapWidth = 28;
        private const int mapHigh = 31;

        private RectangleShape coin_rectangle;
        private RectangleShape wall_rectangle;
        private RectangleShape energizer_rectangle;
        private RectangleShape ghosthouse_rectangle;

        private int[,] Map = new int[mapHigh, mapWidth] {

            { 10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10 },
            { 10,01,01,01,01,01,01,01,01,01,01,01,01,10,10,01,01,01,01,01,01,01,01,01,01,01,01,10 },
            { 10,01,10,10,10,10,01,10,10,10,10,10,01,10,10,01,10,10,10,10,10,01,10,10,10,10,01,10 },
            { 10,02,10,10,10,10,01,10,10,10,10,10,01,10,10,01,10,10,10,10,10,01,10,10,10,10,02,10 },
            { 10,01,10,10,10,10,01,10,10,10,10,10,01,10,10,01,10,10,10,10,10,01,10,10,10,10,01,10 },
            { 10,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,10 },
            { 10,01,10,10,10,10,01,10,10,01,10,10,10,10,10,10,10,10,01,10,10,01,10,10,10,10,01,10 },
            { 10,01,10,10,10,10,01,10,10,01,10,10,10,10,10,10,10,10,01,10,10,01,10,10,10,10,01,10 },
            { 10,01,01,01,01,01,01,10,10,01,01,01,01,10,10,01,01,01,01,10,10,01,01,01,01,01,01,10 },
            { 10,10,10,10,10,10,01,10,10,10,10,10,00,10,10,00,10,10,10,10,10,01,10,10,10,10,10,10 },
            { 10,10,10,10,10,10,01,10,10,10,10,10,00,10,10,00,10,10,10,10,10,01,10,10,10,10,10,10 },
            { 10,10,10,10,10,10,01,10,10,00,00,00,00,00,00,00,00,00,00,10,10,01,10,10,10,10,10,10 },
            { 10,10,10,10,10,10,01,10,10,00,10,10,00,00,00,00,10,10,00,10,10,01,10,10,10,10,10,10 },
            { 10,10,10,10,10,10,01,10,10,00,10,10,00,00,00,00,10,10,00,10,10,01,10,10,10,10,10,10 },
            { 00,00,00,00,00,00,01,00,00,00,10,10,00,00,00,00,10,10,00,00,00,01,00,00,00,00,00,00 },
            { 10,10,10,10,10,10,01,10,10,00,10,10,10,10,10,10,10,10,00,10,10,01,10,10,10,10,10,10 },
            { 10,10,10,10,10,10,01,10,10,00,10,10,10,10,10,10,10,10,00,10,10,01,10,10,10,10,10,10 },
            { 10,10,10,10,10,10,01,10,10,00,00,00,00,00,00,00,00,00,00,10,10,01,10,10,10,10,10,10 },
            { 10,10,10,10,10,10,01,10,10,00,10,10,10,10,10,10,10,10,00,10,10,01,10,10,10,10,10,10 },
            { 10,10,10,10,10,10,01,10,10,00,10,10,10,10,10,10,10,10,00,10,10,01,10,10,10,10,10,10 },
            { 10,01,01,01,01,01,01,01,01,01,01,01,01,10,10,01,01,01,01,01,01,01,01,01,01,01,01,10 },
            { 10,01,10,10,10,10,01,10,10,10,10,10,01,10,10,01,10,10,10,10,10,01,10,10,10,10,01,10 },
            { 10,01,10,10,10,10,01,10,10,10,10,10,01,10,10,01,10,10,10,10,10,01,10,10,10,10,01,10 },
            { 10,02,01,01,10,10,01,01,01,01,01,01,01,00,00,01,01,01,01,01,01,01,10,10,01,01,02,10 },
            { 10,10,10,01,10,10,01,10,10,01,10,10,10,10,10,10,10,10,01,10,10,01,10,10,01,10,10,10 },
            { 10,10,10,01,10,10,01,10,10,01,10,10,10,10,10,10,10,10,01,10,10,01,10,10,01,10,10,10 },
            { 10,01,01,01,01,01,01,10,10,01,01,01,01,10,10,01,01,01,01,10,10,01,01,01,01,01,01,10 },
            { 10,01,10,10,10,10,10,10,10,10,10,10,01,10,10,01,10,10,10,10,10,10,10,10,10,10,01,10 },
            { 10,01,10,10,10,10,10,10,10,10,10,10,01,10,10,01,10,10,10,10,10,10,10,10,10,10,01,10 },
            { 10,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,10 },
            { 10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10 } };

        private Tile[,] _tileMap = new Tile[mapHigh, mapWidth];

        public TileMap()
        {
            wall_rectangle = new RectangleShape(new Vector2f(16f, 16f));
            wall_rectangle.FillColor = Color.Black;
            wall_rectangle.OutlineColor = Color.Blue;
            wall_rectangle.OutlineThickness = -4f;

            coin_rectangle = new RectangleShape(new Vector2f(16f, 16f));
            coin_rectangle.FillColor = Color.Yellow;
            coin_rectangle.OutlineColor = Color.Black;
            coin_rectangle.OutlineThickness = -7f;

            energizer_rectangle = new RectangleShape(new Vector2f(16f, 16f));
            energizer_rectangle.FillColor = Color.Yellow;
            energizer_rectangle.OutlineColor = Color.Black;
            energizer_rectangle.OutlineThickness = -5f;

            ghosthouse_rectangle = new RectangleShape(new Vector2f(16f, 16f));
            ghosthouse_rectangle.FillColor = Color.Black;



            FillTileMap(Map);
        }
        private void FillTileMap(int[,] Map)
        {
            int b = 0;
            for (int i = 0; i < mapHigh; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    if (Map[i, j] == 10) _tileMap[i, j] = Tile.Wall;
                    if (Map[i, j] == 00) _tileMap[i, j] = Tile.None;
                    if (Map[i, j] == 01) _tileMap[i, j] = Tile.Coin;
                    if (Map[i, j] == 02) _tileMap[i, j] = Tile.Energizer;
                    if (Map[i, j] == 15) _tileMap[i, j] = Tile.GhostHouse;
                }
            }
        }
        public Tile ReturnTile(int i, int j)
        {
            return _tileMap[i,j];
        }
        public void ChangeTileToNone(int i, int j)
        {
            _tileMap[i, j] = Tile.None;
        }
        void Drawable.Draw(RenderTarget target, RenderStates states)
        {
            for (int i = 0; i < mapHigh; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    if (_tileMap[i, j] == Tile.Wall)
                    {
                        wall_rectangle.Position = new Vector2f((float)(j * 16), (float)(i * 16));
                        target.Draw(wall_rectangle);
                    }

                    if (_tileMap[i, j] == Tile.Coin)
                    {
                        coin_rectangle.Position = new Vector2f((float)(j * 16), (float)(i * 16));
                        target.Draw(coin_rectangle);
                    }

                    if (_tileMap[i, j] == Tile.Energizer)
                    {
                        energizer_rectangle.Position = new Vector2f((float)(j * 16), (float)(i * 16));
                        target.Draw(energizer_rectangle);
                    }

                    if (_tileMap[i, j] == Tile.GhostHouse)
                    {
                        ghosthouse_rectangle.Position = new Vector2f((float)(j * 16), (float)(i * 16));
                        target.Draw(ghosthouse_rectangle);
                    }
                }
            }
        }
    }
}
