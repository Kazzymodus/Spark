using Microsoft.Xna.Framework;

namespace SparkEngine.Components
{
    public struct Drawable : IComponent
    {
        public Vector2 DrawOffset { get; set; }

        public int DrawLayer { get; }
    }
}