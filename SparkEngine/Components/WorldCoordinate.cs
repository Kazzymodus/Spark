using System;
using Microsoft.Xna.Framework;

namespace SparkEngine.Components
{
    [Obsolete]
    public struct WorldCoordinate : IComponent
    {
        public int CoordX { get; set; }
        public int CoordY { get; set; }

        public static implicit operator Point(WorldCoordinate coords)
        {
            return new Point(coords.CoordX, coords.CoordY);
        }
    }
}