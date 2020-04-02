namespace SparkEngine.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;

    public struct ArrayGrid<T> : IComponent where T : struct, IComponent
    {
        public ArrayGrid(Perspective perspective, T[,] grid, Vector2 tileSize, bool wrapAround = false, bool isScreenGrid = false)
        {
            Position = Vector2.Zero;

            Cells = grid;

            Width = grid.GetLength(0);
            Height = grid.GetLength(1);
            TileSize = tileSize;

            DrawLayer = 0; //temp

            Perspective = perspective;
            WrapAround = wrapAround;
            IsScreenGrid = isScreenGrid;
        }

        public Vector2 Position { get; set; }

        public int Width { get; }

        public int Height { get; }

        public Vector2 TileSize { get; }

        public int DrawLayer { get; }

        public Perspective Perspective { get; }

        public bool WrapAround { get; }

        public bool IsScreenGrid { get; }

        public T[,] Cells { get; }

        public T this[int x, int y]
        {
            get => Cells[x, y];
            set => Cells[x, y] = value;
        }

        public bool IsPointWithinBounds(int x, int y)
        {
            return !(x < 0 || x >= Width || y < 0 || y >= Height);
        }

        public bool IsPointWithinBounds(Point coordinate)
        {
            return IsPointWithinBounds(coordinate.X, coordinate.Y);
        }
    }
}
