using System.Collections.Generic;
using System.Numerics;
using AStar;
using AStar.Options;
using Raylib_cs;

namespace MonsterWorld
{
    struct Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(Position a, Position b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Position a, Position b)
        {
            return !(a == b);
        }

        public static Position operator +(Position a, Position b)
        {
            return new Position(a.X + b.X, a.Y + b.Y);
        }

        public static Position operator -(Position a, Position b)
        {
            return new Position(a.X - b.X, a.Y - b.Y);
        }

        public static Position operator *(Position a, int b)
        {
            return new Position(a.X * b, a.Y * b);
        }

        public static Position operator /(Position a, int b)
        {
            return new Position(a.X / b, a.Y / b);
        }

        public override bool Equals(object obj)
        {
            return obj is Position position && this == position;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    class Map
    {
        private Tile[,] _tiles;
        private PathFinder _finder;

        public Map()
        {
            _tiles = new Tile[,] {
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Rock), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Water), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Bush), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Rock), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Rock), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Water), new Tile(TileType.Rock), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Rock), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Water), new Tile(TileType.Rock), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Rock), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Rock), new Tile(TileType.Grass), new Tile(TileType.Bush), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Bush), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Rock), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Rock), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Rock), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Bush), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
                { new Tile(TileType.Grass), new Tile(TileType.Water), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass), new Tile(TileType.Grass) },
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

        public Position[] GetPositionsInRange(int x, int y, int range)
        {
            var positions = new List<Position>();

            for (int row = y - range; row <= y + range; row++)
            {
                for (int col = x - range; col <= x + range; col++)
                {
                    if (col >= 0 && col < Width && row >= 0 && row < Height)
                    {
                        positions.Add(new Position(col, row));
                    }
                }
            }

            return positions.ToArray();
        }

        public Position GetPositionOfTileInRange(int x, int y, int range, TileType tile)
        {
            var positions = GetPositionsInRange(x, y, range);

            foreach (var position in positions)
            {
                if (GetTile(position.X, position.Y).Type == tile)
                {
                    return position;
                }
            }

            return new Position(-1, -1);
        }

        public Position[] FindPath(int x1, int y1, int x2, int y2)
        {
            var path = _finder.FindPath(new System.Drawing.Point(x1, y1), new System.Drawing.Point(x2, y2));

            var positions = new List<Position>();

            foreach (var point in path)
            {
                positions.Add(new Position(point.X, point.Y));
            }

            return positions.ToArray();
        }

        public void Draw()
        {
            for (int row = 0; row < _tiles.GetLength(0); row++)
            {
                for (int col = 0; col < _tiles.GetLength(1); col++)
                {
                    Raylib.DrawTextureRec(_tiles[row, col].Texture, new Rectangle(16 * 6, 16, 16, 16), new Vector2(col * 16, row * 16), Color.WHITE);

                    if (_tiles[row, col].Type != TileType.Grass)
                    {
                        Raylib.DrawTextureRec(_tiles[row, col].Texture, _tiles[row, col].Frame, new Vector2(col * 16, row * 16), Color.WHITE);
                    }
                }
            }
        }
    }
}
