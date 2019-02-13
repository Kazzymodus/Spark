namespace SparkEngine.States.Primary
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using SparkEngine.DataStructures;
    using SparkEngine.Debug;
    using SparkEngine.Dictionaries;
    using SparkEngine.IDs;
    using SparkEngine.Input;
    using SparkEngine.Level;
    using SparkEngine.Player;
    using SparkEngine.Rendering;
    using SparkEngine.Pathfinding;
    using SparkEngine.States.Secondary;
    using SparkEngine.Structures;
    using SparkEngine.Tasks;
    using SparkEngine.Time;
    using SparkEngine.UI;
    using SparkEngine.UI.Buttons;
    using SparkEngine.World;

    public class LevelState : GameState
    {
        #region Fields

        private Level level;

        //private Map currentMap;
        //private Player player;
        private GameSettings settings;

        // private BuildState buildState;

        #endregion

        #region Constructors

        public LevelState(bool isActive)
            : base(isActive)
        {
            settings = new GameSettings(256, 50);
            RenderHelper.Initialise(settings);

            level = new Level(settings.TerrainSize, Camera);

            StateUI = new StateUI();
        }

        #endregion

        #region Properties

        public override int ID
        {
            get { return StateIDs.LevelState; }
        }

        public override bool CanHaveMultiples
        {
            get { return false; }
        }

        #endregion

        #region Methods

        protected internal override void Update(GameTime gameTime)
        {
            level.Update(gameTime);

            base.Update(gameTime);
        }

        protected internal override void DrawWorld(SpriteBatch spriteBatch)
        {
            level.DrawMap(spriteBatch, Camera, settings.DrawGrid);
        }

        protected internal override void DrawScreen(SpriteBatch spriteBatch)
        {            
            if (settings.DrawUI)
            {
                base.DrawScreen(spriteBatch);
            }
        }

        protected internal override void ProcessInput(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Point mouseScreenPosition = InputHandler.MousePosition;
            UIManager.ExecuteHover(mouseScreenPosition);

            if (InputHandler.IsMousePressed(MouseButtons.LMB))
            {
                UIManager.ExecuteClick(mouseScreenPosition, out bool clickedAny);

                if (!clickedAny)
                {
                    Vector2 mouseWorldPosition = Camera.MouseWorldPosition;
                    WorldObject cursorObject = level.Map.ObjectManager.GetCursorObject(mouseWorldPosition.ToPoint(), level.Map);
                }
            }

            // Camera Translation

            if (InputHandler.IsKeyDown(Keys.D))
            {
                Camera.MoveCamera(settings.CameraSpeed * deltaTime, 0);
            }

            if (InputHandler.IsKeyDown(Keys.A))
            {
                Camera.MoveCamera(-settings.CameraSpeed * deltaTime, 0);
            }

            if (InputHandler.IsKeyDown(Keys.S))
            {
                Camera.MoveCamera(0, settings.CameraSpeed * deltaTime);
            }

            if (InputHandler.IsKeyDown(Keys.W))
            {
                Camera.MoveCamera(0, -settings.CameraSpeed * deltaTime);
            }

            // Camera Rotation

            if (InputHandler.IsKeyPressed(Keys.E))
            {
                level.RotateMap(1, Camera);
            }

            if (InputHandler.IsKeyPressed(Keys.Q))
            {
                level.RotateMap(-1, Camera);
            }

            // Misc

            if (InputHandler.IsKeyPressed(Keys.OemTilde))
            {
                StateManager.DebugState.ToggleActive();
            }

            if (InputHandler.IsKeyPressed(Keys.G))
            {
                settings.ToggleGrid();
            }

            if (InputHandler.IsKeyPressed(Keys.H))
            {
                settings.ToggleUI();
            }

            //if (InputHandler.IsMousePressed(MouseButtons.LMB))
            //{
            //    //UIManager.ExecuteClick(InputHandler.MousePosition);
                
            //    Vector2 coordinates = StateManager.MainCamera.GetCursorTile();
                
            //    if (CurrentMap.Terrain.IsValidPlacement(coordinates, Vector2.One))
            //    {
            //        CurrentMap.SpawnStructure(new Generator(3, coordinates));
            //    }
            //}

            //if (InputHandler.IsMousePressed(MouseButtons.RMB))
            //{
            //    Vector2 coordinates = StateManager.MainCamera.GetCursorTile();

            //    coordinates = RenderHelper.RotateCoordsInSquare(coordinates, 2, StateManager.MainCamera.Rotations);

            //    if (CurrentMap.Terrain.IsValidPlacement(coordinates, new Vector2(2, 2)))
            //    {
            //        CurrentMap.SpawnStructure(new Generator(1, coordinates));
            //    }
            //}

            if (InputHandler.IsKeyPressed(Keys.V))
            {
                Vector2 coordinates = level.Map.GetCursorTile(Camera);
                DrawData drawData = EntityDictionary.GetEntity(EntityIDs.Subject);

                if (level.Map.Terrain.IsValidPlacement(coordinates, drawData.Dimensions))
                {
                    level.Map.SpawnSubject(coordinates, drawData);
                }
            }

            if (InputHandler.IsKeyPressed(Keys.P))
            {
                Vector2 coordinates = level.Map.GetCursorTile(Camera);
                Generator temple = new Generator(StructureIDs.Temple, coordinates);

                if (level.Map.Terrain.IsValidPlacement(coordinates, temple.DrawData.Dimensions))
                {
                    level.Map.CreateTask(new BuildTask(temple));
                }
            }

            if (InputHandler.IsKeyPressed(Keys.Escape))
            {
                ObeliskGame.SetExitFlag();
            }

            //if (InputHandler.IsKeyPressed(Keys.J))
            //{
            //    PathFinderTest.FindPathSingleStep(currentMap.Terrain);
            //}

            //if (InputHandler.IsKeyPressed(Keys.K))
            //{
            //    PathFinderTest.Reset();
            //    PathFinderTest.FindPath(currentMap.Terrain, new Vector2(0, 0), new Vector2(49, 49));
            //}
            
            //if (InputHandler.IsKeyPressed(Keys.Space))
            //{
            //    PathFinderTest.Reset();
            //}
        }

        //private UIElement[] CreateResourceWindows()
        //{
        //    UIElement[] windows = new UIElement[ResourceIDs.Count];

        //    for (int i = 0; i < windows.Length; i++)
        //    {
        //        Vector2 position = new Vector2(i * ResourceWindow.Width, 0);
        //        ResourceWindow window = new ResourceWindow(position, player.GetResource(i), i);
        //        windows[i] = window;
        //    }

        //    return windows;
        //}
        #endregion
    }
}
