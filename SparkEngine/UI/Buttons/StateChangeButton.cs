namespace SparkEngine.UI.Buttons
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using SparkEngine.States;

    public class StateChangeButton : Button
    {
        #region Fields

        private GameState state;

        #endregion

        #region Constructors

        public StateChangeButton(Vector2 position, GameState state, string tooltipText = "", UIPanel parent = null)
            : base(position, tooltipText, parent)
        {
            this.state = state;
        }

        #endregion

        #region Methods

        public override void ExecuteClick()
        {
            //if (StateManager.GetState(state.ID) == null)
            //{
            //    StateManager.RequestStatePush(state);
            //}
            //else
            //{
            //    state.ToggleActive();
            //}
        }

        #endregion
    }
}
