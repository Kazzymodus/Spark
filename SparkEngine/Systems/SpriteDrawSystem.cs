namespace SparkEngine.Systems
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
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Rendering;

    public class SpriteDrawSystem : ComponentSystem<Sprite>, IDrawSystem
    {
        public SpriteDrawSystem(int maxSubs = GameState.MaxEntities)
            : base(maxSubs)
        {

        }

        public override void UpdateComponent(ref Sprite sprite, GameState state, GameTime gameTime, InputHandler input)
        {
        }

        protected int?[] LayerSeperatingIndices { get; private set; }

        //private void ExtendLayersToElement(int lastLayerIndex)
        //{
        //    int?[] oldArray = LayerSeperatingIndices ?? new int?[0];
        //    LayerSeperatingIndices = new int?[lastLayerIndex + 1];

        //    for (int i = 0; i < LayerSeperatingIndices.Length; i++)
        //    {
        //        LayerSeperatingIndices[i] = i < oldArray.Length ? oldArray[i] : null;
        //    }
        //}

        //private int GetInsertIndex(int layerIndex)
        //{
        //    if (layerIndex == LayerSeperatingIndices.Length - 1)
        //    {
        //        return subbedEntities.Count;
        //    }
        //    else
        //    {
        //        for (int i = layerIndex + 1; i < LayerSeperatingIndices.Length; i++)
        //        {
        //            if (LayerSeperatingIndices[i] != null)
        //            {
        //                return (int)LayerSeperatingIndices[i];
        //            }
        //        }

        //        throw new InvalidOperationException("layerSeperatingIndices' final element is null and not the only element in the array.");
        //    }
        //}

        //private void UpdateLayerSeperatingIndices(int insertIndex, int layerIndex)
        //{
        //    if (LayerSeperatingIndices[layerIndex] == null)
        //    {
        //        if (layerIndex == LayerSeperatingIndices.Length - 1)
        //        {
        //            LayerSeperatingIndices[layerIndex] = subbedEntities.Count - 1;
        //            return;
        //        }
        //        else
        //        {
        //            LayerSeperatingIndices[layerIndex] = insertIndex;
        //        }
        //    }

        //    for (int i = layerIndex + 1; i < LayerSeperatingIndices.Length; i++)
        //    {
        //        LayerSeperatingIndices[i]++;
        //    }
        //}

        //public override void AddComponent(int entity, GameState state)
        //{
        //    int layerIndex = state.GetComponentOfEntity<Drawable>(entity).DrawLayer;
        //    if (LayerSeperatingIndices == null || layerIndex >= LayerSeperatingIndices.Length)
        //    {
        //        ExtendLayersToElement(layerIndex);
        //    }

        //    int insertIndex = GetInsertIndex(layerIndex);
        //    subbedEntities.Insert(insertIndex, entity);
        //    UpdateLayerSeperatingIndices(insertIndex, layerIndex);

        //    OnAddEntity(entity, state);
        //}

        public void Draw(GameState state, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Matrix cameraTransform)
        {
            Vector2[] layerOffsets = new Vector2[state.DrawLayers.Count];

            for (int i = 0; i < layerOffsets.Length; i++)
            {
                layerOffsets[i] = state.DrawLayers[i].DrawOffset;
            }

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, cameraTransform);

            Sprite[] sprites = Subscribers.GetComponentsCompact();

            for (int i = 0; i < Subscribers.NextIndex; i++)
            {
                DrawSprite(state, spriteBatch, sprites[i], layerOffsets);
            }

            spriteBatch.End();
        }

        public void DrawSprite(GameState state, SpriteBatch spriteBatch, Sprite sprite, Vector2[] layerOffsets)
        {
            Rectangle sourceRectangle;

            if (sprite.IsAnimated)
            {
                int frameWidth = (int)sprite.FrameSize.X;
                int frameHeight = (int)sprite.FrameSize.Y;
                sourceRectangle = new Rectangle(sprite.FrameX * frameWidth, sprite.FrameY * frameHeight, frameWidth, frameHeight);
            }
            else
            {
                sourceRectangle = new Rectangle(Point.Zero, sprite.FrameSize.ToPoint());
            }

            spriteBatch.Draw(sprite.Texture, sprite.DrawPosition + layerOffsets[sprite.DrawLayer], sourceRectangle, sprite.ColorMask);
        }

        //public Vector2 GetDrawPosition(Sprite sprite)
        //{
        //int rotations = camera.Rotations;

        // This is what it used to do, prolly not relevant anymore

        //Vector2 cornerPixels = Projector.CartesianToIsometricToPixels(GetRootCoordinates(rotations), drawLayer.Unit);
        //Vector2 drawPosition = drawLayer.Position + (cornerPixels - Sprite.Anchor);

        //// Correction, as we need to draw in the X center of the tile, not the origin.

        //drawPosition.X += Projector.DefaultTileWidth / 2;
        //if (Dimensions == Projector.Dimension1X1)
        //{
        //    drawPosition.Y += Projector.DefaultTileHeight / 2;
        //}

        //return DrawLayers[sprite.DrawLayer].Position + sprite.DrawPosition;
        //}

        //public Rectangle GetBounds(Camera camera, DrawLayer drawLayer)
        //{
        //    Rectangle bounds = Sprite.Texture.Bounds;
        //    bounds.Location = GetDrawPosition(camera, drawLayer).ToPoint();
        //    return bounds;
        //}

        /// <summary>
        /// Sets the position of the WorldObject to the given coordinates.
        /// </summary>
        /// <param name="coordinates"></param>
        /// <param name="offset"></param>
        //public void SetPosition(Vector2 coordinates)
        //{
        //    Coordinates = coordinates;
        //}

        /// <summary>
        /// Translate the object.
        /// </summary>
        /// <param name="amount">The amount of units to translate.</param>
        //public void Translate(Vector2 amount)
        //{
        //    Coordinates += amount;
        //}

        //public void Draw(SpriteBatch spriteBatch, Camera camera, DrawLayer layer)
        //{
        //    Vector2 frameSize = Sprite.FrameSize;
        //    int frameX = (int)frameSize.X * (Rotation + camera.Rotations) % 4;
        //    int frameY = 0; // For now;

        //    Rectangle frame = new Rectangle(frameX, frameY, (int)frameSize.X, (int)frameSize.Y);
        //    Vector2 drawPosition = GetDrawPosition(camera, layer);

        //    //Log.AddWorldMessage("DP: " + drawPosition.X + "," + drawPosition.Y, drawPosition, camera, Color.Red);
        //    //Log.AddWorldMessage("A: " + SpriteData.Anchor.X + "," + SpriteData.Anchor.Y, drawPosition + SpriteData.Anchor, camera, Color.Red);

        //    Log.AddWorldMessage(Coordinates.ToString(), drawPosition, camera, Color.Red);

        //    spriteBatch.Draw(Sprite.Texture, drawPosition, frame, new Color(Color.White, 0.5f));
        //}

        /// <summary>
        /// Gets the root coordinates. I'll explain this better later. Doesn't do anything at this stage anyway.
        /// </summary>
        /// <param name="worldRotations"></param>
        /// <returns></returns>
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
    }
}
