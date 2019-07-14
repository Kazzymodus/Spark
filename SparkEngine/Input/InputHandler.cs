namespace SparkEngine.Input
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    public class InputHandler
    {
        #region Fields

        private MouseState mouseState = Mouse.GetState();
        private MouseState oldMouseState;

        private KeyboardState keyboardState = Keyboard.GetState();
        private KeyboardState oldKeyboardState;

        #endregion

        #region Properties


        /// <summary>
        /// Gets the screen position of the mouse.
        /// </summary>
        public Point MouseScreenPosition
        {
            get { return Mouse.GetState().Position; }
        }


        /// <summary>
        /// Gets the screen position of the mouse.
        /// </summary>
        public Vector2 MouseScreenPositionVector
        {
            get { return Mouse.GetState().Position.ToVector2(); }
        }

        #endregion

        #region Methods

        public bool IsMouseDown(MouseButtons button)
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

        public bool IsMousePressed(MouseButtons button)
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

        public bool IsMouseReleased(MouseButtons button)
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

        public bool IsKeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        public bool IsKeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key) && oldKeyboardState.IsKeyUp(key);
        }

        public bool IsKeyReleased(Keys key)
        {
            return keyboardState.IsKeyUp(key) && oldKeyboardState.IsKeyDown(key);
        }

        internal void Update()
        {
            oldMouseState = mouseState;
            mouseState = Mouse.GetState();

            oldKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
        }

        #endregion
    }
}
