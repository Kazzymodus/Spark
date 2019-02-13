namespace SparkEngine.Dictionaries
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.IDs;

    public static class FontDictionary
    {
        #region Fields

        private const string FontPath = "Fonts/";
        private static IDictionary<int, SpriteFont> fonts;

        #endregion

        #region Methods

        public static SpriteFont GetFont(int key)
        {
            return fonts[key];
        }

        internal static void LoadFonts(ContentManager content)
        {
            fonts = new Dictionary<int, SpriteFont>()
            {

            };
        }

        #endregion
    }
}
