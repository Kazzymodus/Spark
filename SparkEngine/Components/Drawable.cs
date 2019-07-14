namespace SparkEngine.Components
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Rendering;

    public class Drawable : Component
    {
        public LayerSortMethod LayerSortMethod { get; }

        public int DrawLayer { get; }
    }
}
