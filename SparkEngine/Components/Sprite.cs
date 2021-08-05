using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SparkEngine.Components
{
    public struct Sprite : IComponent
    {
        #region Constructors

        private Sprite(Texture2D texture, Vector2 frameSize, Vector2 anchor, Color? color = null)
        {
            Texture = texture;
            FrameSize = frameSize;
            Anchor = anchor;
            ColorMask = color ?? Color.White;

            DrawPosition = default;
            DrawLayer = 0;

            FrameX = 0;
            FrameY = 0;

            IsAnimated = frameSize.ToPoint() != texture.Bounds.Size;
        }

        #endregion

        #region Properties

        public Vector2 DrawPosition { get; set; }

        public int DrawLayer { get; set; }

        public Texture2D Texture { get; }

        public Vector2 FrameSize { get; }

        public Vector2 Anchor { get; }

        public Color ColorMask { get; }

        public bool IsAnimated { get; }

        public int FrameX { get; set; }

        public int FrameY { get; set; }

        #endregion

        #region Methods

        public void SetFrames(int x, int y)
        {
            FrameX = x;
            FrameY = y;
        }

        public static Sprite CreateIsometricSprite(Texture2D texture, Vector2 tileSize, Vector2 dimensions,
            int rotations = 1, int animationLength = 1)
        {
            if (rotations <= 0 || animationLength <= 0)
                throw new ArgumentException("rotations and animationLength must be larger than 0.");

            var frameX = texture.Width / rotations;
            var frameY = texture.Height / animationLength;
            var frameSize = new Vector2(frameX, frameY);
            var anchor = GetIsometricSpriteAnchor(frameSize, dimensions, tileSize);

            return new Sprite(texture, frameSize, anchor);
        }

        public static Sprite CreateClone(Sprite sprite)
        {
            return CreateTileSprite(sprite.Texture, sprite.FrameSize, sprite.ColorMask);
        }

        public static Sprite CreateTileSprite(Texture2D texture, int horizontalFrames = 1, int verticalFrames = 1,
            Color? color = null)
        {
            if (horizontalFrames <= 0 || verticalFrames <= 0)
                throw new ArgumentOutOfRangeException(
                    "The amount of horizontal and vertical frames must be larger than 0");

            var frameSize = new Vector2(texture.Width / horizontalFrames, texture.Height / verticalFrames);

            return CreateTileSprite(texture, frameSize, color);
        }

        public static Sprite CreateTileSprite(Texture2D texture, Vector2 frameSize, Color? color = null)
        {
            return new Sprite(texture, frameSize, Vector2.Zero, color);
        }

        private static Vector2 GetSquareSpriteAnchor(Vector2 frameSize, Vector2 dimensions, Vector2 unit)
        {
            var anchorY = frameSize.Y - dimensions.Y * unit.Y;
            return new Vector2(0, anchorY);
        }

        private static Vector2 GetIsometricSpriteAnchor(Vector2 frameSize, Vector2 dimensions, Vector2 unit)
        {
            var anchorX = unit.X * (dimensions.X * 0.5f - 0.5f);
            var anchorY = frameSize.Y - unit.Y;

            return new Vector2(anchorX, anchorY);
        }

        #endregion
    }
}