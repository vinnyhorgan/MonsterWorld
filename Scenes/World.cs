using Raylib_cs;
using MonsterWorld.Entities;
using AStar;
using System.Numerics;
using AStar.Options;
using System.Collections.Generic;

namespace MonsterWorld.Scenes
{
    class World : Scene
    {
        private Texture2D _tiles;
        private short[,] _world;
        private PathFinder _pathFinder;
        private Player _player = new();
        private Rectangle _rock = new(0, 16, 16, 16);
        private Rectangle _grass = new(16 * 6, 16, 16, 16);
        private List<Monster> _monsters = new();
        private Camera2D _camera;
        private float _timer;

        public override void Load()
        {
            _tiles = AssetManager.Instance.GetTexture("tiles.png");

            _world = new short[100, 100];

            for (int row = 0; row < _world.GetLength(0); row++)
            {
                for (int col = 0; col < _world.GetLength(1); col++)
                {
                    int randomNumber = Raylib.GetRandomValue(0, 100);

                    if (randomNumber < 10)
                    {
                        _world[row, col] = 0;
                    }
                    else
                    {
                        _world[row, col] = 1;
                    }
                }
            }

            var pathfinderOptions = new PathFinderOptions {
                PunishChangeDirection = true,
                UseDiagonals = false
            };

            var worldGrid = new WorldGrid(_world);
            _pathFinder = new PathFinder(worldGrid, pathfinderOptions);

            for (int i = 0; i < 20; i++)
            {
                var monster = new Monster();

                while (true)
                {
                    var x = Raylib.GetRandomValue(0, _world.GetLength(0) - 1);
                    var y = Raylib.GetRandomValue(0, _world.GetLength(1) - 1);

                    Logger.Info($"Trying {x}, {y}");

                    if (_world[x, y] == 1)
                    {
                        monster.Position = new Vector2(x * 16, y * 16);
                        break;
                    }
                }

                Logger.Info($"Monster {i} at {monster.Position / 16}");

                _monsters.Add(monster);
            }

            _camera = new Camera2D
            {
                target = _player.Position,
                offset = new Vector2(GameWidth / 2, GameHeight / 2),
                rotation = 0.0f,
                zoom = 1.0f
            };
        }

        public override void Update(float dt)
        {
            _timer += dt;

            if (_timer > 0.1)
            {
                _player.Update(dt, _world);

                _camera.target = _player.Position;

                foreach (var monster in _monsters)
                {
                    monster.Update(dt, _world, _pathFinder, _player);
                }

                _timer = 0;
            }
        }

        public override void Draw()
        {
            Raylib.BeginMode2D(_camera);

            // draw map loop y first
            for (int row = 0; row < _world.GetLength(0); row++)
            {
                for (int col = 0; col < _world.GetLength(1); col++)
                {
                    Raylib.DrawTextureRec(_tiles, _grass, new Vector2(col * 16, row * 16), Color.WHITE);

                    if (_world[row, col] == 0)
                    {
                        Raylib.DrawTextureRec(_tiles, _rock, new Vector2(col * 16, row * 16), Color.WHITE);
                    }
                }
            }

            foreach (var monster in _monsters)
            {
                monster.Draw();
            }

            _player.Draw();

            Raylib.EndMode2D();
        }
    }
}
