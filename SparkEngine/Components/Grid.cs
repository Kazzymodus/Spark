namespace SparkEngine.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;

    public class Grid : Drawable
    {
        public Grid(Perspective perspective)
        {
            Perspective = perspective;
        }

        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public Perspective Perspective { get; }
        public bool WrapAround { get; }

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
