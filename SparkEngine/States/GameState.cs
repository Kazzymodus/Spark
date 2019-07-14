namespace SparkEngine.States
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Components;
    using SparkEngine.Entities;
    using SparkEngine.Input;
    using SparkEngine.Rendering;
    using SparkEngine.Systems;

    /// <summary>
    /// Describes a part of your game with its own entities.
    /// </summary>
    public class GameState
    {
        private const int MaxEntities = 4096;

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

        public Camera Camera { get; }

        private static int nextEntityId = 1;

        private static List<int> availableEntityIdPool = new List<int>();

        private static int highestEntityId = 0;

        Component[][] entityComponentTable = new Component[MaxEntities][];

        List<ComponentSystem> componentSystems = new List<ComponentSystem>();

        private readonly DrawLayerCollection drawLayers = new DrawLayerCollection()
        {
            new DrawLayer(DefaultDrawLayerName, true, Vector2.One, Vector2.Zero)
        };

        #endregion

        #region Methods

        //public void AddComponentToDrawLayer(Drawable component, string drawLayerName)
        //{
        //    AddComponentToDrawLayer(component, drawLayers[drawLayerName]);
        //}

        //public void AddComponentToDrawLayer(Drawable component, DrawLayer drawLayer)
        //{
        //    drawLayer.RegisterComponent(component, Camera);
        //}

        public DrawLayer CreateNewDrawLayer(string name, bool isScreenLayer, Vector2 unit)
        {
            return CreateNewDrawLayer(name, isScreenLayer, unit, Vector2.Zero);
        }

        public DrawLayer CreateNewDrawLayer(string name, bool isScreenLayer, Vector2 unit, Vector2 position)
        {
            DrawLayer layer = new DrawLayer(name, isScreenLayer, unit, position);
            drawLayers.Add(layer);

            return layer;
        }

        public int CreateNewEntity(params Component[] components)
        {
            return CreateNewEntity(drawLayers[0], components);
        }

        public int CreateNewEntity(string drawLayerName, params Component[] components)
        {
            return CreateNewEntity(drawLayers[drawLayerName], components);
        }

        public int CreateNewEntity(DrawLayer drawLayer, params Component[] components)
        {
            int entity = GetAvailableEntityID(out bool usedIdFromPool);
            
            if (usedIdFromPool)
            {
                availableEntityIdPool.Remove(entity);
            }
            else
            {
                nextEntityId++;
                highestEntityId = entity;
            }

            AddEntityToApplicableSystems(entity);

            entityComponentTable[entity] = components;

            return entity;
        }

        public static void SetDefaultCamera(Camera camera)
        {
            DefaultCamera = camera;
        }

        public T GetComponentOfEntity<T>(int entity) where T : Component
        {
            foreach (Component component in entityComponentTable[entity])
            {
                if (component is T)
                {
                    return (T)component;
                }
            }

            return null;
        }

        public Component[] GetComponentsOfEntity(int entity)
        {
            return entityComponentTable[entity];
        }
        
        public void AddEntityToApplicableSystems(int entity)
        {
            foreach (ComponentSystem system in componentSystems)
            {
                if (DoesEntityContainComponents(entity, system.RequiredComponents))
                {
                    system.AddEntity(entity);
                }
            }
        }

        public bool DoesEntityContainComponents(int entity, Type[] components)
        {
            Component[] componentTable = entityComponentTable[entity];

            for (int i = 0; i < components.Length; i++)
            {
                bool containsSpecificComponent = false;

                for (int j = 0; j < componentTable.Length; j++)
                {
                    if (componentTable[j].GetType() == components[i])
                    {
                        containsSpecificComponent = true;
                        break;
                    }
                }

                if (!containsSpecificComponent)
                {
                    return false;
                }
            }

            return true;
        }

        internal void Update(GameTime gameTime, InputHandler input)
        {
            foreach (ComponentSystem system in componentSystems)
            {
                system.UpdateAll(this, gameTime, input);
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            drawLayers.Draw(spriteBatch, Camera);
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

        //private List<Component> GetComponentsAtCursor(Point cursorPosition)
        //{
        //    List<Component> cursorComponents = new List<Component>();

        //    foreach (DrawLayer drawLayer in drawLayers)
        //    {
        //        Component cursorComponent = drawLayer.GetComponentAtPosition(cursorPosition, Camera);

        //        if (cursorComponent != null)
        //        {
        //            cursorComponents.Add(cursorComponent);
        //        }
        //    }

        //    return cursorComponents;
        //}

        #endregion
    }
}
