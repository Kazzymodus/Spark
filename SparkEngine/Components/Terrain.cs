﻿namespace SparkEngine.Components
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
        private Texture2D gridTileTexture;

        #endregion

        #region Constructors

        public Terrain(Vector2 dimensions, Texture2D tileSpriteSheet, Texture2D gridTileTexture)
        {
            Dimensions = dimensions;
            this.gridTileTexture = gridTileTexture;
            TileSize = new Vector2(tileSpriteSheet.Width / 4, tileSpriteSheet.Height); // TEMP
            GenerateCellGrid(tileSpriteSheet);
        }

        #endregion

        #region Public Properties

        public Vector2 Position { get; }

        public Vector2 Dimensions { get; }

        public Vector2 TileSize { get; }

        public Vector2 DrawPosition { get; private set; }

        public LayerSortMethod LayerSortMethod { get; } = LayerSortMethod.First;

        #endregion

        #region Public Methods

        public Vector2 GetDrawPosition(Camera camera, Vector2 unit)
        {
            return Position;
        }

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

        public bool IsFreeTile(Vector2 coordinates) // Remember to fix this with different sizes.
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

        public LayerSortMethod SpriteSortMethod { get; } = LayerSortMethod.First;

        #endregion

        #region Internal Methods

        public void Draw(SpriteBatch spriteBatch, Camera camera, DrawLayer layer)
        {
            Vector2 tileSize = layer.Unit;
            Rectangle visibleCoordinates = camera.GetVisibleIsometricCoordinates(layer.Unit, 2);

            Point startCoordinate = visibleCoordinates.Location;
            Point endCoordinate = startCoordinate + visibleCoordinates.Size;

            for (int x = startCoordinate.X; x < endCoordinate.X; x++)
            {
                for (int y = startCoordinate.Y; y < endCoordinate.Y; y++)
                {
                    Point coordinate = Projector.IsometricToCartesian(new Point(x, y));

                    if ((x + y) % 2 != 0 || coordinate.X < 0 || coordinate.X >= tileGrid.GetLength(0) || coordinate.Y < 0 || coordinate.Y >= tileGrid.GetLength(1))
                    {
                        continue;
                    }

                    TerrainTile tile = tileGrid[coordinate.X, coordinate.Y];
                    Vector2 drawPosition = Projector.CartesianToIsometricPixels(tile.Coordinates, tileSize);

                    int frameX = (int)TileSize.X * camera.Rotations;
                    Rectangle drawRectangle = new Rectangle(frameX, 0, (int)tileSize.X, (int)tileSize.Y);
                    Color colour = Color.White;

                    spriteBatch.Draw(tile.SpriteSheet, drawPosition, drawRectangle, colour);
                    spriteBatch.Draw(gridTileTexture, drawPosition, colour);

                    Vector2 iso = Projector.CartesianToIsometric(tile.Coordinates);
                    Log.AddWorldMessage("C:" + coordinate.X + "," + coordinate.Y, drawPosition + new Vector2(8), camera);
                }
            }
        }

        internal void OccupyTiles(Vector2 coordinates, GridObject occupant)
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

        internal void UnoccupyTiles(Vector2 coordinates, GridObject occupant)
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

        private void GenerateCellGrid(Texture2D tileSpriteSheet)
        {
            tileGrid = new TerrainTile[(int)Dimensions.X, (int)Dimensions.Y];
            Random colourGenner = new Random();

            for (int i = 0; i < Dimensions.X; i++)
            {
                for (int j = 0; j < Dimensions.Y; j++)
                {
                    tileGrid[i, j] = new TerrainTile(new Vector2(i, j ), tileSpriteSheet);
                }
            }
        }

        private bool TileExists(Vector2 coordinates)
        {
            return !(coordinates.X < 0 || coordinates.X >= tileGrid.GetLength(0) || coordinates.Y < 0 || coordinates.Y >= tileGrid.GetLength(1));
        }

        #endregion
    }   
}
