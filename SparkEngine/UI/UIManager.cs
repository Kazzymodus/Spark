namespace SparkEngine.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using SparkEngine.Input;

    internal static class UIManager
    {
        #region Fields

        private static List<IHoverable> hoverables = new List<IHoverable>();
        private static List<IClickable> clickables = new List<IClickable>();

        private static IHoverable activeHoverable;

        #endregion

        #region Methods

        public static void RegisterHoverable(IHoverable hoverable)
        {
            hoverables.Add(hoverable);
        }

        public static void RegisterClickable(IClickable clickable)
        {
            clickables.Add(clickable);
        }

        internal static void ExecuteHover(Point mousePosition)
        {
            if (activeHoverable != null)
            {
                if (activeHoverable.Bounds.Contains(mousePosition))
                {
                    activeHoverable.ExecuteHover();
                    return;
                }
                else
                {
                    activeHoverable.ClearHover();
                    activeHoverable = null;
                }
            }

            foreach (IHoverable hoverable in hoverables)
            {
                if (hoverable.Bounds.Contains(mousePosition))
                {
                    activeHoverable = hoverable;
                    activeHoverable.ExecuteHover();
                    return;
                }
            }
        }

        internal static void ExecuteClick(Point mousePosition, out bool clickedAny)
        {
            clickedAny = false;

            foreach (IClickable clickable in clickables)
            {
                if (clickable.Bounds.Contains(mousePosition))
                {
                    clickable.ExecuteClick();
                    clickedAny = true;
                    return;
                }
            }
        }

        #endregion
    }
}
