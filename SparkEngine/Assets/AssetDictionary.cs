namespace SparkEngine.Assets
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework.Content;

    /// <summary>
    /// Currently not more than a wrapper, but let's see where it takes us.
    /// </summary>
    /// <typeparam name="TAsset"></typeparam>
    public class AssetDictionary<TAsset>
    {
        private readonly IDictionary<string, TAsset> assetDictionary = new Dictionary<string, TAsset>();
        private readonly ContentManager content;
        private readonly string assetPath;

        public AssetDictionary(string assetPath, ContentManager content)
        {
            this.assetPath = assetPath + '/';
            this.content = content;
        }

        public TAsset GetAsset(string key)
        {
            assetDictionary.TryGetValue(key, out TAsset value);

            return value;
        }

        public void AddAsset(string key, TAsset asset)
        {
            assetDictionary.Add(key, asset);
        }

        /// <summary>
        /// Tries to load the asset with the given name and add it to the asset dictionary.
        /// </summary>
        /// <param name="assetName">The file name of the asset.</param>
        /// <returns>Was the asset succesfully added?</returns>
        public bool TryAddAsset(string assetName)
        {
            TAsset asset = default(TAsset);

            try
            {
                asset = content.Load<TAsset>(assetPath + assetName);
            }
            catch (ContentLoadException)
            {
                return false;
            }

            KeyValuePair<string, TAsset> keyValuePair = new KeyValuePair<string, TAsset>(assetName, asset);

            assetDictionary.Add(keyValuePair);
            return true;
        }
    }
}
