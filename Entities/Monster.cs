using System.Numerics;
using AStar;
using Raylib_cs;

namespace MonsterWorld.Entities
{
    class Monster
    {
        private Texture2D _texture;
        private Rectangle _frame;
        private System.Drawing.Point[] _path = null;

        public Vector2 Position = Vector2.Zero;

        public Monster()
        {
            _texture = AssetManager.Instance.GetTexture("characters.png");

            _frame = new Rectangle(Raylib.GetRandomValue(0, 7) * 16, 0, 16, 16);
        }

        public void Update(float dt, short[,] world, PathFinder pathFinder, Player player)
        {
            _path = pathFinder.FindPath(new System.Drawing.Point((int)Position.X / 16, (int)Position.Y / 16), new System.Drawing.Point((int)player.Position.X / 16, (int)player.Position.Y / 16));

            if (_path != null && _path.Length > 0)
            {
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
            }
        }

        public void Draw()
        {
            Raylib.DrawTextureRec(_texture, _frame, Position, Color.WHITE);

            if (_path != null)
            {
                for (int i = 0; i < _path.Length; i++)
                {
                    Raylib.DrawRectangle(_path[i].X * 16, _path[i].Y * 16, 16, 16, Raylib.Fade(Color.RED, 0.5f));
                }
            }
        }
    }
}
