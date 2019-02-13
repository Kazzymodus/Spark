namespace SparkEngine.States
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Rendering;
    using SparkEngine.States.Primary;

    public static class StateManager
    {
        #region Fields

        private static Stack<GameState> states = new Stack<GameState>();
        private static Stack<GameState> pushRequests = new Stack<GameState>();

        #endregion

        #region Properties

        #endregion

        #region Methods

        public static void RequestStatePush(GameState state)
        {
            pushRequests.Push(state);
        }

        internal static void Initialise(int screenWidth, int screenHeight)
        {
            GameState.SetDefaultCamera(new Camera(screenWidth, screenHeight));           
        }

        internal static void UpdateStates(GameTime gameTime)
        {
            foreach (GameState state in states)
            {
                StateActivityLevel activityLevel = state.ActivityLevel;

                if (activityLevel != StateActivityLevel.Paused && activityLevel != StateActivityLevel.Stopped)
                {
                    state.Update(gameTime);
                }
            }

            if (pushRequests.Count > 0)
            {
                PushNewRequests();
            }
        }

        internal static void DrawWorldStates(SpriteBatch spriteBatch)
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

        internal static void DrawScreenStates(SpriteBatch spriteBatch)
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

        private static void PushNewRequests()
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
