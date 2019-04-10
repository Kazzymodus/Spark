namespace SparkEngine.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Components;
    using SparkEngine.Input;
    using SparkEngine.Rendering;

    public class Button : Component, IDrawableComponent
    {
        #region Fields

        private bool isHeldDown;

        private MouseButtons validInput;

        private Texture2D texture;

        #endregion

        #region Constructors

        public Button(Texture2D texture, Vector2 position, MouseButtons validInput = MouseButtons.LMB)
        {
            Position = position;
            this.validInput = validInput;
            this.texture = texture;
            Size = texture.Bounds.Size;
        }

        #endregion

        #region Events

        public EventHandler OnClickEvent;

        #endregion

        #region Properties

        public Vector2 Position;

        public Point Size;

        public LayerSortMethod LayerSortMethod { get; } = LayerSortMethod.Last;

        #endregion

        #region Methods

        public override void ProcessInput(GameTime gameTime, out bool gotUsableInput)
        {
            Rectangle bounds = new Rectangle(Position.ToPoint(), Size);

            if (InputHandler.IsMousePressed(validInput))
            {
                gotUsableInput = true;

                if (bounds.Contains(InputHandler.MousePosition))
                {
                    isHeldDown = true;
                }

                return;
            }
            else if (InputHandler.IsMouseReleased(validInput))
            {
                gotUsableInput = true;

                if (isHeldDown)
                {
                    if (bounds.Contains(InputHandler.MousePosition))
                    {
                        OnClickEvent?.Invoke(this, new EventArgs());
                    }

                    isHeldDown = false;
                }

                return;
            }

            gotUsableInput = false;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public Vector2 GetDrawPosition(Camera camera, DrawLayer drawLayer)
        {
            return drawLayer.Position + Position;
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera, DrawLayer drawLayer)
        {

            Color color = isHeldDown ? Color.Gray : Color.White; //temp
            spriteBatch.Draw(texture, GetDrawPosition(camera, drawLayer), color);
        }

        #endregion
    }
}
