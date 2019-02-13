namespace SparkEngine.World
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.DataStructures;
    using SparkEngine.Debug;
    using SparkEngine.Dictionaries;
    using SparkEngine.IDs;
    using SparkEngine.Level;
    using SparkEngine.Rendering;
    using SparkEngine.States;

    public class Tile
    {
        #region Private Fields

        private Texture2D tileTexture;
        private Texture2D gridTexture;
        private int colourOffset;

        #endregion

        #region Constructors

        public Tile(int xCoord, int yCoord, TileData tileData, int colourOffset = 255)
        {
            Coordinates = new Vector2(xCoord, yCoord);
            tileTexture = tileData.Texture;

            this.colourOffset = colourOffset;

            gridTexture = TextureDictionary.GetTileTexture(TileIDs.GridTile);
        }

        #endregion

        #region Public Properties

        public Vector2 Coordinates { get; }

        public int MovementCost { get; }

        public WorldObject Occupant { get; private set; }

        public bool IsOccupied
        {
            get { return Occupant != null; }
        }

        #endregion

        #region Internal Methods

        internal void Occupy(WorldObject occupant)
        {
            Occupant = occupant;
        }

        internal void Unoccupy()
        {
            Occupant = null;
        }

        internal void Draw(SpriteBatch spriteBatch, Map map, bool drawGrid)
        {
            Vector2 correctedCoords = Coordinates;
            correctedCoords.X -= RenderHelper.TerrainSize.X;

            Vector2 rotatedCoords = RenderHelper.RotateCoordsInMap(Coordinates, map.Rotations);
            Vector2 drawPosition = RenderHelper.CoordsToPixels(rotatedCoords);

            int frameX = RenderHelper.DefaultTileWidth * map.Rotations;

            Rectangle drawRectangle = new Rectangle(frameX, 0, RenderHelper.DefaultTileWidth, RenderHelper.DefaultTileHeight);

            Color colour = new Color(255, colourOffset, 255);
            //Color colour = Color.White;

            spriteBatch.Draw(tileTexture, drawPosition, drawRectangle, colour);

            if (drawGrid)
            {
                spriteBatch.Draw(gridTexture, drawPosition, Color.White);
            }

            if (StateManager.DebugState.IsActive)
            {
                string logMessage = "";

                switch (StateManager.DebugState.CellDataMode)
                {
                    case CellPositionInfo.StaticCarthesian:
                        logMessage = "C: " + Coordinates.X + "." + Coordinates.Y;
                        break;
                    case CellPositionInfo.StaticIsometric:
                        Vector2 iso = RenderHelper.CoordsToIsometric(Coordinates);
                        logMessage = "I: " + iso.X + "." + iso.Y;
                        break;
                    case CellPositionInfo.RotatedCarthesian:
                        logMessage = "C: " + rotatedCoords.X + "." + rotatedCoords.Y;
                        break;
                    case CellPositionInfo.RotatedIsometric:
                        iso = RenderHelper.CoordsToIsometric(rotatedCoords);
                        logMessage = "I: " + iso.X + "." + iso.Y;
                        break;
                }

                StateManager.DebugState.DebugLog.AddWorldMessage(logMessage, drawPosition + DebugLog.TileMessageOffset);
            }
        }

        #endregion
    }
}
