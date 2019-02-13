namespace SparkEngine.Debug
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Dictionaries;
    using SparkEngine.IDs;

    public class DebugTiler
    {
        #region Fields

        private List<DebugTile> debugTiles = new List<DebugTile>();

        #endregion

        #region Methods

        public void AddDebugTile(Vector2 coordinates)
        {
            AddDebugTile(coordinates, Color.Red);
        }

        public void AddDebugTile(Vector2 coordinates, Color colour)
        {
            DebugTile tile = new DebugTile(TileDictionary.GetTile(TileIDs.BlankTile).Texture, coordinates, colour);
            debugTiles.Add(tile);
        }

        internal void DrawTiles(SpriteBatch spriteBatch, int rotations)
        {
            foreach (DebugTile tile in debugTiles)
            {
                tile.Draw(spriteBatch, rotations);
            }

            debugTiles.Clear();
        }

        #endregion
    }
}
