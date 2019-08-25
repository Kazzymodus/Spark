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
        #region Constructors

        public Clickable(Texture2D texture, Vector2 position, MouseButtons validInput = MouseButtons.LMB)
        {
            Position = position;
            ValidInput = validInput;
            Size = texture.Bounds.Size;
        }

        #endregion

        #region Events

        public EventHandler OnClickEvent;

        #endregion

        #region Properties

        public Vector2 Position;

        public Point Size;

        public bool IsHeldDown { get; set; }

        public MouseButtons ValidInput { get; set; }

        #endregion
    }
}
