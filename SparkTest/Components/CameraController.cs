using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SparkEngine.Components;
using SparkEngine.Input;
using SparkEngine.States;

namespace SparkTest.Components
{
    internal class CameraController : IComponent
    {
        public CameraController(Camera camera)
        {
            Camera = camera;
        }

        public Camera Camera { get; }

        public override void ProcessInput(GameState state, GameTime gameTime)
        {
            var inputReceived = false;

            if (inputReceived |= InputHandler.IsKeyDown(Keys.Down)) Camera.MoveCamera(new Vector2(0, 10));
            if (inputReceived |= InputHandler.IsKeyDown(Keys.Up)) Camera.MoveCamera(new Vector2(0, -10));
            if (inputReceived |= InputHandler.IsKeyDown(Keys.Right)) Camera.MoveCamera(new Vector2(10, 0));
            if (inputReceived |= InputHandler.IsKeyDown(Keys.Left)) Camera.MoveCamera(new Vector2(-10, 0));

            SkipInputProcessing = !inputReceived;
        }
    }
}