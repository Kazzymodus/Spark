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
    abstract class DrawSystem : ComponentSystem
    {
        private int?[] layerSeperatingIndices;

        public DrawSystem(params Type[] requiredComponents)
            : base(requiredComponents) 
        {

        }

        public override void AddEntity(int entity, GameState state)
        {
            int layerIndex = state.GetComponentOfEntity<Drawable>(entity).DrawLayer;
            if (layerSeperatingIndices == null || layerIndex >= layerSeperatingIndices.Length)
            {
                ExtendLayersToElement(layerIndex);
            }

            int insertIndex = GetInsertIndex(layerIndex);
            subbedEntities.Insert(insertIndex, entity);
            UpdateLayerSeperatingIndices(insertIndex, layerIndex);            
        }

        public virtual void DrawAll(GameState state, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            Vector2 cameraPosition = state.CameraPosition;

            foreach (int drawable in subbedEntities)
            {
                DrawIndividual(state, graphicsDevice, spriteBatch, cameraPosition, state.GetAllComponentsOfEntity(drawable));
            }
        }

        public abstract void DrawIndividual(GameState state, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Vector2 cameraPosition, ComponentBatch components);

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
            int?[] oldArray = layerSeperatingIndices ?? new int?[0];
            layerSeperatingIndices = new int?[lastLayerIndex + 1];

            for (int i = 0; i < layerSeperatingIndices.Length; i++)
            {
                layerSeperatingIndices[i] = i < oldArray.Length ? oldArray[i] : null;
            }
        }

        private int GetInsertIndex(int layerIndex)
        {
            if (layerIndex == layerSeperatingIndices.Length - 1)
            {
                return subbedEntities.Count;
            }
            else
            {
                for (int i = layerIndex + 1; i < layerSeperatingIndices.Length; i++)
                {
                    if (layerSeperatingIndices[i] != null)
                    {
                        return (int)layerSeperatingIndices[i];
                    }
                }

                throw new InvalidOperationException("layerSeperatingIndices' final element is null and not the only element in the array.");
            }
        }

        private void UpdateLayerSeperatingIndices(int insertIndex, int layerIndex)
        {
            if (layerSeperatingIndices[layerIndex] == null)
            {
                if (layerIndex == layerSeperatingIndices.Length - 1)
                {
                    layerSeperatingIndices[layerIndex] = subbedEntities.Count - 1;
                    return;
                }
                else
                {
                    layerSeperatingIndices[layerIndex] = insertIndex;
                }
            }

            for (int i = layerIndex + 1; i < layerSeperatingIndices.Length; i++)
            {
                layerSeperatingIndices[i]++;
            }
        }
    }
}
