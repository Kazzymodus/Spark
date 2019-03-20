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

        internal void RegisterComponent(IDrawableComponent component)
        {
            switch(component.SpriteSortMethod)
            {
                case SpriteSortMethod.First:
                    FirstAdd(component, components);
                    return;
                case SpriteSortMethod.HeightAsDistance:
                    HeightAsDistanceAdd(component, components);
                    return;
                default:
                    throw new NotImplementedException();
            }

        }

        internal void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Begin();
            
            foreach(IDrawableComponent component in components)
            {
                component.Draw(spriteBatch, camera);
            }

            spriteBatch.End();
        }

        private void Sort()
        {
            List<IDrawableComponent> drawOrder = new List<IDrawableComponent>();

            foreach (IDrawableComponent component in components)
            {
                switch (component.SpriteSortMethod)
                {
                    case SpriteSortMethod.First:
                        FirstAdd(component, drawOrder);
                        break;
                    case SpriteSortMethod.HeightAsDistance:
                        HeightAsDistanceAdd(component, drawOrder);
                        break;
                }
            }

            components = drawOrder;
        }

        private void HeightAsDistanceAdd(IDrawableComponent component, List<IDrawableComponent> destination)
        {
            float drawHeight = component.DrawPosition.Y;
            int index = 0;
            bool inserted = false;

            for (int i = 0; i < destination.Count; i++)
            {
                if (destination[i].DrawPosition.Y > drawHeight)
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

        private void FirstAdd(IDrawableComponent component, List<IDrawableComponent> destination)
        {
            destination.Insert(0, component);
        }
    }
}
