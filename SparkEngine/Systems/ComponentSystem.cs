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

        protected List<int> subbedEntities = new List<int>();

        protected List<int> pendingAdds = new List<int>();

        protected List<int> pendingRemovals = new List<int>();

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
                subbedEntities.Add(entity);
            }

            OnAddEntity(entity, state);
        }

        public virtual void OnAddEntity(int entity, GameState state)
        {

        }

        public virtual void RemoveEntity(int entity, GameState state)
        {
            if (isUpdating)
            {
                pendingRemovals.Add(entity);
            }
            else
            {
                subbedEntities.Remove(entity);
            }

            OnRemoveEntity(entity, state);
        }

        public virtual void OnRemoveEntity(int entity, GameState state)
        {

        }

        public virtual void UpdateAll(GameState state, GameTime gameTime, InputHandler input)
        {
            isUpdating = true;

            foreach (int entity in subbedEntities)
            {
                ComponentBatch components = state.GetAllComponentsOfEntity(entity);
                components = components.GetComponentsInTypeOrder(RequiredComponents);

                UpdateIndividual(state, gameTime, input, components);
            }

            isUpdating = false;
        }

        public abstract void UpdateIndividual(GameState state, GameTime gameTime, InputHandler input, ComponentBatch components);

        public bool CanHostEntity(GameState state, int entity)
        {
            ComponentBatch entityComponents = state.GetAllComponentsOfEntity(entity);

            return CanHostEntity(entityComponents);
        }

        public bool CanHostEntity(ComponentBatch entityComponents)
        {
            return entityComponents.ContainsAll(RequiredComponents);
        }

        public ComponentUpdateMethod ExtractUpdate()
        {
            return new ComponentUpdateMethod(this);
        }
    }
}
