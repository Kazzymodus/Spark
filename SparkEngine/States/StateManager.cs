using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SparkEngine.Input;

namespace SparkEngine.States
{
    public class StateManager
    {
        #region Fields

        private readonly Stack<GameState> states = new Stack<GameState>();
        private readonly List<GameState> pushRequests = new List<GameState>();

        private readonly InputHandler inputHandler = new InputHandler();

        #endregion

        #region Properties

        #endregion

        #region Methods

        public void RequestStatePush(GameState state)
        {
            pushRequests.Add(state);
        }

        public void UpdateStates(GameTime gameTime)
        {
            inputHandler.Update();

            var reversedStates = states.Reverse();

            foreach (var state in reversedStates)
            {
                var activityLevel = state.ActivityLevel;

                if (activityLevel != StateActivityLevel.Paused && activityLevel != StateActivityLevel.Inactive)
                    state.Update(gameTime, inputHandler);
            }

            if (pushRequests.Count > 0) PushNewRequests();
        }

        public void DrawStates(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            var reversedStates = states.Reverse();

            foreach (var state in reversedStates)
            {
                var activityLevel = state.ActivityLevel;

                if (activityLevel != StateActivityLevel.Hidden && activityLevel != StateActivityLevel.Inactive)
                    state.Draw(graphicsDevice, spriteBatch);
            }
        }

        private void PushNewRequests()
        {
            foreach (var state in pushRequests) states.Push(state);

            pushRequests.Clear();
        }

        #endregion
    }
}