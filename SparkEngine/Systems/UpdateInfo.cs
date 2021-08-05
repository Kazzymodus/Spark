using Microsoft.Xna.Framework;
using SparkEngine.Input;
using SparkEngine.States;

namespace SparkEngine.Systems
{
    public class UpdateInfo
    {
        public UpdateInfo(GameState state, GameTime time, InputHandler input)
        {
            State = state;
            Time = time;
            Input = input;
        }

        public GameState State { get; }
        public GameTime Time { get; }
        public InputHandler Input { get; }
    }
}