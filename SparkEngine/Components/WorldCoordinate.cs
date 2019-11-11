using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SparkEngine.Components
{
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
