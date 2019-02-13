namespace SparkEngine.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.IDs;
    using SparkEngine.Player;

    public class StateUI
    {
        #region Fields

        private UIPanel[] panels;

        #endregion

        #region Constructors

        public StateUI(params UIPanel[] panels)
        {
            this.panels = panels;
        }

        #endregion

        #region Methods

        internal void Draw(SpriteBatch spriteBatch)
        {
            foreach (UIPanel panel in panels)
            {
                panel.Draw(spriteBatch);
            }
        }

        #endregion
    }
}
