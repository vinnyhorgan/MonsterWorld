using System.Globalization;
using System.Numerics;
using AStar;
using Raylib_cs;

namespace MonsterWorld.Entities
{
    enum MonsterState
    {
        Idle,
        Chase,
        Attack
    }

    class Monster
    {
        private Texture2D _texture;
        private Rectangle _frame;
        private System.Drawing.Point[] _path = null;
        private MonsterState _state = MonsterState.Idle;
        private float _timer = 0.0f;

        public Vector2 Position = Vector2.Zero;

        public Monster()
        {
            _texture = AssetManager.Instance.GetTexture("characters.png");

            _frame = new Rectangle(Raylib.GetRandomValue(0, 7) * 16, 0, 16, 16);
        }

        public void Update(float dt, short[,] _world, PathFinder pathFinder, Player player)
        {
            _timer += dt;

            if (_state == MonsterState.Idle)
            {
                Idle(_world);
            }
            else if (_state == MonsterState.Chase)
            {
                Chase(pathFinder, player);
            }
        }

        public void Draw()
        {
            var tint = Color.WHITE;

            if (_state == MonsterState.Idle)
            {
                tint = Color.BLUE;
            }
            else if (_state == MonsterState.Chase)
            {
                tint = Color.RED;
            }
            else if (_state == MonsterState.Attack)
            {
                tint = Color.YELLOW;
            }

            Raylib.DrawTextureRec(_texture, _frame, Position, tint);

            if (_path != null)
            {
                for (int i = 0; i < _path.Length; i++)
                {
                    Raylib.DrawRectangle(_path[i].X * 16, _path[i].Y * 16, 16, 16, Raylib.Fade(Color.RED, 0.5f));
                }
            }
        }

        private void Idle(short[,] world)
        {
            if (_timer > 2.0f)
            {
                _timer = 0.0f;

                var direction = Raylib.GetRandomValue(0, 3);

                if (direction == 0)
                {
                    if (TileY() - 1 >= 0)
                    {
                        if (world[TileY() - 1, TileX()] == 1)
                        {
                            Position.Y -= 16;
                        }
                    }
                }
                else if (direction == 1)
                {
                    if (TileY() + 1 < world.GetLength(0))
                    {
                        if (world[TileY() + 1, TileX()] == 1)
                        {
                            Position.Y += 16;
                        }
                    }
                }
                else if (direction == 2)
                {
                    if (TileX() - 1 >= 0)
                    {
                        if (world[TileY(), TileX() - 1] == 1)
                        {
                            Position.X -= 16;
                        }
                    }
                }
                else if (direction == 3)
                {
                    if (TileX() + 1 < world.GetLength(1))
                    {
                        if (world[TileY(), TileX() + 1] == 1)
                        {
                            Position.X += 16;
                        }
                    }
                }
            }
        }

        private void Chase(PathFinder pathFinder, Player player)
        {
            if (Position == player.Position)
            {
                return;
            }

            if (Raylib_cs.Raymath.Vector2Distance(Position, player.Position) < 15 * 16)
            {
                _path = pathFinder.FindPath(new System.Drawing.Point((int)Position.X / 16, (int)Position.Y / 16), new System.Drawing.Point((int)player.Position.X / 16, (int)player.Position.Y / 16));
            }

            if (_path != null && _path.Length > 0)
            {
                if (Raylib_cs.Raymath.Vector2Distance(Position, player.Position) < 5 * 16)
                {
                    return;
                }
            }


            if (_path != null && _path.Length > 0)
            {
                var nextTile = _path[1];

                if (nextTile.X > Position.X / 16)
                {
                    Position.X += 16;
                }
                else if (nextTile.X < Position.X / 16)
                {
                    Position.X -= 16;
                }
                else if (nextTile.Y > Position.Y / 16)
                {
                    Position.Y += 16;
                }
                else if (nextTile.Y < Position.Y / 16)
                {
                    Position.Y -= 16;
                }
            }
        }

        private int TileX()
        {
            return (int)Position.X / 16;
        }

        private int TileY()
        {
            return (int)Position.Y / 16;
        }
    }
}
