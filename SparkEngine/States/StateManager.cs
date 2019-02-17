namespace SparkEngine.States
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Rendering;

    public class StateManager
    {
        #region Fields

        private Stack<GameState> states = new Stack<GameState>();
        private Stack<GameState> pushRequests = new Stack<GameState>();

        #endregion

        #region Properties

        #endregion

        #region Methods

        public void RequestStatePush(GameState state)
        {
            pushRequests.Push(state);
        }

        internal void Initialise(int screenWidth, int screenHeight)
        {
            GameState.SetDefaultCamera(new Camera(screenWidth, screenHeight));           
        }

        internal void UpdateStates(GameTime gameTime)
        {
            foreach (GameState state in states)
            {
                StateActivityLevel activityLevel = state.ActivityLevel;

                if (activityLevel != StateActivityLevel.Paused && activityLevel != StateActivityLevel.Stopped)
                {
                    state.ProcessInput(gameTime);
                    state.Update(gameTime);
                }
            }

            if (pushRequests.Count > 0)
            {
                PushNewRequests();
            }
        }

        internal void DrawWorldStates(SpriteBatch spriteBatch)
        {
            foreach (GameState state in states)
            {
                StateActivityLevel activityLevel = state.ActivityLevel;

                if (activityLevel != StateActivityLevel.Hidden && activityLevel != StateActivityLevel.Stopped)
                {
                    state.DrawWorld(spriteBatch);
                }
            }
        }

        internal void DrawScreenStates(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (GameState state in states)
            {
                StateActivityLevel activityLevel = state.ActivityLevel;

                if (activityLevel != StateActivityLevel.Hidden && activityLevel != StateActivityLevel.Stopped)
                {
                    state.DrawScreen(spriteBatch);
                }
            }

            spriteBatch.End();
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
