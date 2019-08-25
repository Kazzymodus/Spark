using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using SparkEngine.Components;
using SparkEngine.Rendering;
using SparkEngine.States;

namespace SparkEngine.Systems
{
    abstract class DrawSystem<T> : ComponentSystem where T : Drawable
    {
        private int?[] layerSeperatingIndices;

        public DrawLayerCollection DrawLayers { get; }

        public DrawSystem()
            : base(typeof(T)) 
        {
            // This is bad, fix this sometime

            layerSeperatingIndices = new int?[DrawLayers.Count];

            for (int i = 0; i < layerSeperatingIndices.Length; i++)
            {
                layerSeperatingIndices[i] = null;
            }
        }

        public override void AddEntity(int entity, GameState state)
        {
            int layerIndex = state.GetComponentOfEntity<T>(entity).DrawLayer;
            if (layerIndex >= layerSeperatingIndices.Length)
            {
                ExtendLayersToElement(layerIndex);
            }

            int insertIndex = GetInsertIndex(layerIndex);
            subbedEntities.Insert(insertIndex, entity);
            UpdateLayerSeperatingIndices(insertIndex, layerIndex);            
        }

        public virtual void DrawAll(GameState state, SpriteBatch spriteBatch, int cameraEntity)
        {
            foreach (int drawable in subbedEntities)
            {
                DrawIndividual(state, spriteBatch, cameraEntity, state.GetAllComponentsOfEntity(drawable));
            }
        }

        public abstract void DrawIndividual(GameState state, SpriteBatch spriteBatch, int cameraEntity, ComponentBatch components);

        private void ExtendLayersToElement(int lastLayerIndex)
        {
            int?[] oldArray = layerSeperatingIndices;
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
