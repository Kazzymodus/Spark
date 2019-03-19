namespace SparkEngine.Assets
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AssetDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> assetDictionary = new Dictionary<TKey, TValue>();

        public TValue GetAsset(TKey key)
        {
            return assetDictionary[key];
        }

        public void AddAsset(TKey key, TValue asset)
        {
            assetDictionary.Add(key, asset);
        }
    }
}
