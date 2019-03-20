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

    public class SparkGame : Game
    {
        #region Fields

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private static bool quitFlag;

        private StateManager stateManager = new StateManager();

        private AssetDictionary<Texture2D> textures;

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
            textures.TryAddAsset("GrassTile");

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
                // StateManager.DrawScreenStates(spriteBatch);
            }

            base.Draw(gameTime);
        }

        private void BuildStates()
        {
            GameState.SetDefaultCamera(new Camera(graphics));

            GameState menu = new GameState("Menu");
            stateManager.RequestStatePush(menu);

            Terrain terrain = new Terrain(new Vector2(50), textures.GetAsset("GrassTile"));
            menu.CreateNewEntity(terrain);

            GridObject gridObject = new GridObject(textures.GetAsset("Obelisk"), Vector2.Zero, new Vector2(2));
            menu.CreateNewEntity(gridObject);
        }

        #endregion
    }
}
