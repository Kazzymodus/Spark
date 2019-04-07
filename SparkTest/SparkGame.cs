namespace SparkTest
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using SparkEngine.Debug;
    using SparkEngine.Assets;
    using SparkEngine.Input;
    using SparkEngine.States;
    using SparkEngine.Components;
    using SparkEngine.Rendering;
    using SparkTest.Components;

    public class SparkGame : Game
    {
        #region Fields

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private static bool quitFlag;

        private StateManager stateManager = new StateManager();

        private AssetDictionary<Texture2D> textures;
        private AssetDictionary<SpriteFont> fonts;

        #endregion

        #region Constructors

        public SparkGame()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                // Certain resolutions may invalidate cursor calculations.
                // 1152x648 is the highest windowed resolution that fits on my screen.

                PreferredBackBufferWidth = 1152,
                PreferredBackBufferHeight = 648,
                //IsFullScreen = true                
            };

            Content.RootDirectory = "Content";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calling this method will quit the application at the end of the current Update call.
        /// </summary>
        public static void QuitApplication()
        {
            quitFlag = true;
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            textures = new AssetDictionary<Texture2D>("Textures", Content);
            textures.TryAddAsset("Obelisk");
            textures.TryAddAsset("House");
            textures.TryAddAsset("GrassTile");
            textures.TryAddAsset("GrassTopDown");
            textures.TryAddAsset("GridTile");

            fonts = new AssetDictionary<SpriteFont>("Fonts", Content);
            fonts.TryAddAsset("CourierNew");

            InitialiseContentDependents();

            BuildStates();

            // StateManager.Initialise(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Temp addition to quickly quit while testing.

            if (InputHandler.IsKeyPressed(Keys.Escape))
            {
                quitFlag = true;
            }

            if (IsActive)
            {
                InputHandler.Update();
                stateManager.UpdateStates(gameTime);
            }

            base.Update(gameTime);

            if (quitFlag)
            {
                Exit();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (IsActive)
            {
                GraphicsDevice.Clear(Color.TransparentBlack);

                // TODO: Add your drawing code here

                stateManager.DrawStates(spriteBatch);

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

                Component.DrawMessages(spriteBatch); // Very much temp, gonna combine this into a manager some day.
                Camera.DrawMessages(spriteBatch);

                spriteBatch.End();

                // StateManager.DrawScreenStates(spriteBatch);
            }

            base.Draw(gameTime);
        }

        private void InitialiseContentDependents()
        {
            Log.SetMessageFont(fonts.GetAsset("CourierNew"));
            GameState.SetDefaultCamera(new Camera(graphics));
        }

        private void BuildStates()
        {
            GameState menu = new GameState("Menu");
            stateManager.RequestStatePush(menu);

            DrawLayer terrainLayer = menu.CreateNewDrawLayer("Terrain", false, new Vector2(64, 32));
            DrawLayer structureLayer = menu.CreateNewDrawLayer("Structures", false, new Vector2(64, 32));

            Terrain terrain = new Terrain(new Vector2(100), textures.GetAsset("GrassTile"), textures.GetAsset("GridTile"));
            menu.CreateNewEntity("Terrain", terrain);

            System.Random rand = new System.Random();

            for (int i = 0; i < 500; i++)
            {
                Vector2 pos = new Vector2(rand.Next(100), rand.Next(100));
                GridObject gridObject = GridObject.CreateIsometricGridObject(textures.GetAsset("Obelisk"), structureLayer, pos, new Vector2(2), 0);
                menu.CreateNewEntity("Structures", gridObject);

                pos = new Vector2(rand.Next(100), rand.Next(100));
                gridObject = GridObject.CreateIsometricGridObject(textures.GetAsset("House"), structureLayer, pos, new Vector2(1), 0);
                menu.CreateNewEntity("Structures", gridObject);
            }
            CameraController cameraController = new CameraController(menu.Camera);
            menu.CreateNewEntity(cameraController);
        }

        #endregion
    }
}
