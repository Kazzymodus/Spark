namespace SparkEngine.Dictionaries
{
    using System.Collections.Generic;
    using SparkEngine.DataStructures;
    using SparkEngine.IDs;

    public static class TileDictionary
    {
        #region Fields

        private static IDictionary<int, TileData> tiles;

        #endregion

        #region Public Methods

        public static TileData GetTile(int key)
        {
            return tiles[key];
        }

        internal static void LoadTiles()
        {
            tiles = new Dictionary<int, TileData>
            {

            };
        }

        #endregion
    }
}
