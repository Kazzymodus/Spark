namespace SparkEngine.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Debug;
    using SparkEngine.Entities;

    public abstract class Component
    {
        #region Properties

        public int Owner { get; private set; }

        protected static Log Log { get; } = new Log();

        #endregion

        #region Method

        public virtual void ProcessInput(GameTime gameTime)
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public static void DrawMessages(SpriteBatch spriteBatch)
        {
            Log.DrawMessages(spriteBatch);
        }

        internal void SetOwner(Entity entity)
        {
            Owner = entity.ID;
        }

        #endregion
    }
}
