namespace SparkEngine.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;

    public class Unit : Component 
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
            return new Unit(unit.X, unit.Y);
        }

        public static implicit operator Vector2(Unit unit)
        {
            return new Vector2(unit.LengthX, unit.LengthY);
        }
    }
}
