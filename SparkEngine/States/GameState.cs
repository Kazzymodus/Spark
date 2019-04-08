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
    /// Describes a part of your game with its own entities.
    /// </summary>
    public class GameState
    {
        private const string DefaultDrawLayerName = "Default";

        #region Constructors

        /// <summary>
        /// Creates a new game state.
        /// </summary>
        /// <param name="activityLevel">The activity level the state starts at. Active by default.</param>
        /// <param name="camera">The camera used for drawing the game state. If not provided, uses the default camera.</param>
        public GameState(string name, StateActivityLevel activityLevel = StateActivityLevel.Active, Camera camera = null)
        {
            Name = name;
            ActivityLevel = activityLevel;
            Camera = camera ?? DefaultCamera;
        }

        #endregion

        #region Properties

        public static Camera DefaultCamera { get; private set; }

        public string Name { get; }

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

        private static List<int> availableEntityIdPool = new List<int>();

        private List<ComponentManager> ComponentManagers { get; } = new List<ComponentManager>();

        private List<Entity> entities = new List<Entity>();

        private readonly Dictionary<string, DrawLayer> drawLayers = new Dictionary<string, DrawLayer>
        {
            { DefaultDrawLayerName, new DrawLayer(true, Vector2.One, Vector2.Zero) }
        };

        #endregion

        #region Methods

        public void AddComponentToDrawLayer(IDrawableComponent component, string drawLayerName)
        {
            AddComponentToDrawLayer(component, drawLayers[drawLayerName]);
        }

        public void AddComponentToDrawLayer(IDrawableComponent component, DrawLayer drawLayer)
        {
            drawLayer.RegisterComponent(component, Camera);
        }

        public DrawLayer CreateNewDrawLayer(string name, bool isScreenLayer, Vector2 unit)
        {
            return CreateNewDrawLayer(name, isScreenLayer, unit, Vector2.Zero);
        }

        public DrawLayer CreateNewDrawLayer(string name, bool isScreenLayer, Vector2 unit, Vector2 position)
        {
            DrawLayer layer = new DrawLayer(isScreenLayer, unit, position);
            drawLayers.Add(name, layer);

            return layer;
        }

        public int CreateNewEntity(params Component[] components)
        {
            return CreateNewEntity(drawLayers[DefaultDrawLayerName], components);
        }

        public int CreateNewEntity(string drawLayerName, params Component[] components)
        {
            return CreateNewEntity(drawLayers[drawLayerName], components);
        }

        public int CreateNewEntity(DrawLayer drawLayer, params Component[] components)
        {
            int id = GetAvailableEntityID(out bool usedIdFromPool);
            Entity entity = new Entity(id);

            if (usedIdFromPool)
            {
                availableEntityIdPool.Remove(id);
            }
            else
            {
                nextEntityId++;
            }

            foreach (Component component in components)
            {
                AddComponentToEntity(entity, component, drawLayer);
            }

            entities.Add(entity);

            return id;
        }

        public static void SetDefaultCamera(Camera camera)
        {
            DefaultCamera = camera;
        }

        internal void ProcessInput(GameTime gameTime)
        {     
            foreach (ComponentManager manager in ComponentManagers)
            {
                manager.ProcessInput(gameTime);
            }
        }

        internal void Update(GameTime gameTime)
        {
            foreach (ComponentManager manager in ComponentManagers)
            {
                manager.Update(gameTime);
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            foreach (KeyValuePair<string, DrawLayer> entries in drawLayers)
            {
                entries.Value.Draw(spriteBatch, Camera);
            }
        }

        private void AddComponentToEntity<TComponent>(Entity entity, TComponent component, DrawLayer drawLayer) where TComponent : Component
        {
            component.SetOwner(entity);

            ComponentManager<TComponent> manager = GetComponentManager<TComponent>();

            if (manager == null)
            {
                manager = CreateNewComponentManager<TComponent>();
            }

            manager.AddComponent(component);

            if (component is IDrawableComponent)
            {
                drawLayer.RegisterComponent((IDrawableComponent)component, Camera);
            }
        }

        private ComponentManager<TComponent> GetComponentManager<TComponent>() where TComponent : Component
        {
            foreach (ComponentManager manager in ComponentManagers)
            {
                if (manager is ComponentManager<TComponent>)
                {
                    return (ComponentManager<TComponent>)manager;
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
