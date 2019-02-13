namespace SparkEngine.Debug
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class DebugLog
    {
        #region Fields

        private const int ListMessageDistance = 10;

        private List<DebugMessage> worldMessages = new List<DebugMessage>();
        private List<DebugMessage> screenMessages = new List<DebugMessage>();
        private List<DebugMessage> listMessages = new List<DebugMessage>();

        #endregion

        #region Properties

        public static Vector2 TileMessageOffset
        {
            get { return new Vector2(8); }
        }

        #endregion

        #region Methods

        public void AddWorldMessage(string text, Vector2 position)
        {
            AddWorldMessage(text, position, Color.White);
        }

        public void AddWorldMessage(string text, Vector2 position, Color colour)
        {
            AddMessage(text, position, colour, worldMessages);
        }

        public void AddScreenMessage(string text, Vector2 position)
        {
            AddScreenMessage(text, position, Color.White);
        }

        public void AddScreenMessage(string text, Vector2 position, Color colour)
        {
            AddMessage(text, position, colour, screenMessages);
        }

        public void AddListMessage(string text)
        {
            AddListMessage(text, Color.White);
        }

        public void AddListMessage(string text, Color colour)
        {
            Vector2 drawPosition = new Vector2(0, listMessages.Count * ListMessageDistance);
            AddMessage(text, drawPosition, colour, listMessages);
        }

        internal void DrawWorldMessages(SpriteBatch spriteBatch)
        {
            DrawMessages(spriteBatch, worldMessages);
        }

        internal void DrawScreenMessages(SpriteBatch spriteBatch)
        {
            DrawMessages(spriteBatch, screenMessages);
        }

        internal void DrawListMessages(SpriteBatch spriteBatch)
        {
            DrawMessages(spriteBatch, listMessages);
        }

        private void AddMessage(string text, Vector2 position, Color colour, List<DebugMessage> messages)
        {
            DebugMessage message = new DebugMessage(text, position, colour);
            messages.Add(message);
        }

        private void DrawMessages(SpriteBatch spriteBatch, List<DebugMessage> messages)
        {
            foreach (DebugMessage message in messages)
            {
                message.Draw(spriteBatch);
            }

            messages.Clear();
        }

        #endregion
    }
}
