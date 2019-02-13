namespace SparkEngine.Dictionaries
{
    using System.Collections.Generic;
    using SparkEngine.DataStructures;
    using SparkEngine.IDs;
    using SparkEngine.Rendering;

    public static class EntityDictionary
    {
        #region Fields

        private static IDictionary<int, DrawData> entities;

        #endregion

        #region Methods

        public static DrawData GetEntity(int key)
        {
            return entities[key];
        }

        internal static void LoadEntities()
        {
            entities = new Dictionary<int, DrawData>
            {
                // {EntityID.Subject, new DrawData(TextureManager.GetEntityTexture(EntityID.Subject), RenderHelper.Dimension1X1)}
                { EntityIDs.Subject, new DrawData(TextureDictionary.GetTileTexture(TileIDs.CursorTile), RenderHelper.Dimension1X1) }
            };
        }

        #endregion
    }
}
