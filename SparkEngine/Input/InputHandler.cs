namespace SparkEngine.Input
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    public static class InputHandler
    {
        #region Fields

        private static MouseState mouseState = Mouse.GetState();
        private static MouseState oldMouseState = mouseState;

        private static KeyboardState keyboardState = Keyboard.GetState();
        private static KeyboardState oldKeyboardState = keyboardState;

        #endregion

        #region Properties

        public static Point MousePosition
        {
            get { return Mouse.GetState().Position; }
        }

        #endregion

        #region Methods

        public static bool IsMouseDown(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.LMB:
                    return mouseState.LeftButton == ButtonState.Pressed;
                case MouseButtons.RMB:
                    return mouseState.RightButton == ButtonState.Pressed;
                case MouseButtons.MMB:
                    return mouseState.MiddleButton == ButtonState.Pressed;
                default:
                    return false;
            }
        }

        public static bool IsMousePressed(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.LMB:
                    return mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released;
                case MouseButtons.RMB:
                    return mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released;
                case MouseButtons.MMB:
                    return mouseState.MiddleButton == ButtonState.Pressed && oldMouseState.MiddleButton == ButtonState.Released;
                default:
                    return false;
            }
        }

        public static bool IsMouseReleased(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.LMB:
                    return mouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed;
                case MouseButtons.RMB:
                    return mouseState.RightButton == ButtonState.Released && oldMouseState.RightButton == ButtonState.Pressed;
                case MouseButtons.MMB:
                    return mouseState.MiddleButton == ButtonState.Released && oldMouseState.MiddleButton == ButtonState.Pressed;
                default:
                    return false;
            }
        }

        public static bool IsKeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        public static bool IsKeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key) && oldKeyboardState.IsKeyUp(key);
        }

        public static bool IsKeyReleased(Keys key)
        {
            return keyboardState.IsKeyUp(key) && oldKeyboardState.IsKeyDown(key);
        }

        public static void Update()
        {
            oldMouseState = mouseState;
            mouseState = Mouse.GetState();

            oldKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
        }

        #endregion
    }
}
