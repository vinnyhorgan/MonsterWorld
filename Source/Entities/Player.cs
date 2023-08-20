using System.Numerics;
using Raylib_cs;

namespace MonsterWorld.Entities
{
    class Player
    {
        private Texture2D _texture;
        private Rectangle _frame = new(0, 80, 16, 16);
        private float _timer = 0;

        public Position Position = new(0, 0);

        public Player()
        {
            _texture = AssetManager.Instance.GetTexture("characters.png");
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

            if (_timer > 0.1)
            {
                _timer = 0;

                if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
                {
                    if (map.GetTile(Position.X, Position.Y - 1).IsWalkable)
                    {
                        Position.Y--;
                    }
                }
                else if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
                {
                    if (map.GetTile(Position.X, Position.Y + 1).IsWalkable)
                    {
                        Position.Y++;
                    }
                }
                else if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
                {
                    if (map.GetTile(Position.X - 1, Position.Y).IsWalkable)
                    {
                        Position.X--;
                    }
                }
                else if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
                {
                    if (map.GetTile(Position.X + 1, Position.Y).IsWalkable)
                    {
                        Position.X++;
                    }
                }
            }
        }

        public void Draw()
        {
            Raylib.DrawTextureRec(_texture, _frame, PixelPosition, Color.WHITE);
        }
    }
}
