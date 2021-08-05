using Microsoft.Xna.Framework;

namespace SparkEngine.Components
{
    public class Grid : IComponent
    {
        public Grid(Perspective perspective, int width, int height, bool wrapAround)
        {
            Perspective = perspective;
            Width = width;
            Height = height;
            WrapAround = wrapAround;
        }

        public int Width { get; }

        public int Height { get; }

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