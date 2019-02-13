namespace Obelisk
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using SparkEngine.Debug;
    using SparkEngine.Dictionaries;
    using SparkEngine.Input;
    using SparkEngine.States;
    using SparkEngine.States.Primary;

    public class ObeliskGame : Game
    {
        #region Fields

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private static bool exitGame;

        #endregion

        #region Constructors

        public ObeliskGame()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                // Certain resolutions may invalidate cursor calculations.
                // 1152x648 is the highest windowed resolution that fits on my screen.

                PreferredBackBufferWidth = 1152,
                PreferredBackBufferHeight = 648,
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

            TextureDictionary.LoadTextures(Content);
            FontDictionary.LoadFonts(Content);
            EffectDictionary.LoadEffects(Content);
            
            TileDictionary.LoadTiles();
            StructureDictionary.LoadStructures(Content);
            EntityDictionary.LoadEntities();

            StateManager.Initialise(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
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
                StateManager.UpdateStates(gameTime);
            }

            base.Update(gameTime);

            if (exitGame)
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

                StateManager.DrawWorldStates(spriteBatch);
                StateManager.DrawScreenStates(spriteBatch);
            }

            base.Draw(gameTime);
        }

        /// <summary>
        /// Calling this method will exit the game at the end of the current Update call.
        /// </summary>
        public static void SetExitFlag()
        {
            exitGame = true;
        }

        #endregion
    }
}
