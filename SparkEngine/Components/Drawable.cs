namespace SparkEngine.Components
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Rendering;

    public struct Drawable : IComponent
    {
        public Vector2 DrawOffset { get; set; }

        public int DrawLayer { get; }
    }
}
