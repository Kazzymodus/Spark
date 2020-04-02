namespace SparkEngine.Systems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using SparkEngine.States;
    using SparkEngine.Input;

    public class UpdateInfo
    {
        public GameState State { get; }
        public GameTime Time { get; }
        public InputHandler Input { get; }

        public UpdateInfo(GameState state, GameTime time, InputHandler input)
        {
            State = state;
            Time = time;
            Input = input;
        }
    }
}
