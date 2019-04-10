namespace SparkEngine.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;

    public interface IInputListener
    {
        void ProcessInput(GameTime gameTime, out bool validInputReceived);
    }
}
