namespace SparkEngine.Dictionaries
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.IDs;

    public static class EffectDictionary
    {
        #region Fields

        private const string EffectPath = "Effects/";
        private static IDictionary<int, Effect> effects;

        #endregion

        #region Methods

        public static Effect GetEffect(int key)
        {
            return effects[key];
        }

        internal static void LoadEffects(ContentManager content)
        {
            effects = new Dictionary<int, Effect>()
            {
                { EffectIDs.PixelShaders, content.Load<Effect>(EffectPath + "PixelShaders") },
            };
        }

        #endregion
    }
}
