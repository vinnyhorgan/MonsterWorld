using Raylib_cs;

namespace MonsterWorld
{
    public enum TileType
    {
        Grass,
        Water,
        Rock,
        Flower,
        Bush
    }

    class Tile
    {
        public TileType Type { get; private set; }
        public bool IsWalkable { get; private set; }
        public Texture2D Texture { get; private set; }
        public Rectangle Frame { get; private set; }

        public Tile(TileType type)
        {
            Type = type;

            switch (type)
            {
                case TileType.Grass:
                    IsWalkable = true;
                    Texture = AssetManager.Instance.GetTexture("tiles.png");
                    Frame = new Rectangle(16 * 6, 16, 16, 16);
                    break;
                case TileType.Water:
                    IsWalkable = true;
                    Texture = AssetManager.Instance.GetTexture("tiles.png");
                    Frame = new Rectangle(16 * 6, 16 * 2, 16, 16);
                    break;
                case TileType.Rock:
                    IsWalkable = false;
                    Texture = AssetManager.Instance.GetTexture("tiles.png");
                    Frame = new Rectangle(0, 16, 16, 16);
                    break;
                case TileType.Flower:
                    IsWalkable = true;
                    Texture = AssetManager.Instance.GetTexture("tiles.png");
                    Frame = new Rectangle(0, 0, 16, 16);
                    break;
                case TileType.Bush:
                    IsWalkable = false;
                    Texture = AssetManager.Instance.GetTexture("tiles.png");
                    Frame = new Rectangle(16 * 2, 16 * 3, 16, 16);
                    break;
            }
        }
    }
}
