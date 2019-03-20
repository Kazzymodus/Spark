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
        #region Fields

        /// <summary>
        /// Determines the rotation point of the object (relative to a single frame).
        /// </summary>
        private Vector2 pivot;

        #endregion

        #region Constructors

        public GridObject(Texture2D spriteSheet, Vector2 coordinates, Vector2 dimensions, int rotation = RenderHelper.RotationNone)
        {
            SpriteSheet = spriteSheet;
            Coordinates = coordinates;
            Dimensions = dimensions;
            Rotation = rotation;

            pivot.X = Dimensions.X * (RenderHelper.DefaultTileWidth * 0.5f);
            pivot.Y = SpriteSheet.Height - (Dimensions.X * RenderHelper.DefaultTileHeight * 0.5f);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The spritesheet used for rendering the object.
        /// </summary>
        public Texture2D SpriteSheet { get; }
        
        /// <summary>
        /// The dimensions in tiles of the object.
        /// </summary>
        public Vector2 Dimensions { get; }

        /// <summary>
        /// The tile of the map the object occupies, where 0.0 is the upper left corner.
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

        public SpriteSortMethod SpriteSortMethod { get; } = SpriteSortMethod.HeightAsDistance;

        public Vector2 DrawPosition { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the draw position.
        /// Should be called whenever anything affecting the drawposition is changed (position, camera rotation).
        /// </summary>
        /// <param name="camera">The camera this component will be rendered to.</param>
        public void CalculateDrawPosition(Camera camera)
        {
            int rotations = camera.Rotations;

            Vector2 cornerPixels = RenderHelper.CoordsToPixels(GetLowerCorner(rotations));
            Vector2 drawPosition = (cornerPixels - pivot) + TileOffset;

            // Correction, as we need to draw in the X center of the tile, not the origin.

            drawPosition.X += RenderHelper.DefaultTileWidth / 2;
            if (Dimensions == RenderHelper.Dimension1X1)
            {
                drawPosition.Y += RenderHelper.DefaultTileHeight / 2;
            }

            DrawPosition = drawPosition;
        }

        /// <summary>
        /// Get the bounds of the sprite in pixels.
        /// </summary>
        /// <param name="camera"></param>
        /// <returns></returns>
        public Rectangle GetPixelBounds(Camera camera)
        {
            int width = RenderHelper.DefaultTileWidth * (int)Dimensions.X;
            return new Rectangle(DrawPosition.ToPoint(), new Point(width, SpriteSheet.Height));
        }

        /// <summary>
        /// Sets the position of the WorldObject to the given coordinates.
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
            Coordinates = coordinates;
            TileOffset = offset;
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
                float clamp = RenderHelper.SqrtTwoReciprocal;
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

        //public T GetClosestWorldObject<T>(List<T> objects) where T : WorldObject
        //{
        //    T closestObject = null;
        //    float closestDistance = float.PositiveInfinity;

        //    for (int i = 0; i < objects.Count; i++)
        //    {
        //        Vector2 currentDirection = objects[i].Coordinates - Coordinates;
        //        float currentDistance = (currentDirection.X * currentDirection.X) + (currentDirection.Y * currentDirection.Y);

        //        if (currentDistance < closestDistance)
        //        {
        //            closestDistance = currentDistance;
        //            closestObject = objects[i];
        //        }
        //    }

        //    return closestObject;
        //}

        //public T GetFarthestWorldObject<T>(List<T> objects) where T : WorldObject
        //{
        //    T farthestObject = null;
        //    float farthestDistance = float.NegativeInfinity;

        //    for (int i = 0; i < objects.Count; i++)
        //    {
        //        Vector2 currentDirection = objects[i].Coordinates - Coordinates;
        //        float currentDistance = (currentDirection.X * currentDirection.X) + (currentDirection.Y * currentDirection.Y);

        //        if (currentDistance > farthestDistance)
        //        {
        //            farthestDistance = currentDistance;
        //            farthestObject = objects[i];
        //        }
        //    }

        //    return farthestObject;
        //}

        internal int GetDrawHeight(int worldRotations)
        {
            Vector2 cornerCoordinates = Coordinates;

            if (Dimensions != RenderHelper.Dimension1X1)
            {
                cornerCoordinates.X -= worldRotations / 2 * (Dimensions.X - 1); // For rotations 2 and 3.
                cornerCoordinates.Y -= worldRotations == 1 || worldRotations == 2 ? 1 * (Dimensions.Y - 1) : 0; // For rotations 1 and 2.
            }      

            Vector2 iso = RenderHelper.CoordsToIsometric(cornerCoordinates);
            Vector2 terrainDimensions = RenderHelper.TerrainSize;

            switch (worldRotations)
            {
                case 0:
                    return (int)iso.Y;
                case 1:
                    return (int)(iso.X + (terrainDimensions.X - 1));
                case 2:
                    return (int)(((terrainDimensions.Y - 1) * 2) - iso.Y);
                case 3:
                    return (int)(((terrainDimensions.X - 1) * 2) - (iso.X + (terrainDimensions.X - 1)));
            }

            return 0;
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            //if(directionalLighting != null)
            //{
            //    directionalLighting.Parameters["imageSize"].SetValue(new Vector2(DrawData.Texture.Width, DrawData.Texture.Height));
            //    directionalLighting.Parameters["sourceRectangle"].SetValue(new Vector4(sourceRectangle.X, sourceRectangle.Y, sourceRectangle.Width, sourceRectangle.Height));
            //    //directionalLighting.CurrentTechnique.Passes["DirectionalLighting"].Apply();
            //}

            spriteBatch.Draw(SpriteSheet, DrawPosition, GetSourceRectangle(camera.Rotations), Color.White);
        }

        //internal void DrawOutline(SpriteBatch spriteBatch, Matrix transform, int worldRotations, Color colour)
        //{
        //    Rectangle sourceRectangle = GetSourceRectangle(worldRotations);

        //    Effect outlineShader = EffectDictionary.GetEffect(EffectIDs.PixelShaders);
        //    outlineShader.Parameters["imageSize"].SetValue(new Vector2(DrawData.Texture.Width, DrawData.Texture.Height));
        //    outlineShader.Parameters["sourceRectangle"].SetValue(new Vector4(sourceRectangle.X, sourceRectangle.Y, sourceRectangle.Width, sourceRectangle.Height));
        //    outlineShader.Parameters["colour"].SetValue(colour.ToVector3());
        //    //outlineShader.CurrentTechnique.Passes["Outline"].Apply();

        //    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, outlineShader, transform);
        //    spriteBatch.Draw(DrawData.Texture, GetDrawPosition(worldRotations), sourceRectangle, colour);
        //    spriteBatch.End();
        //}


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
        /// Gets the lowest corner's coordinates from an isometric sprite.
        /// </summary>
        /// <param name="worldRotations"></param>
        /// <returns></returns>
        private Vector2 GetLowerCorner(int worldRotations)
        {
            Vector2 rotatedCoords = RenderHelper.RotateCoordsInMap(Coordinates + TileOffset, worldRotations);

            if (Dimensions != RenderHelper.Dimension1X1)
            {
                rotatedCoords.X += worldRotations == 1 || worldRotations == 2 ? Dimensions.X - 1 : 0;
                rotatedCoords.Y += worldRotations / 2 * (Dimensions.Y - 1);
            }

            return rotatedCoords;
        }

        /// <summary>
        /// Gets the source rectangle for drawing the correct frame of the spritesheet.
        /// </summary>
        /// <param name="worldRotations"></param>
        /// <returns></returns>
        private Rectangle GetSourceRectangle(int worldRotations)
        {
            int totalRotations = (worldRotations + Rotation) % 4;
            int frameX = RenderHelper.DefaultTileWidth * (int)Dimensions.X * totalRotations;
            return new Rectangle(frameX, 0, RenderHelper.DefaultTileWidth * (int)Dimensions.X, SpriteSheet.Height);
        }

        #endregion
    }

    //public abstract class WorldRenderer
    //{
    //    public abstract void GetDrawPosition();

    //    public virtual void Draw(SpriteBatch spriteBatch)
    //    {

    //    }
    //}

    //public class CarthesianRenderer : WorldRenderer
    //{
    //    public override void GetDrawPosition()
    //    {
    //        Vector2 cornerPixels = RenderHelper.CoordsToPixels(GetLowerCorner(worldRotations));
    //        Vector2 drawPosition = (cornerPixels - pivot) + TileOffset;

    //        // Correction, as we need to draw in the X center of the tile, not the origin.

    //        drawPosition.X += RenderHelper.DefaultTileWidth / 2;
    //        if (Dimensions == RenderHelper.Dimension1X1)
    //        {
    //            drawPosition.Y += RenderHelper.DefaultTileHeight / 2;
    //        }

    //        return drawPosition;
    //    }
    //}

    //public class IsometricRenderer : WorldRenderer
    //{
    //    public override void Draw(SpriteBatch spriteBatch)
    //    {
    //        base.Draw(spriteBatch);
    //    }

    //    public override void GetDrawPosition()
    //    {
    //        Vector2 cornerPixels = RenderHelper.CoordsToPixels(GetLowerCorner(worldRotations));
    //        Vector2 drawPosition = (cornerPixels - pivot) + TileOffset;

    //        // Correction, as we need to draw in the X center of the tile, not the origin.

    //        drawPosition.X += RenderHelper.DefaultTileWidth / 2;

    //        if (Dimensions == RenderHelper.Dimension1X1)
    //        {
    //            drawPosition.Y += RenderHelper.DefaultTileHeight / 2;
    //        }

    //        return drawPosition;
    //    }
    //}
}
