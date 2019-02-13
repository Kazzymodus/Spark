namespace SparkEngine.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public abstract class UIElement
    {
        #region Constructors

        public UIElement(Vector2 position, UIPanel parent)
        {
            Position = position;
            ParentPosition = parent != null ? parent.Position : Vector2.Zero;
        }

        #endregion

        #region Properties

        public Vector2 Position { get; }

        protected internal Vector2 ParentPosition { get; }

        #endregion

        #region Methods

        internal abstract void Draw(SpriteBatch spriteBatch);

        #endregion
    }
}
