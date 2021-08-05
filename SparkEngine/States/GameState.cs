using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SparkEngine.Components;
using SparkEngine.Input;
using SparkEngine.Rendering;
using SparkEngine.Systems;

namespace SparkEngine.States
{
    /// <summary>
    ///     Describes a part of your game with its own entities.
    /// </summary>
    public class GameState
    {
        #region Constructors

        public GameState(string name, StateActivityLevel activityLevel = StateActivityLevel.Active,
            params ComponentSystem[] componentSystems)
        {
            Name = name;
            ActivityLevel = activityLevel;

            foreach (var system in componentSystems) RegisterComponentSystem(system);

            DrawLayers = new DrawLayerCollection
            {
                new DrawLayer()
            };
        }

        #endregion

        #region Fields

        public const int MaxEntities = 1000;

        private readonly List<int> availableEntityIdPool = new List<int>();

        private int nextEntityId = 1;

        #endregion

        #region Properties

        public string Name { get; }

        public StateActivityLevel ActivityLevel { get; set; }

        public SystemCollection ComponentSystems { get; } = new SystemCollection();

        public int RenderCamera { get; private set; }

        public DrawLayerCollection DrawLayers { get; }

        public bool HasRenderCamera => RenderCamera != 0;

        public Vector2? CameraPosition
        {
            get
            {
                if (!HasRenderCamera) return null;

                var renderCamera = GetComponentOfEntity<Camera>(RenderCamera);
                return new Vector2(renderCamera.PositionX, renderCamera.PositionY);
            }
        }

        #endregion

        #region Methods

        public int CreateNewEntity()
        {
            var entity = GetAvailableEntityID(out var usedIdFromPool);

            if (usedIdFromPool)
                availableEntityIdPool.Remove(entity);
            else
                nextEntityId++;

            return entity;
        }

        public void CreateComponentForEntity<T>(T template, int entity) where T : struct, IComponent
        {
            foreach (var system in ComponentSystems)
                if (system is ComponentSystem<T> validSystem)
                {
                    validSystem.AddNewComponentToEntity(template, entity, this);
                    return;
                }

            Console.WriteLine($"No system supporting type {typeof(T)} is registered.");
        }

        public int CreateNewEntityWithComponent<T>(T template) where T : struct, IComponent
        {
            var entity = CreateNewEntity();
            CreateComponentForEntity(template, entity);
            return entity;
        }

        public void DestroyEntity(int entity)
        {
            foreach (var system in ComponentSystems)
                if (system.HasComponentOfEntity(entity))
                {
                    //system.DestroyComponent(entity, this);
                }

            availableEntityIdPool.Add(entity);
        }

        public void RegisterComponentSystem(ComponentSystem system)
        {
            ComponentSystems.Add(system);
        }

        public T GetComponentOfEntity<T>(int entity) where T : struct, IComponent
        {
            foreach (var system in ComponentSystems)
                if (system is ComponentSystem<T> compSys)
                {
                    var comp = compSys.GetComponent(entity);

                    if (comp == null)
                        throw new ArgumentException(
                            $"Entity (ID: {entity}) does not contain a component of type {typeof(T)}");

                    return (T) comp;
                }

            throw new ArgumentException($"No registered system found that takes components of type {typeof(T)}.");
        }

        public void SetRenderCamera(int cameraEntity)
        {
            if (cameraEntity == 0)
            {
                Console.WriteLine(
                    "You can not set the RenderCamera to Entity Zero using SetRenderCamera. If you wish to reset the RenderCamera, use ClearRenderCamera");
                return;
            }

            foreach (var system in ComponentSystems)
                if (system is CameraSystem camSystem)
                {
                    if (camSystem.HasComponentOfEntity(cameraEntity))
                    {
                        RenderCamera = cameraEntity;
                        return;
                    }

                    Console.WriteLine("Provided entity (ID: {0}) does not have a Camera component.", cameraEntity);
                    return;
                }

            Console.WriteLine("There's no registered CameraSystem.");
        }

        public void ClearRenderCamera()
        {
            RenderCamera = 0;
        }

        public void ScheduleGlobalTask<T>(int task, int source, UpdateInfo updateInfo) where T : ComponentSystem
        {
            var system = (T) ComponentSystems[typeof(T)];
        }

        public void ScheduleTask<T>(int task, int source, int target, UpdateInfo updateInfo) where T : ComponentSystem
        {
            ((T) ComponentSystems[typeof(T)]).ScheduleTask(task, source, target, updateInfo);
        }

        internal void Update(GameTime gameTime, InputHandler input)
        {
            var updateInfo = new UpdateInfo(this, gameTime, input);

            foreach (var system in ComponentSystems) system.Update(updateInfo);
        }

        internal void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            if (RenderCamera == 0) return;

            var drawInfo = new DrawInfo(this, graphicsDevice, spriteBatch,
                GetComponentOfEntity<Camera>(RenderCamera).Transform);

            foreach (var system in ComponentSystems)
                if (system is IDrawSystem drawSystem)
                    drawSystem.Draw(drawInfo);
        }

        private int GetAvailableEntityID(out bool usedIdFromPool)
        {
            var newId = availableEntityIdPool.FirstOrDefault();
            usedIdFromPool = newId > 0;

            if (!usedIdFromPool) newId = nextEntityId;

            return newId;
        }

        #endregion
    }
}