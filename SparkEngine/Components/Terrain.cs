namespace SparkEngine.Components
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Rendering;

    public class Terrain : Component, IDrawableComponent
    {
        #region Private Fields

        private TerrainTile[,] tileGrid;
        private readonly TileMode tileMode;

        #endregion

        #region Constructors

        //public Terrain(MapTileMode tileMode, TileData tileData, Vector2 dimensions)
        //{
        //    this.tileMode = tileMode;
        //    Dimensions = dimensions;

        //    GenerateCellGrid(tileData);
        //    GeneratePillarSpots();
        //}

        #endregion

        #region Public Properties

        public Vector2 Dimensions { get; }

        public Vector2 TileSize { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the tile corresponding to the passed coordinates.
        /// </summary>
        /// <param name="coordinates">The coordinates of the tile.</param>
        /// <returns></returns>
        public TerrainTile GetTile(Vector2 coordinates)
        {
            return tileGrid[(int)coordinates.X, (int)coordinates.Y];
        }

        //public GetTiles(Predicate<T> predicate)
        //{
        //    List<TerrainTile> terrainTiles;
        //}

        public bool WorldObjectCanBePlaced(WorldObject gameObject, Vector2 coordinates)
        {
            for (int xTile = 0; xTile < Dimensions.X; xTile++)
            {
                for (int yTile = 0; yTile < Dimensions.Y; yTile++)
                {
                    int xCoord = (int)coordinates.X - xTile;
                    int yCoord = (int)coordinates.Y - yTile;

                    if (!TileExists(new Vector2(xCoord, yCoord)) || tileGrid[xCoord, yCoord].IsOccupied)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool IsWithinTerrainBounds(Vector2 coordinates)
        {
            return !(coordinates.X < 0 || coordinates.X >= Dimensions.X || coordinates.Y < 0 || coordinates.Y >= Dimensions.Y);
        }

        public bool TileIsOccupied(Vector2 coordinates)
        {
            return tileGrid[(int)coordinates.X, (int)coordinates.Y].IsOccupied;
        }

        public bool TileIsBlocked(Vector2 coordinates)
        {
            return !(tileGrid[(int)coordinates.X, (int)coordinates.Y].Occupant == null); // || !tileGrid[(int)coordinates.X, (int)coordinates.Y].Occupant.IsPathBlocker);
        }

        public SpriteSortMethod SpriteSortMethod { get; }

        public Vector2 DrawPosition { get; private set; }

        #endregion

        #region Internal Methods

        public void CalculateDrawPosition(Camera camera)
        {
            DrawPosition = Vector2.Zero;
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            foreach (TerrainTile cell in tileGrid)
            {
                if (camera.GetVisibleCoordinates().Contains(cell.Coordinates))
                {
                    cell.Draw(spriteBatch, camera);
                }
            }
        }

        internal void OccupyTiles(Vector2 coordinates, WorldObject occupant)
        {
            Vector2 dimensions = occupant.Dimensions;

            for (int xTile = 0; xTile < dimensions.X; xTile++)
            {
                for (int yTile = 0; yTile < dimensions.Y; yTile++)
                {
                    int xCoord = (int)coordinates.X - xTile;
                    int yCoord = (int)coordinates.Y - yTile;

                    tileGrid[xCoord, yCoord].Occupy(occupant);
                }
            }
        }

        internal void UnoccupyTiles(Vector2 coordinates, WorldObject occupant)
        {
            UnoccupyTiles(coordinates, occupant.Dimensions);
        }

        internal void UnoccupyTiles(Vector2 coordinates, Vector2 dimensions)
        {
            for (int xTile = 0; xTile < dimensions.X; xTile++)
            {
                for (int yTile = 0; yTile < dimensions.Y; xTile++)
                {
                    int xCoord = (int)coordinates.X - xTile;
                    int yCoord = (int)coordinates.Y - yTile;

                    tileGrid[xCoord, yCoord].Unoccupy();
                }
            }
        }

        #endregion

        #region Private Fields

        private void GenerateCellGrid()
        {
            tileGrid = new TerrainTile[(int)Dimensions.X, (int)Dimensions.Y];
            Random colourGenner = new Random();

            for (int i = 0; i < Dimensions.X; i++)
            {
                for (int j = 0; j < Dimensions.Y; j++)
                {
                    tileGrid[i, j] = new TerrainTile();
                }
            }
        }

        private bool TileExists(Vector2 coordinates)
        {
            return !(coordinates.X < 0 || coordinates.X >= tileGrid.GetLength(0) || coordinates.Y < 0 || coordinates.Y >= tileGrid.GetLength(1));
        }

        #endregion
    }

    /// <summary>
    /// Determines what shape the rendererer considers the terrain tiles to be for rendering purpose.
    /// </summary>
    public enum TileMode
    {
        /// <summary>
        /// Tiles are square.
        /// </summary>
        Square,
        /// <summary>
        /// Tiles are diamond.
        /// </summary>
        Diamond,
        /// <summary>
        /// Tiles are hexagonal. Let's not use this one yet.
        /// </summary>
        Hexagonal
    }

    public class TerrainTile
    {
        #region Private Fields

        public Texture2D SpriteSheet { get; set; }

        #endregion

        #region Constructors

        #endregion

        #region Public Properties

        public Color Color { get; set; } = Color.White;

        public Vector2 Coordinates { get; }

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

        internal void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            throw new NotImplementedException();
        }

        //internal void Draw(SpriteBatch spriteBatch, Map map, bool drawGrid)
        //{
        //    Vector2 correctedCoords = Coordinates;
        //    correctedCoords.X -= RenderHelper.TerrainSize.X;

        //    Vector2 rotatedCoords = RenderHelper.RotateCoordsInMap(Coordinates, map.Rotations);
        //    Vector2 drawPosition = RenderHelper.CoordsToPixels(rotatedCoords);

        //    int frameX = RenderHelper.DefaultTileWidth * map.Rotations;

        //    Rectangle drawRectangle = new Rectangle(frameX, 0, RenderHelper.DefaultTileWidth, RenderHelper.DefaultTileHeight);

        //    Color colour = new Color(255, colourOffset, 255);
        //    //Color colour = Color.White;

        //    spriteBatch.Draw(tileTexture, drawPosition, drawRectangle, colour);

        //    if (drawGrid)
        //    {
        //        spriteBatch.Draw(gridTexture, drawPosition, Color.White);
        //    }

        //    if (StateManager.DebugState.IsActive)
        //    {
        //        string logMessage = "";

        //        switch (StateManager.DebugState.CellDataMode)
        //        {
        //            case CellPositionInfo.StaticCarthesian:
        //                logMessage = "C: " + Coordinates.X + "." + Coordinates.Y;
        //                break;
        //            case CellPositionInfo.StaticIsometric:
        //                Vector2 iso = RenderHelper.CoordsToIsometric(Coordinates);
        //                logMessage = "I: " + iso.X + "." + iso.Y;
        //                break;
        //            case CellPositionInfo.RotatedCarthesian:
        //                logMessage = "C: " + rotatedCoords.X + "." + rotatedCoords.Y;
        //                break;
        //            case CellPositionInfo.RotatedIsometric:
        //                iso = RenderHelper.CoordsToIsometric(rotatedCoords);
        //                logMessage = "I: " + iso.X + "." + iso.Y;
        //                break;
        //        }

        //        StateManager.DebugState.DebugLog.AddWorldMessage(logMessage, drawPosition + DebugLog.TileMessageOffset);
        //    }
        //}

        #endregion
    }
}
