using System;
using Microsoft.Xna.Framework;

namespace SparkEngine.Components
{
    [Obsolete]
    public struct Unit : IComponent
    {
        public float LengthX { get; set; }
        public float LengthY { get; set; }

        public Unit(float x, float y)
        {
            LengthX = x;
            LengthY = y;
        }

        public static implicit operator Unit(Vector2 unit)
        {
            var (x, y) = unit;
            return new Unit(x, y);
        }

        public static implicit operator Vector2(Unit unit)
        {
            return new Vector2(unit.LengthX, unit.LengthY);
        }
    }
}