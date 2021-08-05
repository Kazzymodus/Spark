using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SparkEngine.Components;
using SparkEngine.Entities;
using SparkEngine.States;
using SparkEngine.Utilities;

namespace SparkEngine.Systems
{
    public class WrappingScreenGridSystem : ComponentSystem<WrappingScreenGrid>, IDrawSystem
    {
        public WrappingScreenGridSystem(int maxSubs = GameState.MaxEntities)
            : base(maxSubs)
        {
        }

        public void Draw(DrawInfo drawInfo)
        {
            var cameraPosition = (Vector2) drawInfo.State.CameraPosition;
            var layerOffsets = drawInfo.State.DrawLayers.GetLayerOffsets();

            var grids = Subscribers.GetComponentsCompact();

            drawInfo.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, drawInfo.CameraTransform);

            for (var i = 0; i < grids.Length; i++) DrawGrid(grids[i], drawInfo, cameraPosition, layerOffsets);

            drawInfo.SpriteBatch.End();
        }

        protected internal override void UpdateComponent(ref WrappingScreenGrid grid, int index, UpdateInfo updateInfo)
        {
            //Sprite[,] cells = grid.Cells;

            //for (int x = 0; x < cells.GetLength(0); x++)
            //{
            //    for (int y = 0; y < cells.GetLength(1); y++)
            //    {
            //        cells[x, y];
            //    }
            //}
        }

        protected static Point GetCursorTile(Vector2 gridPosition, Vector2 unit, Vector2 cameraPosition,
            Vector2 cursorPosition)
        {
            var tile = -gridPosition + cameraPosition + cursorPosition;
            tile.X /= unit.X;
            if (tile.X < 0) tile.X--;
            tile.Y /= unit.Y;
            if (tile.Y < 0) tile.Y--;

            return tile.ToPoint();
        }

        public static WrappingScreenGrid CreateScreenFillingGrid(Texture2D texture, Point screenSize, Vector2 cellSize,
            Perspective perspective)
        {
            if (perspective == Perspective.Isometric)
                throw new NotImplementedException();

            var width = (int) (screenSize.X / cellSize.X + 0.5f) - 2;
            var height = (int) (screenSize.Y / cellSize.Y + 0.5f) + 1;

            return new WrappingScreenGrid(texture, width, height, cellSize, perspective);

            //Random rand = new Random();
            //byte[,] frames = new byte[width, height];
            //for(int x = 0; x < width; x++)
            //{
            //    for (int y= 0; y < height; y++)
            //    {
            //        frames[x, y] = (byte)rand.Next(10);
            //    }
            //}
            //return new ScreenTextureGrid(texture, frames, cellSize, perspective);
        }

        private void DrawGrid(WrappingScreenGrid grid, DrawInfo drawInfo, Vector2 cameraPosition,
            Vector2[] layerOffsets)
        {
            var xOverflow = cameraPosition.X % grid.CellSize.X;
            var yOverflow = cameraPosition.Y % grid.CellSize.Y;

            var startPosition = cameraPosition - new Vector2(xOverflow, yOverflow); // - grid.CellSize;
            var spriteBatch = drawInfo.SpriteBatch;

            var sourceRectangles = new Rectangle[(int) (grid.Texture.Width / grid.CellSize.X)];

            for (var i = 0; i < sourceRectangles.Length; i++)
                sourceRectangles[i] =
                    new Rectangle((int) grid.CellSize.X * i, 0, (int) grid.CellSize.X, (int) grid.CellSize.Y);

            if (grid.Perspective == Perspective.Standard)
            {
                var tileOffset = startPosition;

                var xIndex = grid.XHead;
                var yIndex = grid.YHead;

                for (var x = 0; x < grid.Width; x++)
                {
                    for (var y = 0; y < grid.Height; y++)
                    {
                        spriteBatch.Draw(grid.Texture, tileOffset, sourceRectangles[grid.FrameGrid[xIndex, yIndex]],
                            Color.White /*new Color(xIndex / (float)grid.Width, 1f, 1f)*/);
                        //DrawCell(grid[x, y], spriteBatch, tileOffset);
                        tileOffset.Y += grid.CellSize.Y;
                        if (++yIndex >= grid.Height) yIndex = 0;
                    }

                    tileOffset.X += grid.CellSize.X;
                    tileOffset.Y = startPosition.Y;
                    if (++xIndex >= grid.Width)
                        xIndex = 0;
                    //return;
                }
            }

            /*

            Point viewportSize = new Point(drawInfo.GraphicsDevice.Viewport.Width, drawInfo.GraphicsDevice.Viewport.Height);

            Vector2 layerOffset = layerOffsets[grid.DrawLayer];

            Rectangle visibleCoordinates = GetVisibleTiles(grid, cameraPosition, viewportSize, layerOffset);

            Point startCoordinate = visibleCoordinates.Location;
            Point endCoordinate = startCoordinate + visibleCoordinates.Size;

            Vector2 startPosition = grid.Position + layerOffset;
            SpriteBatch spriteBatch = drawInfo.SpriteBatch;

            if (grid.Perspective == Perspective.Standard)
            {
                if (!grid.WrapAround)
                {
                    startCoordinate.X = MathHelper.Clamp(startCoordinate.X, 0, grid.Width);
                    startCoordinate.Y = MathHelper.Clamp(startCoordinate.Y, 0, grid.Height);
                    endCoordinate.X = MathHelper.Clamp(endCoordinate.X, 0, grid.Width);
                    endCoordinate.Y = MathHelper.Clamp(endCoordinate.Y, 0, grid.Height);

                    Vector2 tileOffset = startPosition + new Vector2(startCoordinate.X * grid.TileSize.X, startCoordinate.Y * grid.TileSize.Y);

                    for (int x = startCoordinate.X; x < endCoordinate.X; x++)
                    {
                        for (int y = startCoordinate.Y; y < endCoordinate.Y; y++)
                        {
                            DrawCell(grid[x, y], spriteBatch, tileOffset);
                            tileOffset.Y += grid.TileSize.Y;
                        }

                        tileOffset.X += grid.TileSize.X;
                        tileOffset.Y = startPosition.Y + startCoordinate.Y * grid.TileSize.Y;
                    }

                }
                else
                {
                    for (int x = startCoordinate.X; x < endCoordinate.X; x++)
                    {
                        for (int y = startCoordinate.Y; y < endCoordinate.Y; y++)
                        {
                            int xIndex = x % grid.Width;
                            int yIndex = y % grid.Height;

                            if (xIndex < 0)
                            {
                                xIndex += grid.Width;
                            }

                            if (yIndex < 0)
                            {
                                yIndex += grid.Height;
                            }

                            DrawCell(grid[xIndex, yIndex], spriteBatch, startPosition + new Vector2(grid.TileSize.X * x, grid.TileSize.Y * y));
                        }
                    }
                }
            }
            */
        }

        private void DrawScreenGrid(ArrayGrid<Sprite> grid, DrawInfo drawInfo, Vector2 cameraPosition)
        {
        }

        private void DrawCell(Sprite sprite, SpriteBatch spriteBatch, Vector2 offset)
        {
            Rectangle sourceRectangle;

            if (sprite.IsAnimated)
            {
                var frameWidth = (int) sprite.FrameSize.X;
                var frameHeight = (int) sprite.FrameSize.Y;
                sourceRectangle = new Rectangle(sprite.FrameX * frameWidth, sprite.FrameY * frameHeight, frameWidth,
                    frameHeight);
            }
            else
            {
                sourceRectangle = new Rectangle(Point.Zero, sprite.FrameSize.ToPoint());
            }

            spriteBatch.Draw(sprite.Texture, sprite.DrawPosition + offset, sourceRectangle, sprite.ColorMask);
        }

        private Rectangle GetVisibleTiles(ArrayGrid<Sprite> grid, Vector2 cameraPosition, Point viewportSize,
            Vector2 layerOffset)
        {
            var gridPosition = grid.Position + layerOffset;

            Rectangle visibleCoordinates;
            Point offset;

            switch (grid.Perspective)
            {
                case Perspective.Standard:
                    visibleCoordinates = GetVisibleCartesianCoordinates(cameraPosition, viewportSize, grid.TileSize, 1);
                    offset = Projector.PixelsToCartesian(gridPosition, grid.TileSize).ToPoint();
                    break;
                case Perspective.Isometric:
                    visibleCoordinates = GetVisibleIsometricCoordinates(cameraPosition, viewportSize, grid.TileSize, 1);
                    offset = Projector.PixelsToIsometric(gridPosition, grid.TileSize).ToPoint();
                    break;
                default:
                    throw new InvalidOperationException($"Can not process {grid.Perspective} perspective.");
            }

            visibleCoordinates.Location -= offset; // Is this right?

            return visibleCoordinates;
        }

        /// <summary>
        ///     Gets the range of coordinates currently in camera view.
        /// </summary>
        /// <returns>A rectangle containing all visible coordinates.</returns>
        protected static Rectangle GetVisibleCartesianCoordinates(Vector2 cameraPosition, Point viewportSize, Vector2 unit,
            int padding = 0)
        {
            var location = Projector.PixelsToCartesian(cameraPosition.ToPoint(), unit) - new Point(padding);
            var size = Projector.PixelsToCartesian(viewportSize, unit) + new Point(padding * 2);

            return new Rectangle(location, size);
        }

        protected static Rectangle GetVisibleIsometricCoordinates(Vector2 cameraPosition, Point viewportSize, Vector2 unit,
            int padding = 0)
        {
            var location = Projector.PixelsToIsometric(cameraPosition, unit).ToPoint() - new Point(padding);
            var size = Projector.PixelsToIsometric(viewportSize, unit) + new Point(padding * 2);

            return new Rectangle(location, size);
        }

        private ProtoEntity[,] ConvertToDrawableGrid(BatchedGrid grid, Rectangle visibleCoordinates)
        {
            throw new NotImplementedException();

            /*

            ProtoEntity[,] drawGrid = new ProtoEntity[visibleCoordinates.Width, visibleCoordinates.Height];

            //Point startCoordinate = visibleCoordinates.Location;
            //Point endCoordinate = startCoordinate + visibleCoordinates.Size;

            for (int i = 0; i < grid.Batches.Length; i++)
            {
                CellBatch batch = grid.Batches[i];

                //if (batch.Bounds.Intersects(visibleCoordinates))
                //{
                //    if (batch is BitMapBatch)
                //    {
                //        ParseBitMapBatch((BitMapBatch)batch, ref drawGrid, visibleCoordinates);


                //    }
                //}
            }

            return drawGrid;
            
            */
        }

        /*

        private void DrawStandardGrid(Grid grid, Unit unit, ProtoEntity[,] drawGrid, GameState state, SpriteBatch spriteBatch, Point viewportSize)
        {
            DrawLayer layer = state.DrawLayers[grid.DrawLayer];
            Vector2 gridPosition = GetDrawPosition(layer, grid);
            Rectangle visibleCoordinates = GetVisibleCartesianCoordinates(Vector2.Zero, viewportSize, unit, 0); // TEMP

            Point offset = Projector.PixelsToCartesian(gridPosition, unit).ToPoint();
            Point startCoordinate = visibleCoordinates.Location - offset;
            Point endCoordinate = startCoordinate + visibleCoordinates.Size;

            if (!grid.WrapAround)
            {
                startCoordinate.X = MathHelper.Clamp(startCoordinate.X, 0, grid.Width);
                startCoordinate.Y = MathHelper.Clamp(startCoordinate.Y, 0, grid.Height);
                endCoordinate.X = MathHelper.Clamp(endCoordinate.X, 0, grid.Width);
                endCoordinate.Y = MathHelper.Clamp(endCoordinate.Y, 0, grid.Height);
            }

            int frameX = (int)unit.LengthX * 0; // camera.Rotations;
            Rectangle drawRectangle = new Rectangle(frameX, 0, (int)unit.LengthX, (int)unit.LengthY);

            for (int x = startCoordinate.X; x < endCoordinate.X; x++)
            {
                for (int y = startCoordinate.Y; y < endCoordinate.Y; y++)
                {
                    int tileX = x;
                    int tileY = y;

                    if (grid.WrapAround)
                    {
                        bool isNegative = tileX < 0;
                        tileX %= Math.Abs(grid.Width);

                        if (isNegative)
                        {
                            tileX = -tileX;
                        }

                        isNegative = tileY < 0;
                        tileY %= Math.Abs(grid.Height);

                        if (isNegative)
                        {
                            tileY = -tileY;
                        }
                    }

                    ProtoEntity tile = drawGrid[tileX, tileY];

                    if (tile != null)
                    {
                        Vector2 drawPosition = gridPosition + new Vector2(x * unit.LengthX, y * unit.LengthY);

                        //Texture2D texture = ((ComponentBatch)tile.Components).GetComponent<Sprite>();
                        //Color colour = tile.Colour;

                        //spriteBatch.Draw(texture, drawPosition, drawRectangle, colour);
                    }
                }
            }
        }

        private void DrawIsometricGrid(Grid grid, Unit unit, GameState state, SpriteBatch spriteBatch, Point viewportSize)
        {
            DrawLayer layer = state.DrawLayers[grid.DrawLayer];
            Vector2 gridPosition = GetDrawPosition(layer, grid);

            Rectangle visibleCoordinates = GetVisibleIsometricCoordinates(Vector2.Zero, viewportSize, unit, 0); // TEMP

            Point offset = Projector.PixelsToIsometric(gridPosition, unit).ToPoint();
            Point startCoordinate = visibleCoordinates.Location - offset;
            Point endCoordinate = startCoordinate + visibleCoordinates.Size;

            int frameX = (int)unit.LengthX * 0;// camera.Rotations;
            Rectangle drawRectangle = new Rectangle(frameX, 0, (int)unit.LengthX, (int)unit.LengthY);

            for (int x = startCoordinate.X; x < endCoordinate.X; x++)
            {
                for (int y = startCoordinate.Y; y < endCoordinate.Y; y++)
                {
                    Point coordinate = Projector.IsometricToCartesian(new Point(x, y));

                    if ((x + y) % 2 != 0 || (!grid.WrapAround && !grid.IsPointWithinBounds(coordinate)))
                    {
                        continue;
                    }

                    if (grid.WrapAround)
                    {
                        if (coordinate.X < 0)
                        {
                            coordinate.X = grid.Width + coordinate.X;
                        }
                        else if (coordinate.X >= grid.Width)
                        {
                            coordinate.X -= grid.Width;
                        }

                        if (coordinate.Y < 0)
                        {
                            coordinate.Y = grid.Height + coordinate.Y;
                        }
                        else if (coordinate.Y >= grid.Height)
                        {
                            coordinate.Y -= grid.Height;
                        }
                    }

                    //ProtoEntity tile = tileGrid[coordinate.X, coordinate.Y];
                    //Vector2 drawPosition = gridPosition + new Vector2(x * TileSize.X * 0.5f, y * TileSize.Y * 0.5f);

                    //Texture2D texture = tileTextures[tile.TextureId];
                    //Color colour = tile.Colour;

                    //spriteBatch.Draw(texture, drawPosition, drawRectangle, colour);
                    //spriteBatch.Draw(tileTextures[0], drawPosition, colour);

                    //Log.AddWorldMessage("C:" + x + "," + y, drawPosition + new Vector2(8), camera);
                }
            }
        }

        private ProtoEntity[,] ConvertToDrawableGrid(Grid grid, Unit unit, GameState state, SpriteBatch spriteBatch, Point viewportSize)
        {
            DrawLayer layer = state.DrawLayers[grid.DrawLayer];
            Vector2 gridPosition = GetDrawPosition(layer, grid);

            Rectangle visibleCoordinates = GetOffsetVisibleCoordinates(grid, unit, gridPosition, state.CameraPosition, viewportSize);

            // So far so good.

            //Point startCoordinate = visibleCoordinates.Location;
            //Point endCoordinate = startCoordinate + visibleCoordinates.Size;

            //if (!grid.WrapAround)
            //{
            //    if (grid.Perspective == Perspective.Isometric)
            //    {
            //        startCoordinate = Projector.IsometricToCartesian(startCoordinate);
            //        endCoordinate = Projector.IsometricToCartesian(endCoordinate);
            //    }

            //    startCoordinate.X = MathHelper.Clamp(startCoordinate.X, 0, grid.Width);
            //    startCoordinate.Y = MathHelper.Clamp(startCoordinate.Y, 0, grid.Height);
            //    endCoordinate.X = MathHelper.Clamp(endCoordinate.X, 0, grid.Width);
            //    endCoordinate.Y = MathHelper.Clamp(endCoordinate.Y, 0, grid.Height);

            //    if (grid.Perspective == Perspective.Isometric)
            //    {
            //        startCoordinate = Projector.CartesianToIsometric(startCoordinate);
            //        endCoordinate = Projector.CartesianToIsometric(endCoordinate);
            //    }
            //}

            //int frameX = (int)unit.LengthX * 0; // camera.Rotations;
            //Rectangle drawRectangle = new Rectangle(frameX, 0, (int)unit.LengthX, (int)unit.LengthY);

            //for (int x = startCoordinate.X; x < endCoordinate.X; x++)
            //{
            //    for (int y = startCoordinate.Y; y < endCoordinate.Y; y++)
            //    {
            //        int tileX;
            //        int tileY;

            //        if (grid.Perspective == Perspective.Standard)
            //        {
            //            tileX = x;
            //            tileY = y;
            //        }
            //        else
            //        {
            //            if ((x + y % 2) != 0)
            //            {
            //                continue;
            //            }

            //            Point index = Projector.IsometricToCartesian(new Point(x, y));
            //            tileX = index.X;
            //            tileY = index.Y;
            //        }

            //        if (grid.WrapAround)
            //        {
            //            bool isNegative = tileX < 0;
            //            tileX %= grid.Width;

            //            if (isNegative)
            //            {
            //                tileX = -tileX;
            //            }

            //            isNegative = tileY < 0;
            //            tileY %= grid.Height;

            //            if (isNegative)
            //            {
            //                tileY = -tileY;
            //            }
            //        }

            //        ProtoEntity tile = drawGrid[tileX, tileY];

            //        if (tile != null)
            //        {
            //            Vector2 drawPosition = gridPosition + new Vector2(x * unit.LengthX, y * unit.LengthY);

            //            //Texture2D texture = ((ComponentBatch)tile.Components).GetComponent<Sprite>();
            //            //Color colour = tile.Colour;

            //            //spriteBatch.Draw(texture, drawPosition, drawRectangle, colour);
            //        }
            //    }
            //}


            if (grid is ArrayGrid)
            {
                return null;// ConvertToDrawableGrid((ArrayGrid)grid, visibleCoordinates);
            }

            if (grid is BatchedGrid)
            {
                return ConvertToDrawableGrid((BatchedGrid)grid, visibleCoordinates);
            }

            throw new InvalidOperationException($"Grid of type {grid.ToString()} is not supported.");
        }

        private void DrawArrayGrid(ArrayGrid grid, Unit unit, GameState state, GraphicsDevice graphics, SpriteBatch spriteBatch, Rectangle visibleCoordinates)
        {
            Point startCoordinate = visibleCoordinates.Location;
            Point endCoordinate = startCoordinate + visibleCoordinates.Size;

            Vector2 gridPosition = GetDrawPosition(state.DrawLayers[grid.DrawLayer], grid);

            if (grid.Perspective == Perspective.Standard)
            {
                if (!grid.WrapAround)
                {
                    startCoordinate.X = MathHelper.Clamp(startCoordinate.X, 0, grid.Width);
                    startCoordinate.Y = MathHelper.Clamp(startCoordinate.Y, 0, grid.Height);
                    endCoordinate.X = MathHelper.Clamp(endCoordinate.X, 0, grid.Width);
                    endCoordinate.Y = MathHelper.Clamp(endCoordinate.Y, 0, grid.Height);

                    foreach (ComponentDrawMethod method in drawMethods)
                    {
                        for (int x = startCoordinate.X; x < endCoordinate.X; x++)
                        {
                            for (int y = startCoordinate.Y; y < endCoordinate.Y; y++)
                            {
                                method.Draw(state, graphics, spriteBatch, gridPosition + new Vector2(unit.LengthX * x, unit.LengthY * y), grid[x, y].Components);
                            }
                        }
                    }
                }
                else
                {
                    foreach (ComponentDrawMethod method in drawMethods)
                    {
                        for (int x = startCoordinate.X; x < endCoordinate.X; x++)
                        {
                            for (int y = startCoordinate.Y; y < endCoordinate.Y; y++)
                            {
                                int xIndex = x % grid.Width;
                                int yIndex = y % grid.Height;

                                if (xIndex < 0)
                                {
                                    xIndex += grid.Width;
                                }

                                if (yIndex < 0)
                                {
                                    yIndex += grid.Height;
                                }

                                method.Draw(state, graphics, spriteBatch, gridPosition + new Vector2(unit.LengthX * x, unit.LengthY * y), grid[xIndex, yIndex].Components);
                            }
                        }
                    }
                }
            }
        }

    */

        #region Methods

        //public override void ProcessInput(InputHandler input, GameState state, GameTime gameTime, bool underCursor)
        //{
        //    Point cursorCoordinates = (state.Camera.GetMouseWorldPosition(input.MouseScreenPositionVector) / TileSize).ToPoint(); // FIX THIS

        //    if (WrapsAround)
        //    {
        //        bool isNegative = cursorCoordinates.X < 0;
        //        cursorCoordinates.X = Math.Abs(cursorCoordinates.X) % Dimensions.X;
        //        if (isNegative)
        //        {
        //            cursorCoordinates.X = -cursorCoordinates.X;
        //        }

        //        isNegative = cursorCoordinates.Y < 0;
        //        cursorCoordinates.Y = Math.Abs(cursorCoordinates.Y) % Dimensions.Y;
        //        if (isNegative)
        //        {
        //            cursorCoordinates.Y = -cursorCoordinates.Y;
        //        }
        //    }

        //    T cursorTile = null;

        //    if (IsWithinBounds(cursorCoordinates))
        //    {
        //        cursorTile = tileGrid[cursorCoordinates.X, cursorCoordinates.Y];
        //    }

        //    if (cursorTile != null && input.IsMousePressed(MouseButtons.LMB))
        //    {
        //        cursorTile.SetColour(Color.Red);
        //    }
        //}

        //private static CellBatch GetBatchFromStamp(ProtoEntity stamp, BatchedGrid sampleGrid)
        //{
        //    return GetBatchFromStamp(stamp, sampleGrid, Point.Zero, sampleGrid.Dimensions);
        //}

        //private static CellBatch GetBatchFromStamp(ProtoEntity stamp, BatchedGrid sampleGrid, Point startPoint, Point sampleSize)
        //{
        //    List<Point> clones = new List<Point>();

        //    for (int i = startPoint.X; i < sampleSize.X; i++)
        //    {
        //        for (int j = startPoint.Y; j < startPoint.Y; j++)
        //        {
        //            if (sampleGrid.GetTile(i, j).Equals(stamp))
        //            {
        //                clones.Add(new Point(i, j));
        //            }
        //        }
        //    }

        //    int cloneAmount = clones.Count;
        //    int totalCells = sampleSize.X * sampleSize.Y;

        //    bool isStencil = false;

        //    if (cloneAmount > totalCells / 2)
        //    {
        //        isStencil = true;


        //    }

        //    return new CellBatch(stamp, startPoint, sampleSize, clones.ToArray(), isStencil);
        //}

        #endregion
    }
}