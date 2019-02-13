namespace SparkEngine.Debug
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Dictionaries;
    using SparkEngine.IDs;

    public struct DebugMessage
    {
        #region Fields

        private string text;
        private Vector2 drawPosition;
        private Color colour;

        #endregion

        #region Constructors

        public DebugMessage(string text, Vector2 drawPosition, Color colour)
        {
            this.text = text;
            this.drawPosition = drawPosition;
            this.colour = colour;
        }

        #endregion

        #region Methods

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(FontDictionary.GetFont(FontIDs.CourierNew), text, drawPosition, colour);
        }

        #endregion
    }
}
