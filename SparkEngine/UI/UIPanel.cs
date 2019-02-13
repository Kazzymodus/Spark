namespace SparkEngine.UI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class UIPanel
    {
        #region Fields

        private Texture2D backDrop;
        private List<UIElement> uiElements = new List<UIElement>();
        private Pivot pivot;

        #endregion

        #region Constructors

        public UIPanel(Texture2D backDrop)
        {
            this.backDrop = backDrop;
            pivot = Pivot.UpperLeft;
        }

        public UIPanel(Texture2D backDrop, Pivot pivot, Vector2 viewportSize)
        {
            this.backDrop = backDrop;
            this.pivot = pivot;

            Position = GetPivotPosition(viewportSize);
        }

        #endregion

        #region Properties

        public Vector2 Position { get; private set; }

        #endregion

        #region Methods

        public void AddElements(params UIElement[] elements)
        {
            foreach (UIElement element in elements)
            {
                uiElements.Add(element);
            }
        }

        public void AddElements(List<UIElement> elements)
        {
            AddElements(elements.ToArray());
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backDrop, Position, Color.White);

            foreach (UIElement element in uiElements)
            {
                element.Draw(spriteBatch);
            }
        }

        private Vector2 GetPivotPosition(Vector2 viewportSize)
        {
            float x = ((int)pivot % 3) * 0.5f;
            float y = ((int)pivot / 3) * 0.5f;

            Vector2 factor = new Vector2(x, y);

            return (viewportSize * factor) - (backDrop.Bounds.Size.ToVector2() * factor);
        }

        #endregion
    }
}
