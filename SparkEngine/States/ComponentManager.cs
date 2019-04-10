namespace SparkEngine.States
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SparkEngine.Components;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    internal abstract class ComponentManager
    {
        public abstract Type ComponentType { get; }

        internal abstract void ProcessInput(GameTime gameTime);

        internal abstract void Update(GameTime gameTime);
    }

    internal class ComponentManager<T> : ComponentManager where T : Component
    {
        private List<T> components = new List<T>();

        // private readonly bool useDoublebuffer = typeof(T) is INeedsDoubleBufferingButIHaventThoughtOfAProperNameForThisYet; // TEST STUFF, DON'T JUDGE
        
        public override Type ComponentType
        {
            get { return typeof(T); }
        }

        public void AddComponent(T component)
        {
            components.Add(component);
        }

        internal override void ProcessInput(GameTime gameTime)
        {
            foreach(Component component in components)
            {
                component.ProcessInput(gameTime, out bool gotUsableInput);

                if (!gotUsableInput)
                {
                    break;
                }
            }
        }

        internal override void Update(GameTime gameTime)
        {
            foreach(Component component in components)
            {
                component.Update(gameTime);
            }
        }
    }
}
