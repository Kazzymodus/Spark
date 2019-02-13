namespace SparkEngine.Dictionaries
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.IDs;

    public static class TextureDictionary
    {
        #region Fields

        private const string TileTexturePath = "Textures/Tiles/";
        private const string StructureTexturePath = "Textures/Structures/";
        private const string EntityTexturePath = "Textures/Entities/";
        private const string UITexturePath = "Textures/UI/";

        private const string PanelBackdropPath = UITexturePath + "PanelBackdrops/";

        private static IDictionary<int, Texture2D> tileTextures;        
        private static IDictionary<int, Texture2D> structureTextures;
        private static IDictionary<int, Texture2D> entityTextures;
        private static IDictionary<int, Texture2D> uiTextures;

        #endregion

        #region Methods

        public static Texture2D GetTileTexture(int key)
        {
            return tileTextures[key];
        }

        public static Texture2D GetStructureTexture(int key)
        {
            return structureTextures[key];
        }

        public static Texture2D GetEntityTexture(int key)
        {
            return entityTextures[key];
        }

        public static Texture2D GetUITexture(int key)
        {
            return uiTextures[key];
        }

        internal static void LoadTextures(ContentManager content)
        {
            LoadTileTextures(content);
            LoadStructureTextures(content);
            LoadEntityTextures(content);
            LoadUITextures(content);
        }

        private static void LoadTileTextures(ContentManager content)
        {
            tileTextures = new Dictionary<int, Texture2D>()
            {

            };
        }

        private static void LoadStructureTextures(ContentManager content)
        {
            structureTextures = new Dictionary<int, Texture2D>
            {

            };
        }

        private static void LoadEntityTextures(ContentManager content)
        {
            entityTextures = new Dictionary<int, Texture2D>
            {

            };
        }

        private static void LoadUITextures(ContentManager content)
        {
            uiTextures = new Dictionary<int, Texture2D>
            {

            };
        }

        #endregion
    }
}
