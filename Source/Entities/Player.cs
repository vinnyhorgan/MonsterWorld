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

        public void Update(float dt, Map map)
        {
            if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
            {
                if (map.GetTile(TileX, TileY - 1).IsWalkable)
                {
                    Position.Y -= 16;
                }
            }
            else if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
            {
                if (map.GetTile(TileX, TileY + 1).IsWalkable)
                {
                    Position.Y += 16;
                }
            }
            else if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
            {
                if (map.GetTile(TileX - 1, TileY).IsWalkable)
                {
                    Position.X -= 16;
                }
            }
            else if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
            {
                if (map.GetTile(TileX + 1, TileY).IsWalkable)
                {
                    Position.X += 16;
                }
            }
        }

        public void Draw()
        {
            Raylib.DrawTextureRec(_texture, _frame, Position, Color.WHITE);
        }
    }
}
