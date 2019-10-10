namespace SparkEngine.States
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Rendering;
    using SparkEngine.Debug;
    using SparkEngine.Input;

    public class StateManager
    {
        #region Fields

        private Stack<GameState> states = new Stack<GameState>();
        private List<GameState> pushRequests = new List<GameState>();

        private InputHandler inputHandler = new InputHandler();

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

            IEnumerable<GameState> reversedStates = states.Reverse();

            foreach(GameState state in reversedStates)
            {
                StateActivityLevel activityLevel = state.ActivityLevel;

                if (activityLevel != StateActivityLevel.Paused && activityLevel != StateActivityLevel.Inactive)
                {
                    state.Update(gameTime, inputHandler);
                }
            }

            if (pushRequests.Count > 0)
            {
                PushNewRequests();
            }
        }

        public void DrawStates(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            IEnumerable<GameState> reversedStates = states.Reverse();

            foreach (GameState state in reversedStates)
            {
                StateActivityLevel activityLevel = state.ActivityLevel;

                if (activityLevel != StateActivityLevel.Hidden && activityLevel != StateActivityLevel.Inactive)
                {
                    state.Draw(graphicsDevice, spriteBatch);
                }
            }
        }

        private void PushNewRequests()
        {
            foreach (GameState state in pushRequests)
            {
                states.Push(state);
            }

            pushRequests.Clear();
        }

        #endregion
    }
}
