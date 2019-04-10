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
            bool isMouseDown = false;

            if (button.HasFlag(MouseButtons.LMB))
            {
                isMouseDown |= mouseState.LeftButton == ButtonState.Pressed;
            }
            if (button.HasFlag(MouseButtons.LMB))
            {
                isMouseDown |= mouseState.RightButton == ButtonState.Pressed;
            }
            if (button.HasFlag(MouseButtons.LMB))
            {
                isMouseDown |= mouseState.MiddleButton == ButtonState.Pressed;
            }

            return isMouseDown;
        }

        public static bool IsMousePressed(MouseButtons button)
        {
            bool isMousePressed = false;

            if (button.HasFlag(MouseButtons.LMB))
            {
                isMousePressed |= mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released;
            }
            if (button.HasFlag(MouseButtons.RMB))
            {
                isMousePressed |= mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released;
            }
            if (button.HasFlag(MouseButtons.MMB))
            {
                isMousePressed |= mouseState.MiddleButton == ButtonState.Pressed && oldMouseState.MiddleButton == ButtonState.Released;
            }

            return isMousePressed;
        }

        public static bool IsMouseReleased(MouseButtons button)
        {
            bool isMouseReleased = false;

            if (button.HasFlag(MouseButtons.LMB))
            {
                isMouseReleased |= mouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed;
            }
            if (button.HasFlag(MouseButtons.RMB))
            {
                isMouseReleased |= mouseState.RightButton == ButtonState.Released && oldMouseState.RightButton == ButtonState.Pressed;
            }
            if (button.HasFlag(MouseButtons.MMB))
            {
                isMouseReleased |= mouseState.MiddleButton == ButtonState.Released && oldMouseState.MiddleButton == ButtonState.Pressed;
            }

            return isMouseReleased;
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
