namespace SparkEngine.Components
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Rendering;

    interface IDrawableComponent
    {
        LayerSortMethod LayerSortMethod { get; }

        void Draw(SpriteBatch spriteBatch, Camera camera, Vector2 unit);

        /// <summary>
        /// Returns the draw position.
        /// </summary>
        /// <param name="camera">The camera this component will be rendered to.</param>
        Vector2 GetDrawPosition(Camera camera, Vector2 unit);
    }
}
