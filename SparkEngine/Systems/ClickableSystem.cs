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

    class ClickableSystem : ComponentSystem
    {
        public override void UpdateAll(GameState state, GameTime gameTime, InputHandler input)
        {
            //MouseButtons mouseInput

            base.UpdateAll(state, gameTime, input);
        }

        public override void UpdateIndividual(GameState state, GameTime gameTime, InputHandler input, ComponentBatch components)
        {
            Clickable clickable = components.GetComponent<Clickable>();

            if (input.IsMousePressed(clickable.ValidInput))
            {
                clickable.OnClickEvent?.Invoke(this, new EventArgs());
            }
        }
    }
}
