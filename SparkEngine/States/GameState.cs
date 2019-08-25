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

        #region Constructors

        /// <summary>
        /// Creates a new game state.
        /// </summary>
        /// <param name="activityLevel">The activity level the state starts at. Active by default.</param>
        /// <param name="camera">The camera used for drawing the game state. If not provided, uses the default camera.</param>
        public GameState(string name, bool useDefaultCamera, StateActivityLevel activityLevel = StateActivityLevel.Active)
        {
            Name = name;
            ActivityLevel = activityLevel;
            
            if (useDefaultCamera)
            {
                SetRenderCamera(DefaultCamera);
            }
        }

        public GameState(string name, int cameraEntity, StateActivityLevel activityLevel = StateActivityLevel.Active)
        {
            Name = name;
            ActivityLevel = activityLevel;
            SetRenderCamera(cameraEntity);
        }

        #endregion

        #region Properties

        public static int DefaultCamera { get; private set; } = 0;

        public string Name { get; }

        public bool UsesDefaultCamera
        {
            get
            {
                return HasRenderCamera && RenderCamera == DefaultCamera;
            }
        }

        public bool HasRenderCamera
        {
            get
            {
                return RenderCamera != 0;
            }
        }

        public StateActivityLevel ActivityLevel { get; set; }

        public int RenderCamera { get; private set; }

        private int nextEntityId = 1;

        private List<int> availableEntityIdPool = new List<int>();

        private int highestEntityId = 0;

        Component[][] entityComponentTable = new Component[MaxEntities][];

        List<ComponentSystem> componentSystems = new List<ComponentSystem>();

        #endregion

        #region Methods

        public int CreateNewEntity(params Component[] components)
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

        public void DestroyEntity(int entity)
        {
            Component[] entityComponents = entityComponentTable[entity];

            foreach (ComponentSystem system in componentSystems)
            {
                Type[] systemComponents = system.RequiredComponents;

                bool allComponentsMatch = true;

                for (int i = 0; i < systemComponents.Length; i++)
                {
                    bool componentsMatch = false;

                    for (int j = 0; j < entityComponents.Length; j++)
                    {
                        if (systemComponents[i] == entityComponents[j].GetType())
                        {
                            componentsMatch = true;
                            break;
                        }
                    }

                    if (!componentsMatch)
                    {
                        allComponentsMatch = false;
                        break;
                    }
                }

                if (allComponentsMatch)
                {
                    system.RemoveEntity(entity, this);
                }
            }

            entityComponentTable[entity] = null;

            availableEntityIdPool.Add(entity);
        }

        public void SetRenderCamera(int cameraEntity)
        {
            if (cameraEntity == 0)
            {
                Console.WriteLine("You can not set the RenderCamera to Entity Zero using SetRenderCamera. If you wish to reset the RenderCamera, use ClearRenderCamera");
                return;
            }

            if (!DoesEntityContainComponents(cameraEntity, typeof(Camera), typeof(ScreenPosition)))
            {
                Console.WriteLine("Provided entity (ID: {0}) does not contain both a ScreenPosition and Camera component.", cameraEntity);
                return;
            }

            RenderCamera = cameraEntity;
        }

        public void ClearRenderCamera()
        {
            RenderCamera = 0;
        }

        public static void SetDefaultCamera(int camera)
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

        public T[] GetComponentsOfEntity<T>(int entity) where T : Component
        {
            ComponentBatch allComponents = entityComponentTable[entity];

            return allComponents.GetComponents<T>();
        }

        public ComponentBatch GetAllComponentsOfEntity(int entity)
        {
            return entityComponentTable[entity];
        }
        
        public void AddEntityToApplicableSystems(int entity)
        {
            foreach (ComponentSystem system in componentSystems)
            {
                if (DoesEntityContainComponents(entity, system.RequiredComponents))
                {
                    system.AddEntity(entity, this);
                }
            }
        }

        public bool DoesEntityContainComponents(int entity, params Type[] components)
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
            if (RenderCamera == 0)
            {
                return;
            }
        }

        
        private int GetAvailableEntityID(out bool usedIdFromPool)
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
