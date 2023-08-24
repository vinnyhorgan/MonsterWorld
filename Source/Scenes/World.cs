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
        private Monster _selectedMonster;

        public override void Load()
        {
            _map = new Map();

            var monsterDataFile = AssetManager.Instance.GetData("monsters.json");
            List<MonsterData> monsterData = JsonConvert.DeserializeObject<List<MonsterData>>(monsterDataFile);

            for (int i = 0; i < 10; i++)
            {
                var monster = new Monster(monsterData[Raylib.GetRandomValue(0, monsterData.Count - 1)]);

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
        }

        public override void Update(float dt)
        {
            _player.Update(dt, _map);

            _camera.target = _player.PixelPosition;

            foreach (var monster in _monsters)
            {
                monster.Update(dt, _map);
            }

            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                _selectedMonster = null;
            }
        }

        public override void Draw()
        {
            Raylib.BeginMode2D(_camera);

            _map.Draw();

            foreach (var monster in _monsters)
            {
                monster.Draw(_map);

                if (monster == _selectedMonster)
                {
                    Raylib.DrawRectangle((int)monster.PixelPosition.X, (int)monster.PixelPosition.Y, 16, 16, Raylib.Fade(Color.RED, 0.5f));

                    continue;
                }

                if (Raylib.CheckCollisionPointRec(Raylib.GetScreenToWorld2D(Mouse, _camera), new Rectangle(monster.PixelPosition.X, monster.PixelPosition.Y, 16, 16)))
                {
                    Raylib.DrawRectangle((int)monster.PixelPosition.X, (int)monster.PixelPosition.Y, 16, 16, Raylib.Fade(Color.RED, 0.5f));

                    if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
                    {
                        _selectedMonster = monster;
                    }
                }
            }

            _player.Draw();

            Raylib.EndMode2D();

            if (_selectedMonster != null)
            {
                Raylib.DrawRectangle(GameWidth - 200, 0, 200, GameHeight, Raylib.Fade(Color.BLACK, 0.5f));

                Raylib.DrawText($"Name: {_selectedMonster._data.Name}", GameWidth - 190, 10, 10, Color.WHITE);
                Raylib.DrawTextureRec(AssetManager.Instance.GetTexture(_selectedMonster._data.FullImage), _selectedMonster._data.FullFrame, new Vector2(GameWidth - 190, 30), Color.WHITE);
            }

            Raylib.DrawText("WASD to move", 10, 10, 10, Color.WHITE);
        }
    }
}
