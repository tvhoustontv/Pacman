using SFML.Graphics;
using SFML.System;
using Pacman.DataStructures;

namespace Pacman.GameStates.World.WorldObjects
{
    public abstract class BaseObjectState : Drawable
    {
        protected MoveDirection _moveDirection { get; set; }
        protected MoveDirection _nextMoveDirection { get; set; }

        protected float _objectSpeed;

        protected CircleShape _object;
        protected bool mastChangeDirection;
        protected Point2f point2F;
        public Point2f Point2F
        {
            get
            {
                return point2F;
            }
        }

        public BaseObjectState(Color color, float x, float y)
        {
            _moveDirection = _nextMoveDirection = MoveDirection.Freeze;
            point2F.x = x;
            point2F.y = y;

            _object = new CircleShape(8f);
            _object.FillColor = color;
        }
        public void ChangeDirection(MoveDirection direction)
        {
            if (direction != _moveDirection)
            {
                switch (direction)
                {
                    case MoveDirection.Right:
                        _nextMoveDirection = direction; mastChangeDirection = true;
                        break;
                    case MoveDirection.Left:
                        _nextMoveDirection = direction; mastChangeDirection = true;
                        break;
                    case MoveDirection.Up:
                        _nextMoveDirection = direction; mastChangeDirection = true;
                        break;
                    case MoveDirection.Down:
                        _nextMoveDirection = direction; mastChangeDirection = true;
                        break;
                    case MoveDirection.Freeze:
                        _nextMoveDirection = direction; mastChangeDirection = true;
                        break;
                }
            }
        }
        protected void Move(TileMap tileMap)
        {
            if (mastChangeDirection && InTile())
            {
                switch (_nextMoveDirection)
                {
                    case MoveDirection.Up:
                        if (CanMove(_nextMoveDirection, tileMap, point2F.x, point2F.y - 8f, true))
                        {
                            SetInTile(point2F);
                            _moveDirection = _nextMoveDirection;
                            mastChangeDirection = false;
                        }

                        break;
                    case MoveDirection.Down:
                        if (CanMove(_nextMoveDirection, tileMap, point2F.x, point2F.y, true))
                        {
                            SetInTile(point2F);
                            _moveDirection = _nextMoveDirection;
                            mastChangeDirection = false;
                        }
                        break;
                    case MoveDirection.Left:
                        if (CanMove(_nextMoveDirection, tileMap, point2F.x - 8f, point2F.y, true))
                        {
                            SetInTile(point2F);
                            _moveDirection = _nextMoveDirection;
                            mastChangeDirection = false;
                        }

                        break;
                    case MoveDirection.Right:
                        if (_moveDirection == MoveDirection.Up || _moveDirection == MoveDirection.Left)
                        {
                            if (CanMove(_nextMoveDirection, tileMap, point2F.x, point2F.y, true))
                            {
                                SetInTile(point2F);
                                _moveDirection = _nextMoveDirection;
                                mastChangeDirection = false;
                            }
                        }
                        else
                        {
                            if (CanMove(_nextMoveDirection, tileMap, point2F.x, point2F.y + 8f, true))
                            {
                                SetInTile(point2F);
                                _moveDirection = _nextMoveDirection;
                                mastChangeDirection = false;
                            }
                        }
                        break;
                    case MoveDirection.Freeze:
                        _moveDirection = _nextMoveDirection;
                        mastChangeDirection = false;
                        break;

                }
            }

            switch (_moveDirection)
            {
                case MoveDirection.Up:
                    if (CanMove(_moveDirection, tileMap, point2F.x, point2F.y - _objectSpeed))
                    {
                        point2F.y -= _objectSpeed;
                    }
                    else
                    {
                        point2F.y = ((int)(point2F.y / 16f)) * 16;
                        if (!mastChangeDirection) ChangeDirection(MoveDirection.Freeze);
                    }
                    break;

                case MoveDirection.Down:
                    if (CanMove(_moveDirection, tileMap, point2F.x, point2F.y + _objectSpeed))
                    {
                        point2F.y += _objectSpeed;
                    }
                    else
                    {
                        point2F.y = ((int)((point2F.y + _objectSpeed) / 16f)) * 16;
                        if (!mastChangeDirection) ChangeDirection(MoveDirection.Freeze);
                    }
                    break;

                case MoveDirection.Left:
                    if (point2F.x <= 8) point2F.x = 416f;
                    if (CanMove(_moveDirection, tileMap, point2F.x - _objectSpeed, point2F.y))
                    {
                        point2F.x -= _objectSpeed;
                    }
                    else
                    {
                        point2F.x = ((int)(point2F.x) / 16f) * 16;
                        if (!mastChangeDirection) ChangeDirection(MoveDirection.Freeze);
                    }
                    break;

                case MoveDirection.Right:
                    if (point2F.x >= 424) point2F.x = 0f;
                    if (CanMove(_moveDirection, tileMap, point2F.x + _objectSpeed, point2F.y))
                    {
                        point2F.x += _objectSpeed;
                    }
                    else
                    {
                        point2F.x = ((int)((point2F.x + _objectSpeed) / 16f)) * 16;
                        if (!mastChangeDirection) ChangeDirection(MoveDirection.Freeze);
                    }
                    break;
                case MoveDirection.Freeze:
                    SetInTile(point2F);
                    break;

            }
        }
        private bool CanMove(MoveDirection direction, TileMap tileMap, float x, float y, bool TestNextDir = false)
        {
            switch (direction)
            {
                case MoveDirection.Up:
                    if (tileMap.ReturnTile((int)(y / 16f), (int)(x / 16f)) == Tile.Wall) return false;
                    break;
                case MoveDirection.Down:
                    if (tileMap.ReturnTile((int)((y / 16f) + 1), (int)(x / 16f)) == Tile.Wall) return false;
                    break;
                case MoveDirection.Left:
                    if (tileMap.ReturnTile((int)(y / 16f), (int)((x / 16f))) == Tile.Wall) return false;
                    break;
                case MoveDirection.Right:
                    if (tileMap.ReturnTile((int)(y / 16f), (int)((x / 16f) + 1)) == Tile.Wall) return false;
                    break;
            }
            return true;
        }
        protected void SetInTile(Point2f point2f)
        {
            point2F.x = ((int)(point2f.x / 16f)) * 16;
            point2F.y = ((int)(point2f.y / 16f)) * 16;
        }
        protected bool InTile()
        {
            if (((point2F.x) >= ((int)(point2F.x / 16f) * 16)) && ((point2F.x) < ((int)(point2F.x / 16f) * 16) + 1) &&
                ((point2F.y) >= ((int)(point2F.y / 16f) * 16)) && ((point2F.y) < ((int)(point2F.y / 16f) * 16) + 1))
            {
                return true;
            }
            return false;
        }
        protected bool SamePoints(Point2f firstPoint2f, Point2f secondPoint2F, bool movingObject = false)
        {
            if (!movingObject)
            {
                if ((secondPoint2F.x >= firstPoint2f.x) && (secondPoint2F.x < (firstPoint2f.x + 1f)) &&
                    (secondPoint2F.y >= firstPoint2f.y) && (secondPoint2F.y < (firstPoint2f.y + 1f)))
                {
                    return true;
                }
            }
            else
            {
                if (((firstPoint2f.x - secondPoint2F.x) < 0))
                {
                    return true;
                }
            }

            return false;
        }
        protected bool Collision(Point2f firstPoint2f, Point2f secondPoint2f)
        {
             bool XColl = false;
             bool  YColl = false;

            if ((firstPoint2f.x + 15f >= secondPoint2f.x) && (firstPoint2f.x+1f <= secondPoint2f.x + 15f)) XColl = true;
            if ((firstPoint2f.y + 15f >= secondPoint2f.y) && (firstPoint2f.y+1f <= secondPoint2f.y + 15f)) YColl = true;

            if (XColl & YColl) { return true; }
            return false;
        }
        public abstract void Draw(RenderTarget target, RenderStates states);
    }
}
