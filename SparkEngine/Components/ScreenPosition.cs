using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SparkEngine.Components
{
    public class ScreenPosition : Component
    {
        public float ScreenX { get; set; }
        public float ScreenY { get; set; }

        public ScreenPosition()
            : this(0, 0)
        {

        }

        public ScreenPosition(float x, float y)
        {
            ScreenX = x;
            ScreenY = y;
        }

        public static implicit operator ScreenPosition(Vector2 pos)
        {
            return new ScreenPosition(pos.X, pos.Y);
        }

        public static implicit operator Vector2(ScreenPosition pos)
        {
            return new Vector2(pos.ScreenX, pos.ScreenY);
        }

        public static Vector2 operator -(ScreenPosition pos)
        {
            return new Vector2(-pos.ScreenX, -pos.ScreenY);
        }
    }
}
