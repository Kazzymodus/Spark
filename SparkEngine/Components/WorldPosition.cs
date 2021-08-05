using System;
using Microsoft.Xna.Framework;

namespace SparkEngine.Components
{
    [Obsolete]
    public struct WorldPosition : IComponent
    {
        public float WorldX { get; set; }
        public float WorldY { get; set; }

        public WorldPosition(float x, float y)
        {
            WorldX = x;
            WorldY = y;
        }

        public static implicit operator WorldPosition(Vector2 pos)
        {
            return new WorldPosition(pos.X, pos.Y);
        }

        public static implicit operator Vector2(WorldPosition pos)
        {
            return new Vector2(pos.WorldX, pos.WorldY);
        }

        public static Vector2 operator -(WorldPosition pos)
        {
            return new Vector2(-pos.WorldX, -pos.WorldY);
        }
    }
}