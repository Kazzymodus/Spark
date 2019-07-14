namespace SparkEngine.Systems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using SparkEngine.Components;
    using SparkEngine.Input;
    using SparkEngine.States;

    class ClickableSystem : ComponentSystem<Clickable>
    {
        internal override void Update(GameState state, GameTime gameTime, InputHandler input)
        {
            
        }
    }
}
