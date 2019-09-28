using System;
using SFML.Graphics;
using SFML.System;
using Pacman.DataStructures;
using Pacman.AssetManager;

namespace Pacman.GameStates.World.WorldObjects
{ 
    class Ghost : BaseObjectState
    {
        private float _time = 0;
        private bool mastfindrandWay;
        private int step = 0;

        int aimI, aimJ;
        int ghostI, ghostJ;
        private Point2f[] toghostDirections;

        private int [,] waveMap;
        private int[,] pathMap;
        public Ghost(Color color, float x, float y, TileMap tileMap) :base(color, x, y)
        {
            _objectSpeed = 0.7f;
            _moveDirection = _nextMoveDirection = MoveDirection.Down;
            point2F.x = x;
            point2F.y = y;
            ghostI = (int)(y / 16f);
            ghostJ = (int)(x / 16f);

            if (ghostJ == 15)
            {
                aimI = 1;
                aimJ = 1;
            }
            if (ghostJ == 14)
            {
                aimI = 1;
                aimJ = 26;
            }
            if (ghostJ == 13)
            {
                aimI = 29;
                aimJ = 1;
            }
            if (ghostJ == 12)
            {
                aimI = 29;
                aimJ = 26;
            }


            _object = new CircleShape(8f);
            _object.FillColor = color;
            _objectSpeed = 0.4f;

            CreateWaveMap(tileMap);
            FindPath(aimI, aimJ, ghostI, ghostJ);
        }
        public void Update(TileMap tileMap, Player player, float time)
        {
            this._time += time;

            if (mastfindrandWay)
            {
                if (this._time < 40000f)
                {
                    RandomNextAim(tileMap);
                }
                else
                {
                    aimI = (int)(player.Point2F.y / 16f);
                    aimJ = (int)(player.Point2F.x / 16f);
                    this._time = 0f;
                }
               CreateWaveMap(tileMap);
               FindPath(aimI,aimJ, (int)(point2F.y/16f), (int)(point2F.x / 16f));
               step = 0;
                mastfindrandWay = false;
            }
            CheckPlayerContact(player);
            MoveToAim(toghostDirections, tileMap);
        }
        private void CreateWaveMap(TileMap tileMap)
        {
            waveMap = new int [31,29];
            pathMap = new int[31, 29];

            for (int i=0; i < 31; i++)
            {
                for (int j=0; j < 28; j++ )
                {
                    if (tileMap.ReturnTile(i, j) == Tile.Wall) { waveMap[i, j] = -1; pathMap[i, j] = -1; }
                    if (tileMap.ReturnTile(i, j) != Tile.Wall) { waveMap[i, j] = 0; pathMap[i, j] = 0; }
                }
            }
        }
        private void FindPath(int aimI, int aimJ, int playerI, int playerJ)
        {
            int d = waveMap[playerI, playerJ] = 1;
            waveMap[aimI, aimJ] = 100;
            bool checkaim = false;

            do
            {
                for (int i = 0; i < 31; i++)
                {
                    for (int j = 0; j < 29; j++)
                    {
                        if ((waveMap[i, j] == d) && !checkaim)
                        {
                            if( ( (j - 1) >=0 ) && ( (j + 1) <= 28 ) && !checkaim)
                            {
                                if ((waveMap[i, j - 1] == 100) && !checkaim) { checkaim = true; waveMap[i, j - 1] = d + 1; }
                                if ((waveMap[i, j - 1] == 0) && !checkaim) { waveMap[i, j - 1] = d + 1; }

                                if ((waveMap[i, j + 1] == 100) && !checkaim) { checkaim = true; waveMap[i, j + 1] = d + 1; }
                                if ((waveMap[i, j + 1] == 0) && !checkaim) { waveMap[i, j + 1] = d + 1; }
                            }

                            if ( ( (i - 1) >=0 ) && ( (i + 1) <= 30 ) && !checkaim)
                            {
                                if ((waveMap[i - 1, j] == 100) && !checkaim) { checkaim = true; waveMap[i - 1, j] = d + 1; }
                                if ((waveMap[i - 1, j] == 0) && !checkaim) { waveMap[i - 1, j] = d + 1; }

                                if ((waveMap[i + 1, j] == 100) && !checkaim) { checkaim = true; waveMap[i + 1, j] = d + 1; }
                                if ((waveMap[i + 1, j] == 0) && !checkaim) { waveMap[i + 1, j] = d + 1; }
                            }
                        }
                    }
                }
                d++;
            }
            while (!checkaim);

            toghostDirections = new Point2f[waveMap[aimI, aimJ]];

            d = pathMap[aimI, aimJ] = waveMap[aimI, aimJ];
            bool checkghost = false;
            bool stepfound = false;
            int pathI = aimI;
            int pathJ = aimJ;
            
            do
            {
                for (int i = 0; i < 31; i++)
                {
                    for (int j = 0; j < 29; j++)
                    {
                        if ((i == pathI) && (j == pathJ) &&(i>0)&&(j>0))
                        {
                        
                        if ((waveMap[i, j] == d) && !checkghost)
                        {
                            if (((i - 1) >= 0) && ((i + 1) <= 30) && ((j - 1) >= 0) && ((j + 1) <= 28) && !checkghost && (d > 0))
                            {
                                if ((waveMap[i, j - 1] == 1) || (waveMap[i, j + 1] == 1) ||
                                    (waveMap[i - 1, j] == 1) || (waveMap[i + 1, j] == 1))
                                {
                                    checkghost = true;
                                        if (waveMap[i, j - 1] == 1)
                                        {
                                            toghostDirections[d - 1].x = (float)(j * 16); toghostDirections[d - 1].y = (float)(i * 16);
                                            toghostDirections[d - 2].x = (float)((j-1) * 16); toghostDirections[d - 2].y = (float)(i * 16);
                                        }
                                        if (waveMap[i, j + 1] == 1)
                                        {
                                            toghostDirections[d - 1].x = (float)(j * 16); toghostDirections[d - 1].y = (float)(i * 16);
                                            toghostDirections[d - 2].x = (float)((j+1) * 16); toghostDirections[d - 2].y = (float)(i * 16);
                                        }
                                        if (waveMap[i - 1, j] == 1)
                                        {
                                            toghostDirections[d - 1].x = (float)(j * 16); toghostDirections[d - 1].y = (float)(i * 16);
                                            toghostDirections[d - 2].x = (float)(j * 16); toghostDirections[d - 2].y = (float)((i-1) * 16);
                                        }
                                        if (waveMap[i + 1, j] == 1)
                                        {
                                            toghostDirections[d - 1].x = (float)(j * 16); toghostDirections[d - 1].y = (float)(i * 16);
                                            toghostDirections[d - 2].x = (float)(j * 16); toghostDirections[d - 2].y = (float)((i+1) * 16);
                                        }
                                    }

                                if (waveMap[i, j - 1] == (d - 1))
                                {
                                    stepfound = true;
                                    pathI = i;
                                    pathJ = j - 1;
                                    toghostDirections[d - 1].x = (float)(j * 16);
                                    toghostDirections[d - 1].y = (float)(i * 16);
                                }
                                if ((waveMap[i, j + 1] == (d - 1)) && !stepfound)
                                {
                                    stepfound = true;
                                    pathI = i;
                                    pathJ = j + 1;
                                    toghostDirections[d - 1].x = (float)(j * 16);
                                    toghostDirections[d - 1].y = (float)(i * 16);
                                }
                                if ((waveMap[i - 1, j] == (d - 1)) && !stepfound)
                                {
                                    stepfound = true;
                                    pathI = i - 1;
                                    pathJ = j;
                                    toghostDirections[d - 1].x = (float)(j * 16);
                                    toghostDirections[d - 1].y = (float)(i * 16);
                                }
                                if ((waveMap[i + 1, j] == (d - 1)) && !stepfound)
                                {
                                    pathI = i + 1;
                                    pathJ = j;
                                    stepfound = true;
                                    toghostDirections[d - 1].x = (float)(j * 16);
                                    toghostDirections[d - 1].y = (float)(i * 16);
                                }
                            }
                        }
                    }
                    }
                }
                d--;
                stepfound = false;
            }
            while (!checkghost);

        }
        private void RandomNextAim(TileMap tileMap)
        {
            Random random = new Random();
            bool getpos = false;

            do
            {
                int i = random.Next(1, 29);
                int j = random.Next(1, 26);

                if (tileMap.ReturnTile(i, j) == Tile.None)
                {
                    getpos = true;
                    aimI = i;
                    aimJ = j;
                }

            }
            while (!getpos);
        }
        private void MoveToAim(Point2f[] toghostDirections, TileMap tileMap)
        {
            Move(tileMap);

            if (SamePoints(toghostDirections[step], point2F) && (step <= toghostDirections.Length))
            {
                SetInTile(point2F);
                if (step < toghostDirections.Length-1)
                {
                    if (toghostDirections[step + 1].x > point2F.x)
                    {
                        ChangeDirection(MoveDirection.Right);
                    }
                    if (toghostDirections[step + 1].x < point2F.x)
                    {
                        ChangeDirection(MoveDirection.Left);
                    }
                    if (toghostDirections[step + 1].y > point2F.y)
                    {
                        ChangeDirection(MoveDirection.Down);
                    }
                    if (toghostDirections[step + 1].y < point2F.y)
                    {
                        ChangeDirection(MoveDirection.Up);
                    }
                }
                step++;
            }

            if ((step == toghostDirections.Length) && InTile())
            {
                mastfindrandWay=true;
                ChangeDirection(MoveDirection.Freeze);
            }
        }  
        private void CheckPlayerContact(Player player)
        {
            if (Collision(point2F, player.Point2F))
            {
                player.GhostContact();
            }
        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            _object.Position = new Vector2f(point2F.x, point2F.y);
            target.Draw(_object);
        }
    }
}
