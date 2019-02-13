namespace SparkEngine.UI.Buttons
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Dictionaries;
    using SparkEngine.IDs;
    using SparkEngine.Input;

    public abstract class Button : UIElement, IClickable, IHoverable
    {
        #region Fields

        //Texture2D backdrop;
        private Vector2 size;
        private Tooltip tooltip;

        private bool drawTooltip;

        #endregion

        #region Constructors

        public Button(Vector2 position, string tooltipText, UIPanel parent = null)
            : base(position, parent)
        {
            // TEMP
            size = TextureDictionary.GetUITexture(UserInterfaceIDs.Button).Bounds.Size.ToVector2(); // TEMP
            UIManager.RegisterClickable(this);
            tooltip = new Tooltip(tooltipText);
        }

        #endregion

        #region Properties

        public Rectangle Bounds
        {
            get { return new Rectangle(ParentPosition.ToPoint() + Position.ToPoint(), size.ToPoint()); }
        }

        #endregion

        #region Methods

        public abstract void ExecuteClick();

        public void ExecuteHover()
        {
            drawTooltip = true;
        }

        public void ClearHover()
        {
            drawTooltip = false;
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            // TEMP
            spriteBatch.Draw(TextureDictionary.GetUITexture(UserInterfaceIDs.Button), ParentPosition + Position, Color.White);

            if (drawTooltip)
            {
                tooltip.Draw(spriteBatch);
            }
        }

        #endregion
    }
}
