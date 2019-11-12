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
        public bool IsUpdating { get; protected set; }
        
        protected readonly List<int> pendingRemovals = new List<int>();

        public abstract bool HasComponentOfEntity(int owner);

        public abstract void DestroyComponent(int entity, GameState gameState);

        internal abstract void Update(GameState state, GameTime gameTime, InputHandler input);
    }

    public abstract class ComponentSystem<T> : ComponentSystem where T : struct, IComponent
    {
        protected ComponentPool<T> Subscribers { get; }

        private Dictionary<int, T> pendingAdds = new Dictionary<int, T>();

        public ComponentSystem(int maxSubs = GameState.MaxEntities)
        {
            Subscribers = new ComponentPool<T>(maxSubs, GameState.MaxEntities);
        }

        public T? GetComponent(int entity)
        {
            return Subscribers.GetComponent(entity);
        }

        public override bool HasComponentOfEntity(int entity)
        {
            return Subscribers.HasComponentOfEntity(entity);
        }

        public void AddNewComponentToEntity(T template, int entity, GameState state)
        {
            RegisterComponent(template, entity, state);
        }

        public virtual void OnAddEntity(int entity, GameState state)
        {

        }

        public virtual void OnRemoveEntity(int entity, GameState state)
        {

        }

        internal override void Update(GameState state, GameTime gameTime, InputHandler input)
        {
            IsUpdating = true;

            T[] components = Subscribers.GetComponentsByReference();
            List<int> skipList = Subscribers.AvailableIndices;

            for (int i = 0; i < Subscribers.NextIndex; i++)
            {
                if (skipList.Contains(i))
                {
                    skipList.Remove(i);
                    continue;
                }

                UpdateComponent(ref components[i], state, gameTime, input);
            }            

            IsUpdating = false;

            if (pendingAdds.Count > 0)
            {
                foreach (KeyValuePair<int, T> pending in pendingAdds)
                {
                    if (!Subscribers.HasComponentOfEntity(pending.Key))
                    {
                        AddComponent(pending.Value, pending.Key);
                    }
                }

                pendingAdds.Clear();
            }

            if (pendingRemovals.Count> 0)
            {
                foreach (int pending in pendingRemovals)
                {
                    if (Subscribers.HasComponentOfEntity(pending))
                    {
                        RemoveComponent(pending);
                    }
                }

                pendingRemovals.Clear();
            }
        }

        public abstract void UpdateComponent(ref T component, GameState state, GameTime gameTime, InputHandler input);

        public virtual void RegisterComponent(T component, int owner, GameState state)
        {
            if (IsUpdating)
            {
                pendingAdds.Add(owner, component);
            }
            else
            {
                AddComponent(component, owner);
            }
            OnAddEntity(owner, state);
        }

        public override void DestroyComponent(int owner, GameState state)
        {
            if (IsUpdating)
            {
                pendingRemovals.Add(owner);
            }
            else
            {
                RemoveComponent(owner);
            }
            OnRemoveEntity(owner, state);
        }

        private void AddComponent(T component, int owner)
        {
            Subscribers.Add(component, owner);
        }

        private void RemoveComponent(int owner)
        {
            Subscribers.Remove(owner);           
        }
    }
}
