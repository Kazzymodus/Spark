namespace SparkEngine.Debug
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public struct LogMessage
    {
        #region Fields

        private string message;
        private Vector2 drawPosition;
        private Color colour;

        #endregion

        #region Constructors

        public LogMessage(string message, Vector2 drawPosition, Color colour)
        {
            this.message = message;
            this.drawPosition = drawPosition;
            this.colour = colour;
        }

        #endregion

        #region Methods

        internal void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.DrawString(font, message, drawPosition, colour);
        }

        #endregion
    }
}
