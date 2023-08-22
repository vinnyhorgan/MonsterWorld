using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using MonsterWorld.Entities;
using Newtonsoft.Json;

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

            for (int i = 0; i < 10; i++)
            {
                var monster = new Monster();

                while (true)
                {
                    var x = Raylib.GetRandomValue(0, _map.Width - 1);
                    var y = Raylib.GetRandomValue(0, _map.Height - 1);

                    if (_map.GetTile(x, y).IsWalkable)
                    {
                        monster.Position = new Position(x, y);
                        break;
                    }
                }

                _monsters.Add(monster);
            }

            _camera = new Camera2D
            {
                target = _player.PixelPosition,
                offset = new Vector2(GameWidth / 2, GameHeight / 2),
                rotation = 0.0f,
                zoom = 1.0f
            };

            var monsters = AssetManager.Instance.GetData("monsters.json");
            List<Monster> monsterList = JsonConvert.DeserializeObject<List<Monster>>(monsters);

            foreach (var monster in monsterList)
            {
                Logger.Info($"Monster: {monster.Name}");
                Logger.Info($"Description: {monster.Description}");
                Logger.Info($"SpriteImage: {monster.SpriteImage}");
                Logger.Info($"SpriteFrame: {monster.SpriteFrame}");
                Logger.Info($"FullImage: {monster.FullImage}");
                Logger.Info($"FullFrame: {monster.FullFrame}");
            }
        }

        public override void Update(float dt)
        {
            _player.Update(dt, _map);

            _camera.target = _player.PixelPosition;

            foreach (var monster in _monsters)
            {
                monster.Update(dt, _map);
            }
        }

        public override void Draw()
        {
            Raylib.BeginMode2D(_camera);

            _map.Draw();

            foreach (var monster in _monsters)
            {
                monster.Draw(_map);
            }

            _player.Draw();

            Raylib.EndMode2D();

            Raylib.DrawText("WASD to move", 10, 10, 10, Color.WHITE);
        }
    }
}
