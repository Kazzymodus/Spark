namespace SparkEngine.DataStructures
{
    using Microsoft.Xna.Framework;

    public struct MapData
    {
        #region Constructors

        public MapData(Vector2 dimensions)
        {
            Dimensions = dimensions;
        }

        #endregion

        #region Properties

        public Vector2 Dimensions { get; }

        #endregion
    }
}
