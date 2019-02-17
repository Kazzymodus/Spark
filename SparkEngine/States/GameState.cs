namespace SparkEngine.States
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Rendering;
    using SparkEngine.UI;

    public abstract class GameState
    {
        #region Constructors

        internal GameState(StateActivityLevel activityLevel, Camera camera = null)
        {
            if (camera == null)
            {
                Camera = DefaultCamera;
            }
            else
            {
                Camera = camera;
            }
        }

        #endregion

        #region Properties

        public bool UsesDefaultCamera
        {
            get
            {
                return Camera.Equals(DefaultCamera);
            }
        }

        public static Camera DefaultCamera { get; private set; }

        public StateActivityLevel ActivityLevel { get; set; }

        // public StateUI StateUI { get; protected set; }

        public Camera Camera { get; }

        #endregion

        #region Methods

        internal static void SetDefaultCamera(Camera camera)
        {
            DefaultCamera = camera;
        }

        protected internal virtual void ProcessInput(GameTime gameTime)
        {

        }

        protected internal virtual void Update(GameTime gameTime)
        {

        }

        protected internal virtual void DrawWorld(SpriteBatch spriteBatch)
        {

        }

        protected internal virtual void DrawScreen(SpriteBatch spriteBatch)
        {
            
        }

        #endregion
    }
}
