﻿namespace SparkEngine.Systems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using SparkEngine.Components;
    using SparkEngine.States;
    using SparkEngine.Input;

    class SpriteDrawSystem : DrawSystem
    {
        public SpriteDrawSystem(GameState state)
            : base(typeof(Sprite))
        {

        }

        /// <summary>
        /// Returns the draw position.
        /// </summary>
        /// <param name="camera">The camera this component will be rendered to.</param>
        /// <param name="tileSize">The dimensions (in pixels) of a single tile.</param>
        public Vector2 GetDrawPosition(Camera camera, DrawLayer drawLayer)
        {
            int rotations = camera.Rotations;
            Vector2 cornerPixels = Projector.CartesianToIsometricToPixels(GetRootCoordinates(rotations), drawLayer.Unit);
            Vector2 drawPosition = drawLayer.Position + (cornerPixels - Sprite.Anchor);

            //// Correction, as we need to draw in the X center of the tile, not the origin.

            //drawPosition.X += Projector.DefaultTileWidth / 2;
            //if (Dimensions == Projector.Dimension1X1)
            //{
            //    drawPosition.Y += Projector.DefaultTileHeight / 2;
            //}

            return drawPosition;
        }

        public Rectangle GetBounds(Camera camera, DrawLayer drawLayer)
        {
            Rectangle bounds = Sprite.Texture.Bounds;
            bounds.Location = GetDrawPosition(camera, drawLayer).ToPoint();
            return bounds;
        }

        /// <summary>
        /// Sets the position of the WorldObject to the given coordinates.
        /// </summary>
        /// <param name="coordinates"></param>
        /// <param name="offset"></param>
        public void SetPosition(Vector2 coordinates)
        {
            Coordinates = coordinates;
        }

        /// <summary>
        /// Translate the object.
        /// </summary>
        /// <param name="amount">The amount of units to translate.</param>
        public void Translate(Vector2 amount)
        {
            Coordinates += amount;
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera, DrawLayer layer)
        {
            Vector2 frameSize = Sprite.FrameSize;
            int frameX = (int)frameSize.X * (Rotation + camera.Rotations) % 4;
            int frameY = 0; // For now;

            Rectangle frame = new Rectangle(frameX, frameY, (int)frameSize.X, (int)frameSize.Y);
            Vector2 drawPosition = GetDrawPosition(camera, layer);

            //Log.AddWorldMessage("DP: " + drawPosition.X + "," + drawPosition.Y, drawPosition, camera, Color.Red);
            //Log.AddWorldMessage("A: " + SpriteData.Anchor.X + "," + SpriteData.Anchor.Y, drawPosition + SpriteData.Anchor, camera, Color.Red);

            Log.AddWorldMessage(Coordinates.ToString(), drawPosition, camera, Color.Red);

            spriteBatch.Draw(Sprite.Texture, drawPosition, frame, new Color(Color.White, 0.5f));
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
    }
}