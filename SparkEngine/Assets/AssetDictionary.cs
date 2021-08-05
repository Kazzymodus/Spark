using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace SparkEngine.Assets
{
    /// <summary>
    ///     Currently not more than a wrapper, but let's see where it takes us.
    /// </summary>
    /// <typeparam name="TAsset"></typeparam>
    public class AssetDictionary<TAsset>
    {
        private readonly IDictionary<string, TAsset> assetDictionary = new Dictionary<string, TAsset>();
        private readonly string assetPath;
        private readonly ContentManager content;

        public AssetDictionary(string assetPath, ContentManager content)
        {
            this.assetPath = assetPath + '/';
            this.content = content;
        }

        public TAsset GetAsset(string key)
        {
            if (assetDictionary.TryGetValue(key, out var value))
                return value;
            throw new Exception("No asset with key \"" + key + "\" was found.");
        }

        public void AddAsset(string key, TAsset asset)
        {
            assetDictionary.Add(key, asset);
        }

        /// <summary>
        ///     Tries to load the asset with the given name and add it to the asset dictionary.
        /// </summary>
        /// <param name="assetName">The file name of the asset.</param>
        /// <returns>Was the asset successfully added?</returns>
        public bool TryAddAsset(string assetName)
        {
            TAsset asset;

            try
            {
                asset = content.Load<TAsset>(assetPath + assetName);
            }
            catch (ContentLoadException)
            {
                return false;
            }

            var keyValuePair = new KeyValuePair<string, TAsset>(assetName, asset);

            assetDictionary.Add(keyValuePair);
            return true;
        }
    }
}