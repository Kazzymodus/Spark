namespace SparkEngine.Components
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Input;
    using SparkEngine.Rendering;
    using SparkEngine.States;

    public class Grid : Component
    {
        /*
        #region Fields

        private const int TileDrawPadding = 2;
        private const int MaxTextures = 256;

        private T[,] tileGrid;
        private Texture2D[] tileTextures;

        private readonly Action<SpriteBatch, Camera, DrawLayer> DrawMethod;

        #endregion

        #region Constructors

        private Grid(TileMode tileMode, Vector2 position, Vector2 tileSize, bool wrapsAround, params Texture2D[] tileTextures)
        {
            Position = position;
            WrapsAround = wrapsAround;
            TileSize = tileSize;
            this.tileTextures = tileTextures;

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

        public Point Dimensions
        {
            get { return new Point(tileGrid.GetLength(0), tileGrid.GetLength(1)); }
        }

        public Vector2 TileSize { get; }

        public CellData CellData { get; }

        public bool WrapsAround { get; }

        public LayerSortMethod LayerSortMethod { get; } = LayerSortMethod.First;

        #endregion

        #region Methods

        public static Grid<T> CreateSquareGrid(Vector2 position, Vector2 tileSize, bool wrapsAround, params Texture2D[] textures)
        {
            Grid<T> grid = new Grid<T>(TileMode.Square, position, tileSize, wrapsAround, textures);
            return grid;
        }
        
        public static Grid<T> CreateIsometricGrid(Vector2 position, Vector2 tileSize, bool wrapsAround, params Texture2D[] textures)
        {
            Grid<T> grid = new Grid<T>(TileMode.Isometric, position, tileSize, wrapsAround, textures);
            return grid;
        }

        public Vector2 GetDrawPosition(Camera camera, DrawLayer drawLayer)
        {
            return drawLayer.Position + Position * drawLayer.Unit;
        }

        public Rectangle GetBounds(Camera camera, DrawLayer drawLayer)
        {
            return new Rectangle(camera.Position.ToPoint(), camera.ViewportSize);
        }

        /// <summary>
        /// Get the cell corresponding to the passed coordinates.
        /// </summary>
        /// <param name="coordinates">The coordinates of the cell.</param>
        /// <returns></returns>
        public T GetTile(Point coordinates)
        {
            return GetTile(coordinates.X, coordinates.Y);
        }

        /// <summary>
        /// Get the cell corresponding to the passed coordinates.
        /// </summary>
        /// <param name="coordinates">The coordinates of the cell.</param>
        /// <returns></returns>
        public T GetTile(Vector2 coordinates)
        {
            return GetTile((int)coordinates.X, (int)coordinates.Y);
        }

        public T GetTile(int x, int y)
        {
            return tileGrid[x, y];
        }

        public override void ProcessInput(InputHandler input, GameState state, GameTime gameTime, bool underCursor)
        {
            Point cursorCoordinates = (state.Camera.GetMouseWorldPosition(input.MouseScreenPositionVector) / TileSize).ToPoint(); // FIX THIS

            if (WrapsAround)
            {
                bool isNegative = cursorCoordinates.X < 0;
                cursorCoordinates.X = Math.Abs(cursorCoordinates.X) % Dimensions.X;
                if (isNegative)
                {
                    cursorCoordinates.X = -cursorCoordinates.X;
                }

                isNegative = cursorCoordinates.Y < 0;
                cursorCoordinates.Y = Math.Abs(cursorCoordinates.Y) % Dimensions.Y;
                if (isNegative)
                {
                    cursorCoordinates.Y = -cursorCoordinates.Y;
                }
            }

            T cursorTile = null;

            if (IsWithinBounds(cursorCoordinates))
            {
                cursorTile = tileGrid[cursorCoordinates.X, cursorCoordinates.Y];
            }

            if (cursorTile != null && input.IsMousePressed(MouseButtons.LMB))
            {
                cursorTile.SetColour(Color.Red);
            }
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

        public void Draw(SpriteBatch spriteBatch, Camera camera, DrawLayer drawLayer)
        {
            DrawMethod(spriteBatch, camera, drawLayer);            
        }

        public void SquareDraw(SpriteBatch spriteBatch, Camera camera, DrawLayer drawLayer)
        {
            Vector2 gridPosition = GetDrawPosition(camera, drawLayer);
            Rectangle visibleCoordinates = camera.GetVisibleCartesianCoordinates(TileSize, TileDrawPadding);

            Point offset = Projector.PixelsToCartesian(gridPosition, TileSize).ToPoint();
            Point startCoordinate = visibleCoordinates.Location - offset;
            Point endCoordinate = startCoordinate + visibleCoordinates.Size;

            Log.AddListMessage(startCoordinate.ToString());
            Log.AddListMessage(endCoordinate.ToString());

            if (!WrapsAround)
            {
                startCoordinate.X = MathHelper.Clamp(startCoordinate.X, 0, Dimensions.X);
                startCoordinate.Y = MathHelper.Clamp(startCoordinate.Y, 0, Dimensions.Y);
                endCoordinate.X = MathHelper.Clamp(endCoordinate.X, 0, Dimensions.X);
                endCoordinate.Y = MathHelper.Clamp(endCoordinate.Y, 0, Dimensions.Y);
            }

            int frameX = (int)TileSize.X * camera.Rotations;
            Rectangle drawRectangle = new Rectangle(frameX, 0, (int)TileSize.X, (int)TileSize.Y);

            for (int x = startCoordinate.X; x < endCoordinate.X; x++)
            {
                for (int y = startCoordinate.Y; y < endCoordinate.Y; y++)
                {
                    int tileX = x;
                    int tileY = y;

                    if (WrapsAround)
                    {
                        bool isNegative = tileX < 0;
                        tileX %= Math.Abs(Dimensions.X);

                        if (isNegative)
                        {
                            tileX = -tileX;
                        }

                        isNegative = tileY < 0;
                        tileY %= Math.Abs(Dimensions.Y);

                        if (isNegative)
                        {
                            tileY = -tileY;
                        }
                    }

                    GridCell tile = tileGrid[tileX, tileY];
                    Vector2 drawPosition = gridPosition + new Vector2(x * TileSize.X, y * TileSize.Y);

                    Texture2D texture = tileTextures[tile.TextureId];
                    Color colour = tile.Colour;

                    spriteBatch.Draw(texture, drawPosition, drawRectangle, colour);
                }
            }
        }

        public void IsometricDraw(SpriteBatch spriteBatch, Camera camera, DrawLayer drawLayer)
        {
            Vector2 gridPosition = GetDrawPosition(camera, drawLayer);

            Rectangle visibleCoordinates = camera.GetVisibleIsometricCoordinates(TileSize, TileDrawPadding);

            Point offset = Projector.PixelsToIsometric(gridPosition, TileSize).ToPoint();
            Point startCoordinate = visibleCoordinates.Location - offset;
            Point endCoordinate = startCoordinate + visibleCoordinates.Size;

            int frameX = (int)TileSize.X * camera.Rotations;
            Rectangle drawRectangle = new Rectangle(frameX, 0, (int)TileSize.X, (int)TileSize.Y);

            for (int x = startCoordinate.X; x < endCoordinate.X; x++)
            {
                for (int y = startCoordinate.Y; y < endCoordinate.Y; y++)
                {
                    Point coordinate = Projector.IsometricToCartesian(new Point(x, y));

                    if ((x + y) % 2 != 0 || (!WrapsAround && !IsWithinBounds(coordinate)))
                    {
                        continue;
                    }

                    if (WrapsAround)
                    {
                        if (coordinate.X < 0)
                        {
                            coordinate.X = Dimensions.X + coordinate.X;
                        }
                        else if (coordinate.X >= Dimensions.X)
                        {
                            coordinate.X -= Dimensions.X;
                        }

                        if (coordinate.Y < 0)
                        {
                            coordinate.Y = Dimensions.Y + coordinate.Y;
                        }
                        else if (coordinate.Y >= Dimensions.Y)
                        {
                            coordinate.Y -= Dimensions.Y;
                        }
                    }

                    GridCell tile = tileGrid[coordinate.X, coordinate.Y];
                    Vector2 drawPosition = gridPosition + new Vector2(x * TileSize.X * 0.5f, y * TileSize.Y * 0.5f);

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

        public void SetGrid(T[,] cellGrid)
        {
            tileGrid = cellGrid;
        }
        
        #endregion
        */
    }   
}
