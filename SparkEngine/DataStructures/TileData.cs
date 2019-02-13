namespace SparkEngine.DataStructures
{
    using Microsoft.Xna.Framework.Graphics;

    public struct TileData
    {
        #region Constructors

        public TileData(Texture2D texture)
        {
            Texture = texture;
        }

        #endregion

        #region Properties

        public Texture2D Texture { get; }

        #endregion
    }
}
