using Raylib_cs;
using MonsterWorld.Entities;
using System.Numerics;
using System.Collections.Generic;

namespace MonsterWorld.Scenes
{
    class World : Scene
    {
        private Map _map;
        private Player _player = new();
        private List<Monster> _monsters = new();
        private Camera2D _camera;

        public override void Load()
        {
            _map = new Map();

            for (int i = 0; i < 20; i++)
            {
                var monster = new Monster();

                while (true)
                {
                    var x = Raylib.GetRandomValue(0, _map.Width - 1);
                    var y = Raylib.GetRandomValue(0, _map.Height - 1);

                    if (_map.GetTile(x, y).IsWalkable)
                    {
                        monster.Position = new Vector2(x * 16, y * 16);
                        break;
                    }
                }

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
            _player.Update(dt, _map);

            _camera.target = _player.Position;

            foreach (var monster in _monsters)
            {
                monster.Update(dt, _map, _player);
            }
        }

        public override void Draw()
        {
            Raylib.BeginMode2D(_camera);

            _map.Draw();

            foreach (var monster in _monsters)
            {
                monster.Draw();
            }

            _player.Draw();

            Raylib.EndMode2D();
        }
    }
}
