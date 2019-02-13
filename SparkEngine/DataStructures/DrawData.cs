namespace SparkEngine.DataStructures
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public struct DrawData
    {
        #region Constructors

        public DrawData(Texture2D texture, Vector2 dimensions)
        {
            Texture = texture;
            Dimensions = dimensions;
        }

        #endregion

        #region Properties

        public Texture2D Texture { get; }

        public Vector2 Dimensions { get; }

        #endregion
    }
}
