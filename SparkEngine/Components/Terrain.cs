namespace SparkEngine.Components
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Rendering;

    public class Terrain : Component, IDrawableComponent
    {
        #region Fields

        private const int TileDrawPadding = 2;
        private const int MaxTextures = 256;

        private TerrainTile[,] tileGrid;
        private Texture2D[] tileTextures;

        private Action<SpriteBatch, Camera, DrawLayer> DrawMethod;

        #endregion

        #region Constructors

        private Terrain(TileMode tileMode, Vector2 position, Vector2 dimensions, params Texture2D[] tileTextures)
        {
            Position = position;
            Dimensions = dimensions;
            this.tileTextures = tileTextures;
            tileGrid = new TerrainTile[(int)Dimensions.X, (int)Dimensions.Y];
            GenerateCells();

            switch (tileMode)
            {
                case TileMode.Square:
                    DrawMethod = SquareDraw;
                    break;
                case TileMode.Isometric:
                    DrawMethod = IsometricDraw;
                    break;
            }
        }

        #endregion

        #region Properties

        public Vector2 Position { get; }

        public Vector2 Dimensions { get; }
        
        public LayerSortMethod LayerSortMethod { get; } = LayerSortMethod.First;

        #endregion

        #region Methods

        public static Terrain CreateSquareTerrain(Vector2 position, Vector2 dimensions, params Texture2D[] textures)
        {
            Terrain terrain = new Terrain(TileMode.Square, position, dimensions, textures);
            return terrain;
        }
        
        public static Terrain CreateIsometricTerrain(Vector2 position, Vector2 dimensions, params Texture2D[] textures)
        {
            Terrain terrain = new Terrain(TileMode.Isometric, position, dimensions, textures);
            return terrain;
        }

        public Vector2 GetDrawPosition(Camera camera, DrawLayer drawLayer)
        {
            return drawLayer.Position + Position * drawLayer.Unit;
        }

        /// <summary>
        /// Get the tile corresponding to the passed coordinates.
        /// </summary>
        /// <param name="coordinates">The coordinates of the tile.</param>
        /// <returns></returns>
        public TerrainTile GetTile(Point coordinates)
        {
            return tileGrid[coordinates.X, coordinates.Y];
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

        //public bool IsFreeTile(Vector2 coordinates) // Remember to fix this with different sizes.
        //{
        //    for (int xTile = 0; xTile < Dimensions.X; xTile++)
        //    {
        //        for (int yTile = 0; yTile < Dimensions.Y; yTile++)
        //        {
        //            int xCoord = (int)coordinates.X - xTile;
        //            int yCoord = (int)coordinates.Y - yTile;

        //            if (!TileExists(new Vector2(xCoord, yCoord)) || tileGrid[xCoord, yCoord].IsOccupied)
        //            {
        //                return false;
        //            }
        //        }
        //    }

        //    return true;
        //}

        public bool IsWithinBounds(Point coordinate)
        {
            return !(coordinate.X < 0 || coordinate.X >= Dimensions.X || coordinate.Y < 0 || coordinate.Y >= Dimensions.Y);
        }

        //public bool TileIsOccupied(Vector2 coordinates)
        //{
        //    return tileGrid[(int)coordinates.X, (int)coordinates.Y].IsOccupied;
        //}

        //public bool TileIsBlocked(Vector2 coordinates)
        //{
        //    return !(tileGrid[(int)coordinates.X, (int)coordinates.Y].Occupant == null); // || !tileGrid[(int)coordinates.X, (int)coordinates.Y].Occupant.IsPathBlocker);
        //}

        public LayerSortMethod SpriteSortMethod { get; } = LayerSortMethod.First;

        public void Draw(SpriteBatch spriteBatch, Camera camera, DrawLayer drawLayer)
        {
            DrawMethod(spriteBatch, camera, drawLayer);            
        }

        public void SquareDraw(SpriteBatch spriteBatch, Camera camera, DrawLayer drawLayer)
        {
            Vector2 tileSize = drawLayer.Unit;
            Vector2 terrainPosition = GetDrawPosition(camera, drawLayer);
            Rectangle visibleCoordinates = camera.GetVisibleCartesianCoordinates(tileSize, TileDrawPadding);

            Point offset = Projector.PixelsToCartesian(terrainPosition, tileSize).ToPoint();
            Point startCoordinate = visibleCoordinates.Location - offset;
            Point endCoordinate = startCoordinate + visibleCoordinates.Size;

            Log.AddListMessage(startCoordinate.ToString());
            Log.AddListMessage(endCoordinate.ToString());

            startCoordinate.X = MathHelper.Clamp(startCoordinate.X, 0, (int)Dimensions.X);
            startCoordinate.Y = MathHelper.Clamp(startCoordinate.Y, 0, (int)Dimensions.Y);
            endCoordinate.X = MathHelper.Clamp(endCoordinate.X, 0, (int)Dimensions.X);
            endCoordinate.Y = MathHelper.Clamp(endCoordinate.Y, 0, (int)Dimensions.Y);

            int frameX = (int)tileSize.X * camera.Rotations;
            Rectangle drawRectangle = new Rectangle(frameX, 0, (int)tileSize.X, (int)tileSize.Y);

            for (int x = startCoordinate.X; x < endCoordinate.X; x++)
            {
                for (int y = startCoordinate.Y; y < endCoordinate.Y; y++)
                {
                    TerrainTile tile = tileGrid[x, y];
                    Vector2 drawPosition = terrainPosition + new Vector2(x * tileSize.X, y * tileSize.Y);

                    Texture2D texture = tileTextures[tile.TextureId];
                    Color colour = tile.Colour;

                    spriteBatch.Draw(texture, drawPosition, drawRectangle, colour);

                    Log.AddWorldMessage(x + "\n" + y, drawPosition, camera);
                }
            }
        }

        public void IsometricDraw(SpriteBatch spriteBatch, Camera camera, DrawLayer drawLayer)
        {
            Vector2 tileSize = drawLayer.Unit;
            Vector2 terrainPosition = GetDrawPosition(camera, drawLayer);

            Rectangle visibleCoordinates = camera.GetVisibleIsometricCoordinates(tileSize, TileDrawPadding);

            Point offset = Projector.PixelsToIsometric(terrainPosition, tileSize).ToPoint();
            Point startCoordinate = visibleCoordinates.Location - offset;
            Point endCoordinate = startCoordinate + visibleCoordinates.Size;

            int frameX = (int)tileSize.X * camera.Rotations;
            Rectangle drawRectangle = new Rectangle(frameX, 0, (int)tileSize.X, (int)tileSize.Y);

            for (int x = startCoordinate.X; x < endCoordinate.X; x++)
            {
                for (int y = startCoordinate.Y; y < endCoordinate.Y; y++)
                {
                    Point coordinate = Projector.IsometricToCartesian(new Point(x, y));

                    if ((x + y) % 2 != 0 || !IsWithinBounds(coordinate))
                    {
                        continue;
                    }

                    TerrainTile tile = tileGrid[coordinate.X, coordinate.Y];
                    Vector2 drawPosition = terrainPosition + new Vector2(x * tileSize.X * 0.5f, y * tileSize.Y * 0.5f);

                    Texture2D texture = tileTextures[tile.TextureId];
                    Color colour = tile.Colour;

                    spriteBatch.Draw(texture, drawPosition, drawRectangle, colour);
                    spriteBatch.Draw(tileTextures[0], drawPosition, colour);

                    Log.AddWorldMessage("C:" + x + "," + y, drawPosition + new Vector2(8), camera);
                }
            }
        }

        //internal void OccupyTiles(Vector2 coordinates, GridObject occupant)
        //{
        //    Vector2 dimensions = occupant.Dimensions;

        //    for (int xTile = 0; xTile < dimensions.X; xTile++)
        //    {
        //        for (int yTile = 0; yTile < dimensions.Y; yTile++)
        //        {
        //            int xCoord = (int)coordinates.X - xTile;
        //            int yCoord = (int)coordinates.Y - yTile;

        //            tileGrid[xCoord, yCoord].Occupy(occupant);
        //        }
        //    }
        //}

        //internal void UnoccupyTiles(Vector2 coordinates, GridObject occupant)
        //{
        //    UnoccupyTiles(coordinates, occupant.Dimensions);
        //}

        //internal void UnoccupyTiles(Vector2 coordinates, Vector2 dimensions)
        //{
        //    for (int xTile = 0; xTile < dimensions.X; xTile++)
        //    {
        //        for (int yTile = 0; yTile < dimensions.Y; xTile++)
        //        {
        //            int xCoord = (int)coordinates.X - xTile;
        //            int yCoord = (int)coordinates.Y - yTile;

        //            tileGrid[xCoord, yCoord].Unoccupy();
        //        }
        //    }
        //}

        private void GenerateCells()
        {
            for (int i = 0; i < Dimensions.X; i++)
            {
                for (int j = 0; j < Dimensions.Y; j++)
                {
                    tileGrid[i, j] = new TerrainTile(1);
                }
            }
        }
        
        #endregion
    }   
}
