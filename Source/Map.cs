using System.Numerics;
using AStar;
using AStar.Options;
using Raylib_cs;

namespace MonsterWorld
{
    class Map
    {
        private Tile[,] _tiles;
        private PathFinder _finder;

        public Map()
        {
            _tiles = new Tile[,] {
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Water), new Tile(TileType.Water), new Tile(TileType.Water), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Water), new Tile(TileType.Water), new Tile(TileType.Water), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Water), new Tile(TileType.Water), new Tile(TileType.Water), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
            };

            var pathfinderOptions = new PathFinderOptions {
                HeuristicFormula = AStar.Heuristics.HeuristicFormula.Manhattan,
                UseDiagonals = false,
                PunishChangeDirection = true,
                SearchLimit = 2000
            };

            _finder = new PathFinder(new WorldGrid(Grid), pathfinderOptions);
        }

        public int Width
        {
            get
            {
                return _tiles.GetLength(1);
            }
        }

        public int Height
        {
            get
            {
                return _tiles.GetLength(0);
            }
        }

        public short[,] Grid
        {
            get
            {
                short[,] grid = new short[_tiles.GetLength(0), _tiles.GetLength(1)];

                for (int row = 0; row < _tiles.GetLength(0); row++)
                {
                    for (int col = 0; col < _tiles.GetLength(1); col++)
                    {
                        if (_tiles[row, col].IsWalkable)
                        {
                            grid[row, col] = 1;
                        }
                        else
                        {
                            grid[row, col] = 0;
                        }
                    }
                }

                return grid;
            }
        }

        public Tile GetTile(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                return new Tile(TileType.Rock);
            }

            return _tiles[y, x];
        }

        public Vector2[] FindPath(int x1, int y1, int x2, int y2)
        {
            var positionPath = _finder.FindPath(new Position(y1, x1), new Position(y2, x2));

            var path = new Vector2[positionPath.Length];

            for (int i = 0; i < positionPath.Length; i++)
            {
                path[i] = new Vector2(positionPath[i].Column, positionPath[i].Row);
            }

            return path;
        }

        public void Draw()
        {
            for (int row = 0; row < _tiles.GetLength(0); row++)
            {
                for (int col = 0; col < _tiles.GetLength(1); col++)
                {
                    Raylib.DrawTextureRec(_tiles[row, col].Texture, _tiles[row, col].Frame, new Vector2(col * 16, row * 16), Color.WHITE);
                }
            }
        }
    }
}
