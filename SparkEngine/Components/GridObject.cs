namespace SparkEngine.Components
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Rendering;

    /// <summary>
    /// A GridObject is an object that can occupy a coordinate on a grid, like a tile based terrain.
    /// </summary>
    public class GridObject : IComponent
    {
        #region Constructors

        private GridObject(Sprite spriteData, Vector2 coordinates, Vector2 dimensions, int rotation)
        {
            SpriteData = spriteData;
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
        public Sprite SpriteData { get; }

        public LayerSortMethod LayerSortMethod { get; private set; } = LayerSortMethod.HeightAsDistance;

        #endregion

        #region Methods

        public static GridObject CreateIsometricGridObject(Texture2D spriteSheet, DrawLayer drawLayer, Vector2 coordinates, Vector2 dimensions, int rotation = 0)
        {
            Sprite spriteData = Sprite.CreateIsometricSprite(spriteSheet, new Unit(0, 0), dimensions, 4, 1);

            GridObject gridObject = new GridObject(spriteData, coordinates, dimensions, rotation);
            return gridObject;
        }

        ///// <summary>
        ///// Returns the draw position.
        ///// </summary>
        ///// <param name="camera">The camera this component will be rendered to.</param>
        ///// <param name="tileSize">The dimensions (in pixels) of a single tile.</param>
        //public Vector2 GetDrawPosition(Camera camera, DrawLayer drawLayer)
        //{
        //    int rotations = camera.Rotations;
        //    Vector2 cornerPixels = Projector.CartesianToIsometricToPixels(GetRootCoordinates(rotations), drawLayer.Unit);
        //    Vector2 drawPosition = drawLayer.Position + (cornerPixels - SpriteData.Anchor);

        //    //// Correction, as we need to draw in the X center of the tile, not the origin.

        //    //drawPosition.X += Projector.DefaultTileWidth / 2;
        //    //if (Dimensions == Projector.Dimension1X1)
        //    //{
        //    //    drawPosition.Y += Projector.DefaultTileHeight / 2;
        //    //}

        //    return drawPosition;
        //}

        //public Rectangle GetBounds(Camera camera, DrawLayer drawLayer)
        //{
        //    Rectangle bounds = SpriteData.Texture.Bounds;
        //    bounds.Location = GetDrawPosition(camera, drawLayer).ToPoint();
        //    return bounds;
        //}

        ///// <summary>
        ///// Sets the position of the WorldObject to the given coordinates.
        ///// </summary>
        ///// <param name="coordinates"></param>
        ///// <param name="offset"></param>
        //public void SetPosition(Vector2 coordinates)
        //{
        //    Coordinates = coordinates;
        //}

        ///// <summary>
        ///// Translate the object.
        ///// </summary>
        ///// <param name="amount">The amount of units to translate.</param>
        //public void Translate(Vector2 amount)
        //{
        //    Coordinates += amount;
        //}

        //public void Draw(SpriteBatch spriteBatch, Camera camera, DrawLayer layer)
        //{
        //    Vector2 frameSize = SpriteData.FrameSize;
        //    int frameX = (int)frameSize.X * (Rotation + camera.Rotations) % 4;
        //    int frameY = 0; // For now;

        //    Rectangle frame = new Rectangle(frameX, frameY, (int)frameSize.X, (int)frameSize.Y);
        //    Vector2 drawPosition = GetDrawPosition(camera, layer);

        //    //Log.AddWorldMessage("DP: " + drawPosition.X + "," + drawPosition.Y, drawPosition, camera, Color.Red);
        //    //Log.AddWorldMessage("A: " + SpriteData.Anchor.X + "," + SpriteData.Anchor.Y, drawPosition + SpriteData.Anchor, camera, Color.Red);

        //    Log.AddWorldMessage(Coordinates.ToString(), drawPosition, camera, Color.Red);

        //    spriteBatch.Draw(SpriteData.Texture, drawPosition, frame, new Color(Color.White, 0.5f));
        //}

        ///// <summary>
        ///// Gets the root coordinates. I'll explain this better later. Doesn't do anything at this stage anyway.
        ///// </summary>
        ///// <param name="worldRotations"></param>
        ///// <returns></returns>
        //private Vector2 GetRootCoordinates(int worldRotations)
        //{
        //    return Coordinates;

        //    //Vector2 rotatedCoords = Projector.RotateCoordsInMap(Coordinates + TileOffset, worldRotations);

        //    //if (Dimensions != Projector.Dimension1X1)
        //    //{
        //    //    rotatedCoords.X += worldRotations == 1 || worldRotations == 2 ? Dimensions.X - 1 : 0;
        //    //    rotatedCoords.Y += worldRotations / 2 * (Dimensions.Y - 1);
        //    //}

        //    //return rotatedCoords;
        //}

        #endregion
    }
}
