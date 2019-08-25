namespace SparkEngine.Components
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Rendering;

    public abstract class Drawable : Component
    {
        public Vector2 DrawPosition { get; }

        //public LayerSortMethod LayerSortMethod { get; }

        public int DrawLayer { get; }
    }
}
