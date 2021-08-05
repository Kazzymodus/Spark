using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SparkEngine.States;

namespace SparkEngine.Systems
{
    public class DrawInfo
    {
        public DrawInfo(GameState state, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Matrix cameraTransform)
        {
            State = state;
            GraphicsDevice = graphicsDevice;
            SpriteBatch = spriteBatch;
            CameraTransform = cameraTransform;
        }

        public GameState State { get; }
        public GraphicsDevice GraphicsDevice { get; }
        public SpriteBatch SpriteBatch { get; }
        public Matrix CameraTransform { get; }
    }
}