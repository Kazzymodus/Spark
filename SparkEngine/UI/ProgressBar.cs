namespace SparkEngine.UI
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Dictionaries;
    using SparkEngine.IDs;
    using SparkEngine.Rendering;
    using SparkEngine.States;
    using SparkEngine.World;

    public class ProgressBar : UIElement
    {
        #region Fields

        private Vector2 scale = Vector2.One;

        private Texture2D backdrop = TextureDictionary.GetUITexture(UserInterfaceIDs.ProgressBar);
        private Texture2D fill = TextureDictionary.GetUITexture(UserInterfaceIDs.ProgressBar);

        private Color colour;

        private WorldObject parentObject;

        #endregion

        #region Constructors

        public ProgressBar(WorldObject parentObject, Color colour, UIPanel parent = null)
            : base(Vector2.Zero, parent)
        {
            this.parentObject = parentObject;
            this.colour = colour;
        }

        #endregion

        #region Properties

        public float Progress { private get; set; }

        #endregion

        #region Methods

        internal override void Draw(SpriteBatch spriteBatch)
        {
            //int worldRotations = StateManager.MainCamera.Rotations;
            spriteBatch.Draw(backdrop, parentObject.GetDrawPosition(0), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);

            Vector2 fillAmount = scale;
            fillAmount.X *= Progress;

            spriteBatch.Draw(fill, parentObject.GetDrawPosition(0), null, Color.Red, 0, Vector2.Zero, fillAmount, SpriteEffects.None, 0);
        }

        #endregion
    }
}
