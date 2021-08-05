using Microsoft.Xna.Framework;

namespace SparkEngine.Rendering
{
    public class DrawLayer
    {
        public int ID { get; }

        public string Name { get; }

        public Vector2 Position { get; private set; }

        public Vector2 UnitSize { get; }

        public Vector2 DrawOffset => Position * UnitSize;

        /*

        public Rectangle GetBounds(Camera camera)
        {
            if (components.Count == 0)
            {
                return new Rectangle();
            }

            Vector2 boundsPosition = components[0].GetDrawPosition(camera, this);
            Vector2 boundsEnd = boundsPosition;
            
            for (int i = 1; i < components.Count; i++)
            {
                Vector2 drawPosition = components[i].GetDrawPosition(camera, this);
                Point size = components[i].GetBounds(camera, this).Size;

                if (drawPosition.X < boundsPosition.X)
                {
                    boundsPosition.X = drawPosition.X;
                }
                else if (drawPosition.X + size.X > boundsEnd.X)
                {
                    boundsEnd.X = drawPosition.X;
                }

                if (drawPosition.Y < boundsPosition.Y)
                {
                    boundsPosition.Y = drawPosition.Y;
                }
                else if (drawPosition.Y + size.Y > boundsEnd.Y)
                {
                    boundsEnd.Y = drawPosition.Y;
                }
            }

            Vector2 boundsSize = boundsEnd - boundsPosition;

            return new Rectangle(boundsPosition.ToPoint(), boundsSize.ToPoint());
        }
        
        public Drawable GetComponentAtPosition(Point position, Camera camera)
        {
            foreach (Drawable component in components)
            {
                if (component.GetBounds(camera, this).Contains(position))
                {
                    return component;
                }
            }

            return null;
        }

        internal void RegisterComponent(Drawable component, Camera camera)
        {
            component.DestroyEvent += OnComponentDestroy;

            switch (component.LayerSortMethod)
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
            
            foreach(Drawable component in components)
            {
                component.Draw(spriteBatch, camera, this);
            }

            spriteBatch.End();
        }

        private void Sort(Camera camera)
        {
            List<Drawable> drawOrder = new List<Drawable>();

            foreach (Drawable component in components)
            {
                switch (component.LayerSortMethod)
                {
                    case LayerSortMethod.First:
                        FirstModeInsert(component, drawOrder, camera);
                        break;
                    case LayerSortMethod.HeightAsDistance:
                        HeightAsDistanceModeInsert(component, drawOrder, camera);
                        break;
                    case LayerSortMethod.Last:
                        LastModeInsert(component, components, camera);
                        return;
                }
            }

            components = drawOrder;
        }

        private void HeightAsDistanceModeInsert(Drawable component, List<Drawable> destination, Camera camera)
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

        private void FirstModeInsert(Drawable component, List<Drawable> destination, Camera camera)
        {
            destination.Insert(0, component);
        }

        private void LastModeInsert(Drawable component, List<Drawable> destination, Camera camera)
        {
            destination.Add(component);
        }

        private void OnComponentDestroy(object sender, EventArgs args)
        {
            components.Remove((Drawable)sender);
        }

        */
    }
}