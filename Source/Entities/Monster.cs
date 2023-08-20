using System.Numerics;
using AStar;
using Raylib_cs;

namespace MonsterWorld.Entities
{
    enum MonsterState
    {
        Idle,
        Thirsty,
        Chase
    }

    class Monster
    {
        private Texture2D _texture;
        private Rectangle _frame;
        private Vector2[] _path = new Vector2[0];
        private MonsterState _state = MonsterState.Idle;
        private float _timer = 0.0f;
        private float _thirst = 0.0f;

        public Vector2 Position = Vector2.Zero;

        public Monster()
        {
            _texture = AssetManager.Instance.GetTexture("characters.png");

            _frame = new Rectangle(Raylib.GetRandomValue(0, 7) * 16, 0, 16, 16);
        }

        public int TileX
        {
            get
            {
                return (int)Position.X / 16;
            }
        }

        public int TileY
        {
            get
            {
                return (int)Position.Y / 16;
            }
        }

        public void Update(float dt, Map map, Player player)
        {
            _timer += dt;

            _thirst += dt;

            if (_state == MonsterState.Idle)
            {
                Idle(map);
            }
            else if (_state == MonsterState.Chase)
            {
                Chase(map, player);
            }
            else if (_state == MonsterState.Thirsty)
            {
                Thirsty(map);
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
            else if (_state == MonsterState.Thirsty)
            {
                tint = Color.YELLOW;
            }

            Raylib.DrawTextureRec(_texture, _frame, Position, tint);

            if (_path != null)
            {
                for (int i = 0; i < _path.Length; i++)
                {
                    Raylib.DrawRectangle((int)_path[i].X * 16, (int)_path[i].Y * 16, 16, 16, Raylib.Fade(Color.RED, 0.5f));
                }
            }
        }

        private void Idle(Map map)
        {
            if (_thirst > 30.0f)
            {
                _state = MonsterState.Thirsty;
            }

            if (_timer > 1.0f)
            {
                _timer = 0.0f;

                var direction = Raylib.GetRandomValue(0, 3);

                if (direction == 0)
                {
                    if (map.GetTile(TileX, TileY - 1).IsWalkable)
                    {
                        Position.Y -= 16;
                    }
                }
                else if (direction == 1)
                {
                    if (map.GetTile(TileX, TileY + 1).IsWalkable)
                    {
                        Position.Y += 16;
                    }
                }
                else if (direction == 2)
                {
                    if (map.GetTile(TileX - 1, TileY).IsWalkable)
                    {
                        Position.X -= 16;
                    }
                }
                else if (direction == 3)
                {
                    if (map.GetTile(TileX + 1, TileY).IsWalkable)
                    {
                        Position.X += 16;
                    }
                }
            }
        }

        private void Thirsty(Map map)
        {
            // find nearest water tile
            // find path to water tile
            // follow path to water tile
            // drink water
            // find path back to original position
            // follow path back to original position
            // set state to idle
            // reset thirst timer

            if (_path == null)
            {
                var waterTile = FindNearestWaterTile(map);

                if (waterTile.X == -1 && waterTile.Y == -1)
                {
                    _state = MonsterState.Idle;
                    _thirst = 0.0f;
                    return;
                }

                _path = map.FindPath(TileX, TileY, waterTile.X, waterTile.Y);
            }

            if (_path != null && _path.Length > 0)
            {
                if (_timer > 0.5f)
                {
                    _timer = 0;

                    var nextTile = _path[0];

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

                    var newPath = new Vector2[_path.Length - 1];

                    for (int i = 0; i < newPath.Length; i++)
                    {
                        newPath[i] = _path[i + 1];
                    }

                    _path = newPath;
                }
            }

            if (map.GetTile(TileX, TileY).Type == TileType.Water)
            {
                _state = MonsterState.Idle;
                _thirst = 0.0f;
                _path = null;
            }
        }

        private System.Drawing.Point FindNearestWaterTile(Map map)
        {
            var nearestWaterTile = new System.Drawing.Point(-1, -1);
            var nearestDistance = float.MaxValue;

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    if (map.GetTile(x, y).Type == TileType.Water)
                    {
                        var distance = Raymath.Vector2Distance(Position, new Vector2(x * 16, y * 16));

                        if (distance < nearestDistance)
                        {
                            nearestDistance = distance;
                            nearestWaterTile = new System.Drawing.Point(x, y);
                        }
                    }
                }
            }

            return nearestWaterTile;
        }

        private void Chase(Map map, Player player)
        {
            if (Position == player.Position)
            {
                return;
            }

            if (Raymath.Vector2Distance(Position, player.Position) < 15 * 16)
            {
                _path = map.FindPath(TileX, TileY, player.TileX, player.TileY);
            }

            if (_path != null && _path.Length > 0)
            {
                if (Raymath.Vector2Distance(Position, player.Position) < 5 * 16)
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
    }
}
