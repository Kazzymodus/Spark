namespace SparkEngine.Rendering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Collections.ObjectModel;
    using Microsoft.Xna.Framework.Graphics;

    public class DrawLayerCollection : KeyedCollection<string, DrawLayer>
    {
        protected override string GetKeyForItem(DrawLayer item)
        {
            return item.Name;
        }
    }
}
