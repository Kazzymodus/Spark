using InfiniteMinesweeper.Components;
using InfiniteMinesweeper.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SparkEngine.Assets;
using SparkEngine.Components;
using SparkEngine.Debug;
using SparkEngine.States;
using SparkEngine.Systems;

namespace InfiniteMinesweeper
{
    /// <summary>
    ///     This is the main type for your game.
    /// </summary>
    public class InfiniteMinesweeper : Game
    {
        private static bool quitFlag;
        private readonly GraphicsDeviceManager graphics;

        private readonly StateManager stateManager = new StateManager();
        private AssetDictionary<SpriteFont> fonts;

        private GameState level;
        private SpriteBatch spriteBatch;

        private AssetDictionary<Texture2D> textures;
        private AssetDictionary<Texture2D> uiTextures;

        public InfiniteMinesweeper()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1600,
                PreferredBackBufferHeight = 1280
            };

            Content.RootDirectory = "Content";
        }

        /// <summary>
        ///     Calling this method will quit the application at the end of the current Update call.
        /// </summary>
        public static void QuitApplication()
        {
            quitFlag = true;
        }

        /// <summary>
        ///     Allows the game to perform any initialization it needs to before starting to run.
        ///     This is where it can query for any required services and load any non-graphic
        ///     related content.  Calling base.Initialize will enumerate through any components
        ///     and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        ///     LoadContent will be called once per game and is the place to load
        ///     all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            textures = new AssetDictionary<Texture2D>("Textures", Content);
            textures.TryAddAsset("Cell32");

            fonts = new AssetDictionary<SpriteFont>("Fonts", Content);
            fonts.TryAddAsset("CourierNew");

            // TODO: use this.Content to load your game content here

            InitialiseContentDependents();
            BuildStates();
        }

        /// <summary>
        ///     UnloadContent will be called once per game and is the place to unload
        ///     game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        ///     checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (IsActive) stateManager.UpdateStates(gameTime);

            // TODO: Add your update logic here

            base.Update(gameTime);

            if (quitFlag) Exit();
        }

        /// <summary>
        ///     This is called when the game should draw itself.
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
            const int width = 17;
            const int height = 17;
            const float mineDensity = 0.2f;

            var tileSize = new Point(32);

            var spriteDraw = new SpriteDrawSystem();
            var spriteGridSys = new MineScreenSystem();
            var mineSystem = new MineFieldSystem();
            var cameraSystem = new MineCameraSystem();
            level = new GameState("Level", StateActivityLevel.Active, cameraSystem, spriteDraw, mineSystem,
                spriteGridSys);

            var camera = level.CreateNewEntity();
            cameraSystem.AddNewComponentToEntity(
                new Camera(CameraConstraints.WrapAround, new Rectangle(0, 0, tileSize.X * width, tileSize.Y * height)),
                camera, level);
            level.SetRenderCamera(camera);

            // level.CreateNewEntityWithComponent(Sprite.CreateIsometricSprite(textures.GetAsset("Cell"), new Vector2(16, 16), new Vector2(16, 16)));

            stateManager.RequestStatePush(level);

            var mineFieldTile = Sprite.CreateTileSprite(textures.GetAsset("Cell32"), 13);
            mineFieldTile.FrameX = 12;
            //ArrayGrid grid = new ArrayGrid(Perspective.Standard, mineSystem.CreateMinefield(width, height, mineDensity, minefieldTile), new Vector2(16, 16), true, true);

            var mineField =
                level.CreateNewEntityWithComponent(new MineField(Vector2.Zero, tileSize, width, height, true,
                    mineDensity));

            var spriteGrid = WrappingScreenGridSystem.CreateScreenFillingGrid(textures.GetAsset("Cell32"),
                graphics.GraphicsDevice.Viewport.Bounds.Size, tileSize.ToVector2(), Perspective.Standard);
            spriteGrid.SetWrappedDimensions(width, height);
            var screenGrid = level.CreateNewEntityWithComponent(spriteGrid);

            //level.CreateComponentForEntity(mineFie), mineField);
            //int sprite = level.CreateNewEntityWithComponent(minefieldTile);
        }
    }
}