namespace SparkEngine.Rendering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Collections.ObjectModel;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    
    public class DrawLayerCollection : KeyedCollection<string, DrawLayer>
    {
        protected override string GetKeyForItem(DrawLayer item)
        {
            return item.Name;
        }

        public Vector2[] GetLayerOffsets()
        {
            Vector2[] layerOffsets = new Vector2[Count];

            for (int i = 0; i < layerOffsets.Length; i++)
            {
                layerOffsets[i] = Items[i].DrawOffset;
            }

            return layerOffsets;
        }
    }
}
