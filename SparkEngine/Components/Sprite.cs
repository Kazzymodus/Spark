namespace SparkEngine.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    
    public class Sprite : Drawable
    {
        #region Constructors

        private Sprite(Texture2D texture, Vector2 frameSize, Vector2 anchor, Color color = default(Color))
        {
            Texture = texture;
            FrameSize = frameSize;
            Anchor = anchor;
            ColorMask = color;
        }

        #endregion

        #region Properties

        public Texture2D Texture { get; }

        public Vector2 FrameSize { get; }

        public Vector2 Anchor { get; }

        public Color ColorMask { get; }

        #endregion

        #region Methods

        public static Sprite CreateIsometricSprite(Texture2D texture, Vector2 tileSize, Vector2 dimensions, int rotations = 1, int animationLength = 1)
        {
            if (rotations <= 0 || animationLength <= 0)
            {
                throw new ArgumentException("rotations and animationLength must be larger than 0.");
            }

            int frameX = texture.Width / rotations;
            int frameY = texture.Height / animationLength;
            Vector2 frameSize = new Vector2(frameX, frameY);
            Vector2 anchor = GetIsometricSpriteAnchor(frameSize, dimensions, tileSize);

            return new Sprite(texture, frameSize, anchor);
        }

        public static Sprite CreateTileSprite()
        {
            throw new NotImplementedException();
        }

        private static Vector2 GetSquareSpriteAnchor(Vector2 frameSize, Vector2 dimensions, Vector2 unit)
        {
            float anchorY = frameSize.Y - (dimensions.Y * unit.Y);
            return new Vector2(0, anchorY);
        }

        private static Vector2 GetIsometricSpriteAnchor(Vector2 frameSize, Vector2 dimensions, Vector2 unit)
        {
            float anchorX = unit.X * (dimensions.X * 0.5f - 0.5f);
            float anchorY = frameSize.Y - unit.Y;

            return new Vector2(anchorX, anchorY);
        }

        #endregion
    }
}
