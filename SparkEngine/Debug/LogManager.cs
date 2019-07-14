namespace SparkEngine.Debug
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Components;
    using SparkEngine.Rendering;

    public class LogManager
    {
        public void DrawMessages(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Component.DrawMessages(spriteBatch);
            Camera.DrawMessages(spriteBatch);

            spriteBatch.End();
        }
    }
}
