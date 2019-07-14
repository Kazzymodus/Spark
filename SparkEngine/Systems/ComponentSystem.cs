namespace SparkEngine.Systems
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SparkEngine.Components;
    using SparkEngine.Input;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Entities;
    using SparkEngine.Rendering;
    using SparkEngine.States;

    public abstract class ComponentSystem
    {
        private bool isUpdating;

        protected List<int> validEntities;

        protected List<int> pendingAdds;

        public ComponentSystem(params Type[] requiredComponents)
        {
            RequiredComponents = requiredComponents;
        }

        public Type[] RequiredComponents { get; }

        public virtual void AddEntity(int entity, GameState state)
        {
            if (isUpdating)
            {
                pendingAdds.Add(entity);
            }
            else
            {
                validEntities.Add(entity);
            }

            OnAddEntity(entity, state);
        }

        public virtual void OnAddEntity(int entity, GameState state)
        {

        }

        public virtual void UpdateAll(GameState state, GameTime gameTime, InputHandler input)
        {
            isUpdating = true;

            foreach (int entity in validEntities)
            {
                UpdateIndividual(state, gameTime, input, state.GetComponentsOfEntity(entity));
            }

            isUpdating = false;
        }

        public virtual void DrawAll(GameState state, SpriteBatch spriteBatch, Camera camera)
        {
            foreach (int entity in validEntities)
            {
                DrawIndividual(state, spriteBatch, camera, state.GetComponentsOfEntity(entity));
            }
        }

        public abstract void UpdateIndividual(GameState state, GameTime gameTime, InputHandler input, params Component[] components);

        public abstract void DrawIndividual(GameState state, SpriteBatch spriteBatch, Camera camera, params Component[] components);
    }
}
