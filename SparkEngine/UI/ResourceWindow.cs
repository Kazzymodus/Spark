namespace SparkEngine.UI
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Dictionaries;
    using SparkEngine.IDs;
    using SparkEngine.Input;
    using SparkEngine.Player;

    public class ResourceWindow : UIElement, IHoverable
    {
        #region Fields

        public const int Width = 40;
        public const int Height = 20;

        public const int IconTextDistance = 8;

        private Tooltip tooltip;
        private bool showTooltip;
        private Resource resource;
        private Rectangle sourceRectangle;

        #endregion

        #region Constructors

        public ResourceWindow(Vector2 position, Resource resource, int id, UIPanel parent = null)
            : base(position, parent)
        {
            this.resource = resource;
            tooltip = new Tooltip(resource.Name);

            UIManager.RegisterHoverable(this);

            Point iconSize = TextureDictionary.GetUITexture(UserInterfaceIDs.ResourceIcons).Bounds.Size;

            int frameWidth = iconSize.X / ResourceIDs.Count;
            sourceRectangle = new Rectangle(frameWidth * id, 0, frameWidth, iconSize.Y);
        }

        #endregion

        public Rectangle Bounds
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, Width, Height); }
        }

        #region Methods

        public void ExecuteHover()
        {
            showTooltip = true;
        }

        public void ClearHover()
        {
            showTooltip = false;
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureDictionary.GetUITexture(UserInterfaceIDs.ResourceIcons), ParentPosition + Position, sourceRectangle, Color.White);

            Vector2 iconPosition = ParentPosition + Position;
            iconPosition.X += sourceRectangle.Width + IconTextDistance;

            string displayText = resource.Amount.ToString();

            spriteBatch.DrawString(FontDictionary.GetFont(FontIDs.CourierNew), displayText, iconPosition, Color.White);

            if (showTooltip)
            {
                tooltip.Draw(spriteBatch);
            }
        }

        #endregion
    }
}
