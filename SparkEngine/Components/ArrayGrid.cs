namespace SparkEngine.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SparkEngine.Entities;
    using Microsoft.Xna.Framework;

    public struct ArrayGrid : IComponent
    {
        public ArrayGrid(Perspective perspective, ProtoEntity[,] grid, Vector2 tileSize, bool wrapAround, bool? isHomogenous = null)
        {
            Position = Vector2.Zero;

            Cells = grid;
            IsHomogenous = isHomogenous;

            Width = grid.GetLength(0);
            Height = grid.GetLength(1);
            TileSize = tileSize;

            DrawLayer = 0; //temp

            Perspective = perspective;
            WrapAround = wrapAround;
        }

        public Vector2 Position { get; set; }
        public int Width { get; }
        public int Height { get; }
        public Vector2 TileSize { get; }
        public int DrawLayer { get; }
        public Perspective Perspective { get; }
        public bool WrapAround { get; }

        public ProtoEntity this[int x, int y]
        {
            get => Cells[x, y];
        }

        public ProtoEntity[,] Cells { get; }

        public bool? IsHomogenous { get; private set; }

        public bool DetermineHomogenity(Type[] componentTypes)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (!Cells[x, y].Batch.ContainsAll(componentTypes))
                    {
                        return (bool)(IsHomogenous = false);
                    }
                }
            }

            return (bool)(IsHomogenous = true);
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
