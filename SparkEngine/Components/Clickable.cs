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
    using SparkEngine.States;

    public class Clickable : Component
    {
        #region Fields

        private bool isHeldDown;

        private MouseButtons validInput;

        #endregion

        #region Constructors

        public Clickable(Texture2D texture, Vector2 position, MouseButtons validInput = MouseButtons.LMB)
        {
            Position = position;
            this.validInput = validInput;
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

        public override void ProcessInput(InputHandler input, GameState state, GameTime gameTime, bool underCursor)
        {
            if (input.IsMousePressed(validInput))
            {
                if (underCursor)
                {
                    isHeldDown = true;
                }

                return;
            }
            else if (input.IsMouseReleased(validInput))
            {
                if (isHeldDown)
                {
                    if (underCursor)
                    {
                        OnClickEvent?.Invoke(this, new EventArgs());
                    }

                    isHeldDown = false;
                }

                return;
            }

            SkipInputProcessing = true;
        }

        public override void Update(GameState state, GameTime gameTime)
        {
        }

        public Vector2 GetDrawPosition(Camera camera, DrawLayer drawLayer)
        {
            return drawLayer.Position + Position;
        }

        public Rectangle GetBounds(Camera camera, DrawLayer drawLayer)
        {
            return new Rectangle(Position.ToPoint(), Size);
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera, DrawLayer drawLayer)
        {

            Color color = isHeldDown ? Color.Gray : Color.White; //temp
            spriteBatch.Draw(texture, GetDrawPosition(camera, drawLayer), color);
        }

        #endregion
    }
}
