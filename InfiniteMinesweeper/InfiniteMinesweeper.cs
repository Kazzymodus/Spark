using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SparkEngine.Assets;
using SparkEngine.Components;
using SparkEngine.Debug;
using SparkEngine.Input;
using SparkEngine.Rendering;
using SparkEngine.States;
using SparkEngine.Systems;
using SparkEngine.Systems.Batching;
using InfiniteMinesweeper.Components;
using InfiniteMinesweeper.Systems;

namespace InfiniteMinesweeper
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class InfiniteMinesweeper : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private static bool quitFlag;

        private StateManager stateManager = new StateManager();

        private AssetDictionary<Texture2D> textures;
        private AssetDictionary<SpriteFont> fonts;
        private AssetDictionary<Texture2D> uiTextures;
        
        GameState level;
        
        public InfiniteMinesweeper()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 960,
                PreferredBackBufferHeight = 480,
            };

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Calling this method will quit the application at the end of the current Update call.
        /// </summary>
        public static void QuitApplication()
        {
            quitFlag = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            textures = new AssetDictionary<Texture2D>("Textures", Content);
            textures.TryAddAsset("Cell");

            fonts = new AssetDictionary<SpriteFont>("Fonts", Content);
            fonts.TryAddAsset("CourierNew");

            // TODO: use this.Content to load your game content here

            InitialiseContentDependents();
            BuildStates();
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
                stateManager.UpdateStates(gameTime);
            }

            // TODO: Add your update logic here

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
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            stateManager.DrawStates(graphics.GraphicsDevice, spriteBatch);

            base.Draw(gameTime);
        }

        private void InitialiseContentDependents()
        {
            Log.SetMessageFont(fonts.GetAsset("CourierNew"));
        }

        private void BuildStates()
        {
            int width = 17;
            int height = 17;
            float mineDensity = 0.2f;

            Point tileSize = new Point(16);

            SpriteDrawSystem spriteDraw = new SpriteDrawSystem();
            MineScreenSystem spriteGridSys = new MineScreenSystem();
            MineFieldSystem mineSystem = new MineFieldSystem();
            MineCameraSystem cameraSystem = new MineCameraSystem();
            level = new GameState("Level", StateActivityLevel.Active, cameraSystem, spriteDraw, mineSystem, spriteGridSys);

            int camera = level.CreateNewEntity();
            cameraSystem.AddNewComponentToEntity(new Camera(CameraConstraints.WrapAround, new Rectangle(0, 0, tileSize.X * width, tileSize.Y * height)), camera, level);
            level.SetRenderCamera(camera);

            // level.CreateNewEntityWithComponent(Sprite.CreateIsometricSprite(textures.GetAsset("Cell"), new Vector2(16, 16), new Vector2(16, 16)));

            stateManager.RequestStatePush(level);

            Sprite mineFieldTile = Sprite.CreateTileSprite(textures.GetAsset("Cell"), 13);
            mineFieldTile.FrameX = 12;
            //ArrayGrid grid = new ArrayGrid(Perspective.Standard, mineSystem.CreateMinefield(width, height, mineDensity, minefieldTile), new Vector2(16, 16), true, true);

            int mineField = level.CreateNewEntityWithComponent(new MineField(Vector2.Zero, tileSize, width, height, true, mineDensity));

            WrappingScreenGrid spriteGrid = spriteGridSys.CreateScreenFillingGrid(textures.GetAsset("Cell"), graphics.GraphicsDevice.Viewport.Bounds.Size, tileSize.ToVector2(), Perspective.Standard);
            spriteGrid.SetWrappedDimensions(width, height);
            int screenGrid = level.CreateNewEntityWithComponent(spriteGrid);

            //level.CreateComponentForEntity(mineFie), mineField);
            //int sprite = level.CreateNewEntityWithComponent(minefieldTile);
        }
    }
}
