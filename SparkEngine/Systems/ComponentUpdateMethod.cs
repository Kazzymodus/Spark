namespace SparkEngine.Systems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using SparkEngine.Input;
    using SparkEngine.Components;
    using SparkEngine.States;

    public class ComponentUpdateMethod
    {
        public Action<GameState, GameTime, InputHandler, ComponentBatch> Update { get; }
        public Type[] RequiredComponents { get; }

        private ComponentUpdateMethod(Action<GameState, GameTime, InputHandler, ComponentBatch> method, Type[] requiredComponents)
        {
            Update = method;
            RequiredComponents = requiredComponents;
        }

        public ComponentUpdateMethod(ComponentSystem system)
            : this(system.UpdateIndividual, system.RequiredComponents)
        {

        }
    }
}
