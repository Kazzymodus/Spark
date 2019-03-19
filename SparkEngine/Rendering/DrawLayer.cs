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
            components.Add(component);
        }

        internal void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            Sort();

            foreach(IDrawableComponent component in components)
            {
                component.Draw(spriteBatch, camera);
            }
        }

        private void Sort()
        {
            List<IDrawableComponent> drawOrder = new List<IDrawableComponent>();
            
            foreach (IDrawableComponent component in components)
            {
                if (component.SpriteSortMethod == SpriteSortMethod.HeightAsDistance)
                {
                    float drawHeight = component.DrawPosition.Y;
                    int index = 0;
                    bool inserted = false;

                    for (int i = 0; i < drawOrder.Count; i++)
                    {
                        if (drawOrder[i].DrawPosition.Y > drawHeight)
                        {
                            drawOrder.Insert(index, component);
                            inserted = true;
                            break;
                        }

                        index = i;
                    }

                    if (!inserted)
                    {
                        drawOrder.Add(component);
                    }
                }
            }

            components = drawOrder;
        }
    }
}
