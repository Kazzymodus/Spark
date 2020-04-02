namespace SparkEngine.Systems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.States;

    public class DrawInfo
    {
        public GameState State { get; }
        public GraphicsDevice GraphicsDevice { get; }
        public SpriteBatch SpriteBatch { get; }
        public Matrix CameraTransform { get; }

        public DrawInfo(GameState state, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Matrix cameraTransform)
        {
            State = state;
            GraphicsDevice = graphicsDevice;
            SpriteBatch = spriteBatch;
            CameraTransform = cameraTransform;
        }
    }
}
