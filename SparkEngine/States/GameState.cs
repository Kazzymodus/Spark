namespace SparkEngine.States
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Components;
    using SparkEngine.Entities;
    using SparkEngine.Rendering;
    using SparkEngine.UI;

    /// <summary>
    /// Describes a slice of
    /// </summary>
    public class GameState
    {
        #region Constructors

        internal GameState(StateActivityLevel activityLevel, Camera camera = null)
        {
            if (camera == null)
            {
                Camera = DefaultCamera;
            }
            else
            {
                Camera = camera;
            }
        }

        #endregion

        #region Properties

        public static Camera DefaultCamera { get; private set; }

        public bool UsesDefaultCamera
        {
            get
            {
                return Camera.Equals(DefaultCamera);
            }
        }

        public StateActivityLevel ActivityLevel { get; set; }

        // public StateUI StateUI { get; protected set; }

        public Camera Camera { get; }

        private static int nextEntityId = 1;

        private static List<int> availableEntityIdPool;

        private List<ComponentManager> ComponentManagers { get; }

        private List<Entity> entities = new List<Entity>();

        private Dictionary<string, DrawLayer> drawLayers = new Dictionary<string, DrawLayer>
        {
            { "Default", new DrawLayer() }
        };

        #endregion

        #region Methods

        internal static void SetDefaultCamera(Camera camera)
        {
            DefaultCamera = camera;
        }

        protected internal virtual void ProcessInput(GameTime gameTime)
        {     
            foreach (ComponentManager manager in ComponentManagers)
            {
                manager.Update(gameTime);
            }
        }

        protected internal virtual void Update(GameTime gameTime)
        {
            foreach (ComponentManager manager in ComponentManagers)
            {
                manager.Update(gameTime);
            }
        }

        protected internal virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (KeyValuePair<string, DrawLayer> entries in drawLayers)
            {
                entries.Value.Draw(spriteBatch, Camera);
            }
        }

        private int CreateNewEntity(params Component[] components)
        {
            int id = GetAvailableEntityID(out bool usedIdFromPool);
            Entity entity = new Entity(id);
            if(usedIdFromPool)
            {
                availableEntityIdPool.Remove(id);
            }
            else
            {
                nextEntityId++;
            }

            foreach (Component component in components)
            {
                AddComponentToEntity(entity, component);
            }

            entities.Add(entity);

            return id;
        }

        private void AddComponentToEntity<TComponent>(Entity entity, TComponent component) where TComponent : Component
        {
            component.SetOwner(entity);

            ComponentManager<TComponent> manager = GetComponentManager<TComponent>();

            if (manager == null)
            {
                manager = CreateNewComponentManager<TComponent>();
            }

            manager.AddComponent(component);
        }

        private ComponentManager<TComponent> GetComponentManager<TComponent>() where TComponent : Component
        {
            foreach (ComponentManager manager in ComponentManagers)
            {
                if (manager is ComponentManager<TComponent>)
                {
                    return manager as ComponentManager<TComponent>;
                }
            }

            return null;
        }

        private ComponentManager<TComponent> CreateNewComponentManager<TComponent>() where TComponent : Component
        {
            ComponentManager<TComponent> manager = new ComponentManager<TComponent>();
            ComponentManagers.Add(manager);
            return manager;
        }

        private static int GetAvailableEntityID(out bool usedIdFromPool)
        {
            int newId = availableEntityIdPool.FirstOrDefault();
            usedIdFromPool = newId > 0;

            if (!usedIdFromPool)
            {
                newId = nextEntityId;
            }

            return newId;
        }

        #endregion
    }
}
