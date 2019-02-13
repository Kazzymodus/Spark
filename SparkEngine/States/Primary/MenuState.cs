namespace SparkEngine.States.Primary
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using SparkEngine.Dictionaries;
    using SparkEngine.IDs;
    using SparkEngine.Input;
    using SparkEngine.Rendering;
    using SparkEngine.UI;

    public class MenuState : GameState
    {
        #region Constructors

        public MenuState(bool isActive)
            : base(isActive)
        {
            StateUI = new StateUI();
        }

        #endregion

        #region Properties

        public override int ID
        {
            get { return StateIDs.MenuState; }
        }

        public override bool CanHaveMultiples
        {
            get { return false; }
        }

        #endregion

        #region Methods

        protected internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected internal override void DrawScreen(SpriteBatch spriteBatch)
        {
            string message = "Press Space to start.\n\nLMB = Place building\nV = Place subject\nP = Place task\n\n~ = Toggle Debug";

            spriteBatch.DrawString(FontDictionary.GetFont(FontIDs.CourierNew), message, new Vector2(100, 100), Color.White);

            base.DrawScreen(spriteBatch);
        }

        protected internal override void ProcessInput(GameTime gameTime)
        {
            if (InputHandler.IsKeyPressed(Keys.Space))
            {
                StartNewGame();
            }

            if (InputHandler.IsKeyPressed(Keys.Escape))
            {
                ObeliskGame.SetExitFlag();
            }
        }

        private void StartNewGame()
        {
            StateManager.LevelState.ToggleActive();
            ToggleActive();
        }

        #endregion
    }
}
