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

        public static void CreateNewState()
        {
            GameState state;
        }

        public static void RequestStatePush(GameState state)
        {
            //foreach (GameState primary in PrimaryStates)
            //{
            //    if (primary.GetType().Equals(state.GetType()))
            //    {
            //        throw new Exception("Can not request to push primary state " + state.ToString() + ".");
            //    }
            //}

            pushRequests.Push(state);
        }

        //public static bool StateExists(int stateId)
        //{
        //    foreach (GameState state in gameStates)
        //    {
        //        if (state.ID == stateId)
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        //public static bool StateExists(Type stateType)
        //{
        //    foreach (GameState state in gameStates)
        //    {
        //        if (state.GetType() == stateType)
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        //public static GameState GetState(int stateId)
        //{
        //    foreach (GameState state in gameStates)
        //    {
        //        if (state.ID == stateId)
        //        {
        //            return state;
        //        }
        //    }

        //    return null;
        //}

        internal static void Initialise(int screenWidth, int screenHeight)
        {
            GameState.SetDefaultCamera(new Camera(screenWidth, screenHeight));
            
        }

        internal static void UpdateStates(GameTime gameTime)
        {
            foreach (GameState state in states)
            {
                if (state.ActivityLevel != StateActivityLevel.Paused && state.ActivityLevel != StateActivityLevel.Inactive)
                {
                    secondary.Update(gameTime);
                }
            }

            if (pushRequests.Count > 0)
            {
                PushNewRequests();
            }
        }

        internal static void DrawWorldStates(SpriteBatch spriteBatch)
        {
            foreach (GameState primary in PrimaryStates)
            {
                if (primary.IsActive)
                {
                    primary.DrawWorld(spriteBatch);
                }
            }
            
            foreach (GameState secondary in states)
            {
                if (secondary.IsActive)
                {
                    secondary.DrawWorld(spriteBatch);
                }
            }
        }

        internal static void DrawScreenStates(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (GameState primary in PrimaryStates)
            {
                if (primary.IsActive)
                {
                    primary.DrawScreen(spriteBatch);
                }
            }
                        
            foreach (GameState secondary in states)
            {
                if (secondary.IsActive)
                {
                    secondary.DrawScreen(spriteBatch);
                }
            }

            spriteBatch.End();
        }

        private static void PushNewRequests()
        {
            foreach (GameState state in pushRequests)
            {
                if (!state.CanHaveMultiples && CheckForDuplicates(state))
                {
                    throw new Exception("Only one instance of " + state.GetType() + " is allowed to exist.");
                }

                states.Push(state);
            }

            pushRequests.Clear();
        }

        private static bool CheckForDuplicates(GameState newState)
        {
            foreach (GameState activeState in states)
            {
                if (newState.GetType().Equals(activeState.GetType()))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
