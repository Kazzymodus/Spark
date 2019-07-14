namespace SparkEngine.Debug
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Rendering;

    public class Log
    {
        #region Fields

        private const int ListMessageDistance = 10;

        private static SpriteFont messageFont;

        private readonly List<LogMessage> messages = new List<LogMessage>();
        private int listMessages = 0;

        #endregion

        #region Properties

        public Color LogColour { get; set; } = Color.White;

        #endregion

        #region Methods

        public static void SetMessageFont(SpriteFont font)
        {
            messageFont = font;
        }

        public void AddWorldMessage(string text, Vector2 worldPosition, Camera camera)
        {
            AddWorldMessage(text, worldPosition, camera, LogColour);
        }

        public void AddWorldMessage(string text, Vector2 worldPosition, Camera camera, Color colour)
        {
            Vector3 inverseTranslation = -Matrix.Invert(camera.Transform).Translation;
            Vector2 drawPosition = worldPosition + new Vector2(inverseTranslation.X, inverseTranslation.Y);
            AddMessage(text, drawPosition, colour);
        }

        public void AddScreenMessage(string text, Vector2 position)
        {
            AddScreenMessage(text, position, LogColour);
        }

        public void AddScreenMessage(string text, Vector2 position, Color colour)
        {
            AddMessage(text, position, colour);
        }

        public void AddListMessage(string text)
        {
            AddListMessage(text, LogColour);
        }

        public void AddListMessage(string text, Color colour)
        {
            Vector2 drawPosition = new Vector2(0, listMessages++ * ListMessageDistance);
            AddMessage(text, drawPosition, colour);
        }
        
        private void AddMessage(string text, Vector2 position, Color colour)
        {
            LogMessage message = new LogMessage(text, position, colour);
            messages.Add(message);
        }

        internal void DrawMessages(SpriteBatch spriteBatch)
        {
            foreach (LogMessage message in messages)
            {
                message.Draw(spriteBatch, messageFont);
            }

            messages.Clear();
            listMessages = 0;
        }

        #endregion
    }
}
