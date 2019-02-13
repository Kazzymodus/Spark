namespace SparkEngine.World
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.DataStructures;
    using SparkEngine.Dictionaries;
    using SparkEngine.Level;
    using SparkEngine.IDs;
    using SparkEngine.Rendering;

    public class Terrain
    {
        #region Private Fields

        private Tile[,] tileGrid;
        private readonly MapTileMode tileMode;

        #endregion

        #region Constructors

        public Terrain(MapTileMode tileMode, TileData tileData, Vector2 dimensions)
        {
            this.tileMode = tileMode;
            Dimensions = dimensions;

            GenerateCellGrid(tileData);
            GeneratePillarSpots();
        }

        #endregion

        #region Public Properties

        public Vector2 Dimensions { get; }

        #endregion

        #region Public Methods

        public bool IsValidPlacement(Vector2 coordinates, Vector2 dimensions)
        {
            for (int xTile = 0; xTile < dimensions.X; xTile++)
            {
                for (int yTile = 0; yTile < dimensions.Y; yTile++)
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

        public bool IsOutOfBounds(Vector2 coords)
        {
            if (coords.X < 0 || coords.X >= Dimensions.X || coords.Y < 0 || coords.Y >= Dimensions.Y)
            {
                return true;
            }

            return false;
        }

        public bool IsOccupiedTile(Vector2 coordinates)
        {
            return tileGrid[(int)coordinates.X, (int)coordinates.Y].IsOccupied;
        }

        public bool IsBlockingTile(Vector2 coordinates)
        {
            return !(tileGrid[(int)coordinates.X, (int)coordinates.Y].Occupant == null || !tileGrid[(int)coordinates.X, (int)coordinates.Y].Occupant.IsPathBlocker);
        }

        #endregion

        #region Internal Methods

        internal void DrawTiles(SpriteBatch spriteBatch, Map map, Rectangle visibleCoordinates, bool drawGrid)
        {
            foreach (Tile cell in tileGrid)
            {
                if (visibleCoordinates.Contains(RenderHelper.CoordsToIsometric(map.RotateCoordsInMap(cell.Coordinates))))
                {
                    cell.Draw(spriteBatch, map, drawGrid);
                }
            }
        }

        internal void OccupyTiles(Vector2 coordinates, WorldObject occupant)
        {
            Vector2 dimensions = occupant.DrawData.Dimensions;

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
            UnoccupyTiles(coordinates, occupant.DrawData.Dimensions);
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

        private void GenerateCellGrid(TileData tileData)
        {
            tileGrid = new Tile[(int)Dimensions.X, (int)Dimensions.Y];
            Random colourGenner = new Random();

            for (int i = 0; i < Dimensions.X; i++)
            {
                for (int j = 0; j < Dimensions.Y; j++)
                {
                    tileGrid[i, j] = new Tile(i, j, tileData, colourGenner.Next(220, 256));
                }
            }
        }

        private void GeneratePillarSpots()
        {
            Point startTile = new Point((int)Dimensions.X / 2, (int)Dimensions.Y / 3);
            Point[] square = new Point[4];

            for (int xTile = 0; xTile < square.Length / 2; xTile++)
            {
                for (int yTile = 0; yTile < square.Length / 2; yTile++)
                {
                    int xCoord = startTile.X - xTile;
                    int yCoord = startTile.Y - yTile;

                    square[xTile * 2 + yTile] = new Point(xCoord, yCoord);
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < square.Length; j++)
                {
                    Point oldTile = square[j];
                    Point newTile = new Point(((int)Dimensions.X - 1) - oldTile.Y, oldTile.X);

                    square[j] = newTile;
                    tileGrid[newTile.X, newTile.Y] = new Tile(newTile.X, newTile.Y, TileDictionary.GetTile(TileIDs.BlankTile), 150);
                }
            }
        }

        private bool TileExists(Vector2 coordinates)
        {
            if (coordinates.X < 0 || coordinates.X >= tileGrid.GetLength(0) || coordinates.Y < 0 || coordinates.Y >= tileGrid.GetLength(1))
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
