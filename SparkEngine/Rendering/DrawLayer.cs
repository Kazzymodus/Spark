namespace SparkEngine.Rendering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Components;

    public class DrawLayer
    {
        private List<IDrawableComponent> components = new List<IDrawableComponent>();
        private bool isScreenLayer;

        public DrawLayer(bool isScreenLayer, Vector2 unit, Vector2 position)
        {
            this.isScreenLayer = isScreenLayer;
            Unit = unit;
            Position = position;
        }

        public Vector2 Position { get; private set; }

        public TileMode TileMode { get; private set; }

        public Vector2 Unit { get; }

        internal void RegisterComponent(IDrawableComponent component, Camera camera)
        {
            switch(component.LayerSortMethod)
            {
                case LayerSortMethod.First:
                    FirstModeInsert(component, components, camera);
                    return;
                case LayerSortMethod.HeightAsDistance:
                    HeightAsDistanceModeInsert(component, components, camera);
                    return;
                case LayerSortMethod.Last:
                    LastModeInsert(component, components, camera);
                    return;
                default:
                    throw new NotImplementedException();
            }

        }

        internal void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            Matrix transform = isScreenLayer ? Matrix.Identity : camera.Transform;

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, transform);
            
            foreach(IDrawableComponent component in components)
            {
                component.Draw(spriteBatch, camera, this);
            }

            spriteBatch.End();
        }

        private void Sort(Camera camera)
        {
            List<IDrawableComponent> drawOrder = new List<IDrawableComponent>();

            foreach (IDrawableComponent component in components)
            {
                switch (component.LayerSortMethod)
                {
                    case LayerSortMethod.First:
                        FirstModeInsert(component, drawOrder, camera);
                        break;
                    case LayerSortMethod.HeightAsDistance:
                        HeightAsDistanceModeInsert(component, drawOrder, camera);
                        break;
                }
            }

            components = drawOrder;
        }

        private void HeightAsDistanceModeInsert(IDrawableComponent component, List<IDrawableComponent> destination, Camera camera)
        {
            float drawHeight = component.GetDrawPosition(camera, this).Y;
            int index = 0;
            bool inserted = false;

            for (int i = 0; i < destination.Count; i++)
            {
                if (destination[i].GetDrawPosition(camera, this).Y > drawHeight)
                {
                    destination.Insert(index, component);
                    inserted = true;
                    break;
                }

                index = i;
            }

            if (!inserted)
            {
                destination.Add(component);
            }
        }

        private void FirstModeInsert(IDrawableComponent component, List<IDrawableComponent> destination, Camera camera)
        {
            destination.Insert(0, component);
        }

        private void LastModeInsert(IDrawableComponent component, List<IDrawableComponent> destination, Camera camera)
        {
            destination.Add(component);
        }
    }
}
