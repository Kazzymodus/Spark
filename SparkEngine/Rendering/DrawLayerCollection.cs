using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace SparkEngine.Rendering
{
    public class DrawLayerCollection : KeyedCollection<string, DrawLayer>
    {
        protected override string GetKeyForItem(DrawLayer item)
        {
            return item.Name;
        }

        public Vector2[] GetLayerOffsets()
        {
            var layerOffsets = new Vector2[Count];

            for (var i = 0; i < layerOffsets.Length; i++) layerOffsets[i] = Items[i].DrawOffset;

            return layerOffsets;
        }
    }
}