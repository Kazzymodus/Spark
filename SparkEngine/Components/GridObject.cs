namespace SparkEngine.Components
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Rendering;
    using SparkEngine.States;
    using SparkEngine.UI;

    /// <summary>
    /// A GridObject is an object that can occupy a coordinate on a grid, like a tile based terrain.
    /// </summary>
    public class GridObject : Component, IDrawableComponent
    {
        #region Constructors

        public GridObject(Texture2D spriteSheet, Vector2 coordinates, Vector2 dimensions, int rotation = Projector.RotationNone)
        {
            SpriteData = SpriteData.CreateIsometricSprite(spriteSheet, new Vector2(64, 32), dimensions, 4, 1);

            Coordinates = coordinates;
            Dimensions = dimensions;
            Rotation = rotation;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The position of the object in coordinates, where 0.0 is the upper left corner.
        /// </summary>
        public Vector2 Coordinates { get; private set; }

        /// <summary>
        /// The offset in tiles a tile is drawn from its root coordinates.
        /// Offset ranges from -0.5 to 0.5 (half a tile), at which point the coordinate rolls over.
        /// </summary>
        public Vector2 TileOffset { get; private set; }

        /// <summary>
        /// The (clockwise) rotations of the object. Each rotation signifies 90 degrees.
        /// </summary>
        public int Rotation { get; private set; }

        /// <summary>
        /// The dimensions in tiles of the object.
        /// </summary>
        public Vector2 Dimensions { get; }

        /// <summary>
        /// The sprite data used for rendering the object.
        /// </summary>
        public SpriteData SpriteData { get; }

        public LayerSortMethod LayerSortMethod { get; private set; } = LayerSortMethod.HeightAsDistance;

        #endregion

        #region Methods

        /// <summary>
        /// Returns the draw position.
        /// </summary>
        /// <param name="camera">The camera this component will be rendered to.</param>
        /// <param name="tileSize">The dimensions (in pixels) of a single tile.</param>
        public Vector2 GetDrawPosition(Camera camera, Vector2 tileSize)
        {
            int rotations = camera.Rotations;
            Vector2 cornerPixels = Projector.CarthesianToPixels(GetRootCoordinates(rotations), tileSize);
            Vector2 drawPosition = (cornerPixels - SpriteData.Anchor) + TileOffset;

            //// Correction, as we need to draw in the X center of the tile, not the origin.

            //drawPosition.X += Projector.DefaultTileWidth / 2;
            //if (Dimensions == Projector.Dimension1X1)
            //{
            //    drawPosition.Y += Projector.DefaultTileHeight / 2;
            //}

            return drawPosition;
        }

        /// <summary>
        /// Sets the position of the WorldObject to the given coordinates. Discards any offset.
        /// </summary>
        /// <param name="coordinates"></param>
        public void SetPosition(Vector2 coordinates)
        {
            SetPosition(coordinates, Vector2.Zero);
        }

        /// <summary>
        /// Sets the position of the WorldObject to the given coordinates and tile offset.
        /// </summary>
        /// <param name="coordinates"></param>
        /// <param name="offset"></param>
        public void SetPosition(Vector2 coordinates, Vector2 offset)
        {
            float coordX = (float)Math.Truncate(coordinates.X);
            float coordY = (float)Math.Truncate(coordinates.Y);
            Coordinates = new Vector2(coordX, coordY);
            TileOffset = offset;
            ProcessOffset();
        }

        /// <summary>
        /// Translate the object.
        /// </summary>
        /// <param name="amount">The amount of tiles to translate.</param>
        public void Translate(Vector2 amount)
        {
            float x = amount.X;
            float y = amount.Y;

            Vector2 offset = TileOffset;

            if (y != 0 && x != 0)
            {
                float clamp = Projector.SqrtTwoReciprocal;
                x *= clamp;
                y *= clamp;
            }

            if ((offset.X < 0 && offset.X + x > 0) || (offset.X > 0 && offset.X + x < 0))
            {
                offset.X = 0;
            }
            else
            {
                offset.X += x;
            }

            if ((offset.Y < 0 && offset.Y + y > 0) || (offset.Y > 0 && offset.Y + y < 0))
            {
                offset.Y = 0;
            }
            else
            {
                offset.Y += y;
            }

            TileOffset = offset;

            ProcessOffset();
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera, Vector2 unit)
        {
            Vector2 frameSize = SpriteData.FrameSize;
            int frameX = (int)frameSize.X * (Rotation + camera.Rotations) % 4;
            int frameY = 0; // For now;

            Rectangle frame = new Rectangle(frameX, frameY, (int)frameSize.X, (int)frameSize.Y);

            spriteBatch.Draw(SpriteData.Texture, GetDrawPosition(camera, unit), frame, Color.White);
        }

        /// <summary>
        /// Processes excessive offset. Should be called every time offset is modified.
        /// </summary>
        private void ProcessOffset()
        {
            Vector2 offset = TileOffset;
            Vector2 coordinates = Coordinates;

            while (offset.X >= 0.5f)
            {
                offset.X--;

                coordinates.X++;
            }

            while (offset.X <= -0.5f)
            {
                offset.X++;

                coordinates.X--;
            }

            while (offset.Y >= 0.5f)
            {
                offset.Y--;

                coordinates.Y++;
            }

            while (offset.Y <= -0.5f)
            {
                offset.Y++;

                coordinates.Y--;
            }

            TileOffset = offset;
            Coordinates = coordinates;
        }

        /// <summary>
        /// Gets the root coordinates. I'll explain this better later. Doesn't do anything at this stage anyway.
        /// </summary>
        /// <param name="worldRotations"></param>
        /// <returns></returns>
        private Vector2 GetRootCoordinates(int worldRotations)
        {
            return Coordinates;

            //Vector2 rotatedCoords = Projector.RotateCoordsInMap(Coordinates + TileOffset, worldRotations);

            //if (Dimensions != Projector.Dimension1X1)
            //{
            //    rotatedCoords.X += worldRotations == 1 || worldRotations == 2 ? Dimensions.X - 1 : 0;
            //    rotatedCoords.Y += worldRotations / 2 * (Dimensions.Y - 1);
            //}

            //return rotatedCoords;
        }

        #endregion
    }
}
