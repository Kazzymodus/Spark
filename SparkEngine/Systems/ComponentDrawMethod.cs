namespace SparkEngine.Systems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Components;
    using SparkEngine.States;

    public class ComponentDrawMethod
    {
        public Action<GameState, GraphicsDevice, SpriteBatch, Vector2, ComponentBatch> Draw { get; }
        public Type[] RequiredComponents { get; }

        private ComponentDrawMethod(Action<GameState, GraphicsDevice, SpriteBatch, Vector2, ComponentBatch> method, Type[] requiredComponents)
        {
            Draw = method;
            RequiredComponents = requiredComponents;
        }

        public ComponentDrawMethod(DrawSystem system)
            : this (system.DrawIndividual, system.RequiredComponents)
        {

        }
    }
}
