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
    abstract class DrawSystem : ComponentSystem
    {
        private int?[] layerSeperatingIndices;

        public DrawLayerCollection DrawLayers { get; }

        public DrawSystem(Type drawableType)
            : base(typeof(Drawable), drawableType)
        {
            // This is bad, fix this sometimes

            layerSeperatingIndices = new int?[DrawLayers.Count];

            for (int i = 0; i < layerSeperatingIndices.Length; i++)
            {
                layerSeperatingIndices[i] = null;
            }
        }

        public override void AddEntity(int entity, GameState state)
        {
            int layerIndex = state.GetComponentOfEntity<Drawable>(entity).DrawLayer;

            if (layerIndex == layerSeperatingIndices.Length - 1)
            {
                validEntities.Add(entity);
            }
            else
            {
                if (layerIndex >= layerSeperatingIndices.Length)
                {
                    int?[] oldArray = layerSeperatingIndices;
                    layerSeperatingIndices = new int?[layerIndex + 1];

                    for (int i = 0; i < layerSeperatingIndices.Length; i++)
                    {
                        layerSeperatingIndices[i] = i < oldArray.Length ? oldArray[i] : null;
                    }
                }

                int? insertIndex = layerSeperatingIndices[layerIndex + 1];

                if (insertIndex == null)
                {
                    for (int i = layerIndex + 2; i < layerSeperatingIndices.Length; i++) // + 2 because we've already checked + 1
                    {
                        if (layerSeperatingIndices[i] != null)
                        {
                            insertIndex = layerSeperatingIndices[i];
                            break;
                        }
                    }
                }

                if (insertIndex == null)
                {
                    validEntities.Add(entity);

                    if (layerSeperatingIndices[layerIndex] == null)
                    {
                        layerSeperatingIndices[layerIndex] = validEntities.Count - 1;
                    }
                }
                else
                {
                    validEntities.Insert((int)insertIndex, entity);

                    if (layerSeperatingIndices[layerIndex] == null)
                    {
                        layerSeperatingIndices[layerIndex] = insertIndex;
                    }

                    for (int i = layerIndex + 1; i < layerSeperatingIndices.Length; i++)
                    {
                        layerSeperatingIndices[i]++;
                    }
                }
            }
        }

        public override void DrawAll(GameState state, SpriteBatch spriteBatch, Camera camera)
        {
            foreach (int drawable in validEntities)
            {
                DrawIndividual(state, spriteBatch, camera, state.GetComponentsOfEntity(drawable));
            }
        }
    }
}
