using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SparkEngine.Debug
{
    public struct LogMessage
    {
        #region Fields

        private readonly string message;
        private readonly Vector2 drawPosition;
        private readonly Color colour;

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