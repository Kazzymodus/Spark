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

            };
        }

        #endregion
    }
}
