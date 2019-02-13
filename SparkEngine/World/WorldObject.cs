﻿namespace SparkEngine.World
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.DataStructures;
    using SparkEngine.Dictionaries;
    using SparkEngine.IDs;
    using SparkEngine.Rendering;
    using SparkEngine.States;
    using SparkEngine.UI;

    /// <summary>
    /// An object situated within the world.
    /// </summary>
    public abstract class WorldObject
    {
        #region Fields

        public readonly float SqrtTwoReciprocal = 1f / (float)Math.Sqrt(2); // Move this somewhere better.

        /// <summary>
        /// Determines the rotation point of the object (relative to a single frame).
        /// </summary>
        private Vector2 pivot;

        #endregion

        #region Constructors

        public WorldObject(Vector2 coordinates, DrawData drawData, int rotation = RenderHelper.RotationNone)
        {
            Coordinates = coordinates;
            DrawData = drawData;
            Rotation = rotation;

            pivot.X = DrawData.Dimensions.X * (RenderHelper.DefaultTileWidth * 0.5f);
            pivot.Y = DrawData.Texture.Height - (DrawData.Dimensions.X * RenderHelper.DefaultTileHeight * 0.5f);

            Tooltip = new Tooltip();
        }

        #endregion

        #region Public Properties

        public DrawData DrawData { get; }

        /// <summary>
        /// The tile of the map the object occupies, where 0.0 is the upper left corner.
        /// </summary>
        public Vector2 Coordinates { get; private set; }

        /// <summary>
        /// The offset in tiles a tile is drawn from its root coordinates. Offset ranges from -0.5 to 0.5 (half a tile)
        /// </summary>
        public Vector2 TileOffset { get; private set; }

        public int Rotation { get; private set; }

        public Tooltip Tooltip { get; }

        public abstract bool IsPathBlocker
        {
            get;
        }

        #endregion

        #region Methods

        public Vector2 GetDrawPosition(int worldRotations)
        {
            Vector2 cornerPixels = RenderHelper.CoordsToPixels(GetLowerCorner(worldRotations));
            Vector2 drawPosition = (cornerPixels - pivot) + TileOffset;

            // Correction, as we need to draw in the X center of the tile, not the origin.

            drawPosition.X += RenderHelper.DefaultTileWidth / 2;
            if (DrawData.Dimensions == RenderHelper.Dimension1X1)
            {
                drawPosition.Y += RenderHelper.DefaultTileHeight / 2;
            }

            return drawPosition;
        }

        public Rectangle GetPixelBounds(int worldRotations)
        {
            int width = RenderHelper.DefaultTileWidth * (int)DrawData.Dimensions.X;
            return new Rectangle(GetDrawPosition(worldRotations).ToPoint(), new Point(width, DrawData.Texture.Height));
        }

        public void Translate(Vector2 amount)
        {
            float x = amount.X;
            float y = amount.Y;

            Vector2 offset = TileOffset;

            if (y != 0 && x != 0)
            {
                float clamp = SqrtTwoReciprocal;
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

        public T GetClosestFromList<T>(List<T> objects) where T : WorldObject
        {
            T closestObject = null;
            float closestDistance = float.PositiveInfinity;

            for (int i = 0; i < objects.Count; i++)
            {
                Vector2 currentDirection = objects[i].Coordinates - Coordinates;
                float currentDistance = (currentDirection.X * currentDirection.X) + (currentDirection.Y * currentDirection.Y);

                if (currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    closestObject = objects[i];
                }
            }

            return closestObject;
        }

        public T GetFarthestFromList<T>(List<T> objects) where T : WorldObject
        {
            T farthestObject = null;
            float farthestDistance = float.NegativeInfinity;

            for (int i = 0; i < objects.Count; i++)
            {
                Vector2 currentDirection = objects[i].Coordinates - Coordinates;
                float currentDistance = (currentDirection.X * currentDirection.X) + (currentDirection.Y * currentDirection.Y);

                if (currentDistance > farthestDistance)
                {
                    farthestDistance = currentDistance;
                    farthestObject = objects[i];
                }
            }

            return farthestObject;
        }

        internal int GetDrawHeight(int worldRotations)
        {
            Vector2 cornerCoordinates = Coordinates;

            if (DrawData.Dimensions != RenderHelper.Dimension1X1)
            {
                cornerCoordinates.X -= (worldRotations / 2) * (DrawData.Dimensions.X - 1);
                cornerCoordinates.Y -= worldRotations == 1 || worldRotations == 2 ? 1 * (DrawData.Dimensions.Y - 1) : 0;
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

        internal virtual void Draw(SpriteBatch spriteBatch, Color colour, int worldRotations)
        {
            //if(directionalLighting != null)
            //{
            //    directionalLighting.Parameters["imageSize"].SetValue(new Vector2(DrawData.Texture.Width, DrawData.Texture.Height));
            //    directionalLighting.Parameters["sourceRectangle"].SetValue(new Vector4(sourceRectangle.X, sourceRectangle.Y, sourceRectangle.Width, sourceRectangle.Height));
            //    //directionalLighting.CurrentTechnique.Passes["DirectionalLighting"].Apply();
            //}

            spriteBatch.Draw(DrawData.Texture, GetDrawPosition(worldRotations), GetSourceRectangle(worldRotations), colour);
        }

        internal void DrawOutline(SpriteBatch spriteBatch, Matrix transform, int worldRotations, Color colour)
        {
            Rectangle sourceRectangle = GetSourceRectangle(worldRotations);

            Effect outlineShader = EffectDictionary.GetEffect(EffectIDs.PixelShaders);
            outlineShader.Parameters["imageSize"].SetValue(new Vector2(DrawData.Texture.Width, DrawData.Texture.Height));
            outlineShader.Parameters["sourceRectangle"].SetValue(new Vector4(sourceRectangle.X, sourceRectangle.Y, sourceRectangle.Width, sourceRectangle.Height));
            outlineShader.Parameters["colour"].SetValue(colour.ToVector3());
            //outlineShader.CurrentTechnique.Passes["Outline"].Apply();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, outlineShader, transform);
            spriteBatch.Draw(DrawData.Texture, GetDrawPosition(worldRotations), sourceRectangle, colour);
            spriteBatch.End();
        }

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

        private Vector2 GetLowerCorner(int worldRotations)
        {
            Vector2 rotatedCoords = RenderHelper.RotateCoordsInMap(Coordinates + TileOffset, worldRotations);

            if (DrawData.Dimensions != RenderHelper.Dimension1X1)
            {
                rotatedCoords.X += worldRotations == 1 || worldRotations == 2 ? DrawData.Dimensions.X - 1 : 0;
                rotatedCoords.Y += worldRotations / 2 * (DrawData.Dimensions.Y - 1);
            }

            return rotatedCoords;
        }

        private Rectangle GetSourceRectangle(int worldRotations)
        {
            int totalRotations = (worldRotations + Rotation) % 4;
            int frameX = RenderHelper.DefaultTileWidth * (int)DrawData.Dimensions.X * totalRotations;
            return new Rectangle(frameX, 0, RenderHelper.DefaultTileWidth * (int)DrawData.Dimensions.X, DrawData.Texture.Height);
        }

        #endregion
    }
}
