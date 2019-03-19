namespace SparkEngine.Components
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Rendering;

    interface IDrawableComponent
    {
        SpriteSortMethod SpriteSortMethod { get; }

        Vector2 DrawPosition { get; }

        void Draw(SpriteBatch spriteBatch, Camera camera);

        /// <summary>
        /// Calculates the draw position.
        /// Should be called whenever anything affecting the draw position is changed (position, camera rotation).
        /// </summary>
        /// <param name="camera">The camera this component will be rendered to.</param>
        void CalculateDrawPosition(Camera camera);
    }
}
