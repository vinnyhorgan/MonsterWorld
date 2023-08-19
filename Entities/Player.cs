using System.Numerics;
using Raylib_cs;

namespace MonsterWorld.Entities
{
    class Player
    {
        private Texture2D _texture;
        private Rectangle _frame = new(0, 80, 16, 16);

        public Vector2 Position = Vector2.Zero;

        public Player()
        {
            _texture = AssetManager.Instance.GetTexture("characters.png");
        }

        public void Update(float dt, short[,] world)
        {
            if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
            {
                if (world[TileY() - 1, TileX()] == 1)
                {
                    Position.Y -= 16;
                }
            }
            else if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
            {
                if (world[TileY() + 1, TileX()] == 1)
                {
                    Position.Y += 16;
                }
            }
            else if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
            {
                if (world[TileY(), TileX() - 1] == 1)
                {
                    Position.X -= 16;
                }
            }
            else if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
            {
                if (world[TileY(), TileX() + 1] == 1)
                {
                    Position.X += 16;
                }
            }
        }

        public void Draw()
        {
            Raylib.DrawTextureRec(_texture, _frame, Position, Color.WHITE);
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
