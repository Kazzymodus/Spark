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
                { TileIDs.GridTile, content.Load<Texture2D>(TileTexturePath + "GridTile") },
                { TileIDs.BlankTile, content.Load<Texture2D>(TileTexturePath + "BlankTile") },
                { TileIDs.GrassTile, content.Load<Texture2D>(TileTexturePath + "GrassTile") },
                { TileIDs.CursorTile, content.Load<Texture2D>(TileTexturePath + "CursorTile") }
            };
        }

        private static void LoadStructureTextures(ContentManager content)
        {
            structureTextures = new Dictionary<int, Texture2D>
            {
                { StructureIDs.Obelisk, content.Load<Texture2D>(StructureTexturePath + "Obelisk") },
                { StructureIDs.House, content.Load<Texture2D>(StructureTexturePath + "House") },
                { StructureIDs.Temple, content.Load<Texture2D>(StructureTexturePath + "House") }
            };
        }

        private static void LoadEntityTextures(ContentManager content)
        {
            entityTextures = new Dictionary<int, Texture2D>
            {
                { EntityIDs.Subject, content.Load<Texture2D>(EntityTexturePath + "Subject") }
            };
        }

        private static void LoadUITextures(ContentManager content)
        {
            uiTextures = new Dictionary<int, Texture2D>
            {
                { UserInterfaceIDs.ProgressBar, content.Load<Texture2D>(UITexturePath + "ProgressBar") },
                { UserInterfaceIDs.ResourceIcons, content.Load<Texture2D>(UITexturePath + "ResourceIcons") },
                { UserInterfaceIDs.Button, content.Load<Texture2D>(UITexturePath + "Button") },
                { UserInterfaceIDs.PanelBackdrops.TopBar, content.Load<Texture2D>(PanelBackdropPath + "TopBar") },
                { UserInterfaceIDs.PanelBackdrops.Tasks, content.Load<Texture2D>(PanelBackdropPath + "Tasks") },
                { UserInterfaceIDs.PanelBackdrops.BuildTask, content.Load<Texture2D>(PanelBackdropPath + "BuildTask") }
            };
        }

        #endregion
    }
}
