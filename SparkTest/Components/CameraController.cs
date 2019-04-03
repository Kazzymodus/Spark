﻿namespace SparkTest.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using SparkEngine.Components;
    using SparkEngine.Rendering;
    using SparkEngine.Input;

    class CameraController : Component
    {

        public CameraController(Camera camera)
        {
            Camera = camera;
        }

        public Camera Camera { get; private set; }

        public override void ProcessInput(GameTime gameTime)
        {
            if (InputHandler.IsKeyDown(Keys.Down))
            {
                Camera.MoveCamera(new Vector2(0, 10));
            }
            if (InputHandler.IsKeyDown(Keys.Up))
            {
                Camera.MoveCamera(new Vector2(0, -10));
            }
            if (InputHandler.IsKeyDown(Keys.Right))
            {
                Camera.MoveCamera(new Vector2(10, 0));
            }
            if (InputHandler.IsKeyDown(Keys.Left))
            {
                Camera.MoveCamera(new Vector2(-10, 0));
            }
        }

    }
}
