using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SparkEngine.Components;
using SparkEngine.Rendering;
using SparkEngine.States;
using SparkEngine.Utilities;

namespace SparkEngine.Systems
{
    public abstract class DrawSystem : ComponentSystem
    {
        public DrawSystem(bool noUpdate, params Type[] requiredComponents)
            : base(requiredComponents) 
        {
            NoUpdate = noUpdate;
        }

        protected int?[] LayerSeperatingIndices { get; private set; }

        /// <summary>
        /// Whether or not this system should have its Update functions called.
        /// </summary>
        public bool NoUpdate { get; }

        public override void AddEntity(int entity, GameState state)
        {
            int layerIndex = state.GetComponentOfEntity<Drawable>(entity).DrawLayer;
            if (LayerSeperatingIndices == null || layerIndex >= LayerSeperatingIndices.Length)
            {
                ExtendLayersToElement(layerIndex);
            }

            int insertIndex = GetInsertIndex(layerIndex);
            subbedEntities.Insert(insertIndex, entity);
            UpdateLayerSeperatingIndices(insertIndex, layerIndex);

            OnAddEntity(entity, state);
        }

        public virtual void DrawAll(GameState state, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Matrix cameraTransform)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, cameraTransform);

            foreach (int drawable in subbedEntities)
            {
                DrawIndividual(state, graphicsDevice, spriteBatch, Vector2.Zero, state.GetAllComponentsOfEntity(drawable));
            }

            spriteBatch.End();
        }

        public abstract void DrawIndividual(GameState state, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Vector2 drawOffset, ComponentBatch components);

        protected Vector2 GetDrawPosition(DrawLayer layer, Drawable drawable)
        {

            // Does the Unit affect both drawable.position AND layer.position? Need to make a decision on that someday.

            return layer.Position + layer.UnitSize * drawable.DrawPosition;
        }

        /// <summary>
        /// Gets the range of coordinates currently in camera view.
        /// </summary>
        /// <returns>A rectangle containing all visible coordinates.</returns>
        protected Rectangle GetVisibleCartesianCoordinates(Vector2 cameraPosition, Point viewportSize, Vector2 unit, int padding = 0)
        {
            Point location = Projector.PixelsToCartesian(cameraPosition.ToPoint(), unit) - new Point(padding);
            Point size = Projector.PixelsToCartesian(viewportSize, unit) + new Point(padding * 2);

            return new Rectangle(location, size);
        }

        protected Rectangle GetVisibleIsometricCoordinates(Vector2 cameraPosition, Point viewportSize, Vector2 unit, int padding = 0)
        {
            Point location = Projector.PixelsToIsometric(cameraPosition, unit).ToPoint() - new Point(padding);
            Point size = Projector.PixelsToIsometric(viewportSize, unit) + new Point(padding * 2);

            return new Rectangle(location, size);
        }

        private void ExtendLayersToElement(int lastLayerIndex)
        {
            int?[] oldArray = LayerSeperatingIndices ?? new int?[0];
            LayerSeperatingIndices = new int?[lastLayerIndex + 1];

            for (int i = 0; i < LayerSeperatingIndices.Length; i++)
            {
                LayerSeperatingIndices[i] = i < oldArray.Length ? oldArray[i] : null;
            }
        }

        private int GetInsertIndex(int layerIndex)
        {
            if (layerIndex == LayerSeperatingIndices.Length - 1)
            {
                return subbedEntities.Count;
            }
            else
            {
                for (int i = layerIndex + 1; i < LayerSeperatingIndices.Length; i++)
                {
                    if (LayerSeperatingIndices[i] != null)
                    {
                        return (int)LayerSeperatingIndices[i];
                    }
                }

                throw new InvalidOperationException("layerSeperatingIndices' final element is null and not the only element in the array.");
            }
        }

        private void UpdateLayerSeperatingIndices(int insertIndex, int layerIndex)
        {
            if (LayerSeperatingIndices[layerIndex] == null)
            {
                if (layerIndex == LayerSeperatingIndices.Length - 1)
                {
                    LayerSeperatingIndices[layerIndex] = subbedEntities.Count - 1;
                    return;
                }
                else
                {
                    LayerSeperatingIndices[layerIndex] = insertIndex;
                }
            }

            for (int i = layerIndex + 1; i < LayerSeperatingIndices.Length; i++)
            {
                LayerSeperatingIndices[i]++;
            }
        }

        public ComponentDrawMethod ExtractDraw()
        {
            return new ComponentDrawMethod(this);
        }
    }
}
