namespace Obelisk
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using SparkEngine.Debug;
    using SparkEngine.Input;
    using SparkEngine.States;

    public class ObeliskGame : Game
    {
        #region Fields

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private static bool quitFlag;

        #endregion

        #region Constructors

        public ObeliskGame()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                // Certain resolutions may invalidate cursor calculations.
                // 1152x648 is the highest windowed resolution that fits on my screen.

                PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width,
                PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height,
                IsFullScreen = true                
            };

            Content.RootDirectory = "Content";
        }

        #endregion

        #region Methods

        protected override void Initialize()
        {
            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

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
            if (IsActive)
            {
                InputHandler.Update();
                // StateManager.UpdateStates(gameTime);
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

                // StateManager.DrawWorldStates(spriteBatch);
                // StateManager.DrawScreenStates(spriteBatch);
            }

            base.Draw(gameTime);
        }

        /// <summary>
        /// Calling this method will quit the application at the end of the current Update call.
        /// </summary>
        public static void QuitApplication()
        {
            quitFlag = true;
        }

        #endregion
    }
}
