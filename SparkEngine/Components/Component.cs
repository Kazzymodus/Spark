namespace SparkEngine.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Entities;

    public abstract class Component
    {
        public Component()
        {
        }

        public int Owner { get; private set; }
        
        internal void SetOwner(Entity entity)
        {
            Owner = entity.ID;
        }

        public virtual void ProcessInput(GameTime gameTime)
        {

        }

        public virtual void Update(GameTime gameTime)
        {

        }
    }
}
