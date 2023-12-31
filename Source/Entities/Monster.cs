using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using MonoGame.Extended.Sprites;
using Raylib_cs;

namespace MonsterWorld.Entities
{
    enum MonsterState
    {
        Idle,
        Thirsty,
        Hungry
    }

    class Monster
    {
        public MonsterData _data;

        private Texture2D _texture;
        private Position[] _path = new Position[0];
        private MonsterState _state = MonsterState.Idle;
        private int _range = Raylib.GetRandomValue(1, 3);
        private float _thirst = 0.0f;
        private float _hunger = 0.0f;
        private float _timer = 0.0f;
        private int _direction = 0;
        private int _counter = 0;
        private int _moveCounter = Raylib.GetRandomValue(3, 8);
        private int _thirstResistance = Raylib.GetRandomValue(10, 40);
        private int _hungerResistance = Raylib.GetRandomValue(10, 40);
        private float _speed = Raylib.GetRandomValue(20, 300) / 100.0f;

        public Position Position = new(0, 0);

        public Monster(MonsterData data)
        {
            _data = data;

            _texture = AssetManager.Instance.GetTexture(_data.SpriteImage);
        }

        public Vector2 PixelPosition
        {
            get
            {
                return new Vector2(Position.X * 16, Position.Y * 16);
            }
        }

        public void Update(float dt, Map map)
        {
            _timer += dt;

            if (_state == MonsterState.Idle)
            {
                Idle(map);
            }
            else if (_state == MonsterState.Thirsty)
            {
                Thirsty(map);
            }
            else if (_state == MonsterState.Hungry)
            {
                Hungry(map);
            }
        }

        public void Draw(Map map)
        {
            var tint = Color.WHITE;

            if (_state == MonsterState.Idle)
            {
                tint = Color.WHITE;
            }
            else if (_state == MonsterState.Thirsty)
            {
                tint = Color.BLUE;
            }
            else if (_state == MonsterState.Hungry)
            {
                tint = Color.ORANGE;
            }

            Raylib.DrawTextureRec(_texture, _data.SpriteFrame, PixelPosition, tint);

            if (_path.Length > 0)
            {
                for (int i = 0; i < _path.Length; i++)
                {
                    Raylib.DrawRectangle(_path[i].X * 16, _path[i].Y * 16, 16, 16, Raylib.Fade(Color.RED, 0.5f));
                }
            }

            var range = map.GetPositionsInRange(Position.X, Position.Y, _range);

            foreach (var position in range)
            {
                Raylib.DrawRectangle(position.X * 16, position.Y * 16, 16, 16, Raylib.Fade(Color.GREEN, 0.1f));
            }
        }

        private void Move(Map map)
        {
            _counter++;

            if (_counter == _moveCounter)
            {
                _direction = Raylib.GetRandomValue(0, 3);
                _moveCounter = Raylib.GetRandomValue(3, 8);
                _counter = 0;
            }

            if (_direction == 0)
            {
                if (map.GetTile(Position.X, Position.Y - 1).IsWalkable)
                {
                    Position.Y--;
                }
                else
                {
                    _direction = Raylib.GetRandomValue(0, 3);
                }
            }
            else if (_direction == 1)
            {
                if (map.GetTile(Position.X, Position.Y + 1).IsWalkable)
                {
                    Position.Y++;
                }
                else
                {
                    _direction = Raylib.GetRandomValue(0, 3);
                }
            }
            else if (_direction == 2)
            {
                if (map.GetTile(Position.X - 1, Position.Y).IsWalkable)
                {
                    Position.X--;
                }
                else
                {
                    _direction = Raylib.GetRandomValue(0, 3);
                }
            }
            else if (_direction == 3)
            {
                if (map.GetTile(Position.X + 1, Position.Y).IsWalkable)
                {
                    Position.X++;
                }
                else
                {
                    _direction = Raylib.GetRandomValue(0, 3);
                }
            }
        }

        private void Idle(Map map)
        {
            _thirst += Raylib.GetFrameTime();
            _hunger += Raylib.GetFrameTime();

            if (_thirst > _thirstResistance)
            {
                _state = MonsterState.Thirsty;
            }

            if (_hunger > _hungerResistance)
            {
                _state = MonsterState.Hungry;
            }

            if (_timer > _speed)
            {
                _timer = 0.0f;

                Move(map);
            }
        }

        private void Thirsty(Map map)
        {
            if (_timer > _speed * 0.5f)
            {
                _timer = 0.0f;

                if (_path.Length == 0)
                {
                    var waterPos = map.GetPositionOfTileInRange(Position.X, Position.Y, _range, TileType.Water);

                    if (waterPos.X == -1 && waterPos.Y == -1)
                    {
                        Move(map);
                    }
                    else
                    {
                        _path = map.FindPath(Position.X, Position.Y, waterPos.X, waterPos.Y);
                    }
                }

                if (_path.Length > 0)
                {
                    Position = _path[0];

                    var newPath = new Position[_path.Length - 1];

                    for (int i = 0; i < newPath.Length; i++)
                    {
                        newPath[i] = _path[i + 1];
                    }

                    _path = newPath;
                }

                if (map.GetTile(Position.X, Position.Y).Type == TileType.Water)
                {
                    _state = MonsterState.Idle;
                    _thirst = 0.0f;
                    _path = new Position[0];
                }
            }
        }

        private void Hungry(Map map)
        {
            if (_timer > _speed * 0.5f)
            {
                _timer = 0.0f;

                if (_path.Length == 0)
                {
                    var foodPos = map.GetPositionOfTileInRange(Position.X, Position.Y, _range, TileType.Bush);

                    if (foodPos.X == -1 && foodPos.Y == -1)
                    {
                        Move(map);
                    }
                    else
                    {
                        _path = map.FindPath(Position.X, Position.Y, foodPos.X, foodPos.Y);
                    }
                }

                if (_path.Length > 0)
                {
                    Position = _path[0];

                    var newPath = new Position[_path.Length - 1];

                    for (int i = 0; i < newPath.Length; i++)
                    {
                        newPath[i] = _path[i + 1];
                    }

                    _path = newPath;
                }

                if (map.GetTile(Position.X, Position.Y).Type == TileType.Bush)
                {
                    _state = MonsterState.Idle;
                    _hunger = 0.0f;
                    _path = new Position[0];
                }
            }
        }
    }
}
