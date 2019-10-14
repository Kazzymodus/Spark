namespace SparkEngine.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    
    public class Sprite : Drawable, IEquatable<Sprite>
    {
        #region Constructors

        private Sprite(Texture2D texture, Vector2 frameSize, Vector2 anchor, Color? color = null)
        {
            Texture = texture;
            FrameSize = frameSize;
            Anchor = anchor;
            ColorMask = color ?? Color.White;
        }

        #endregion

        #region Properties

        public Texture2D Texture { get; }

        public Vector2 FrameSize { get; }

        public Vector2 Anchor { get; }

        public Color ColorMask { get; }

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            return Equals(obj as Sprite);
        }

        public bool Equals(Sprite other)
        {
            if (other == null)
            {
                return false;
            }

            if (this == other)
            {
                return true;
            }

            return Texture == other.Texture
                && FrameSize == other.FrameSize;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Texture?.GetHashCode() ?? 29;
                hash = hash * 23 + FrameSize.GetHashCode();
                return hash;
            }
        }

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

        public static Sprite CreateTileSprite(Texture2D texture, int horizontalFrames = 1, int verticalFrames = 1, Color? color = null)
        {
            if (horizontalFrames <= 0 || verticalFrames <= 0)
            {
                throw new ArgumentOutOfRangeException("The amount of horizontal and vertical frames must be larger than 0");
            }

            Vector2 frameSize = new Vector2(texture.Width / horizontalFrames, texture.Height / verticalFrames);

            return new Sprite(texture, frameSize, Vector2.Zero, color);
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
