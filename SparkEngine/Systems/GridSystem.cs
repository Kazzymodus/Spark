namespace SparkEngine.Systems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Components;
    using SparkEngine.Input;
    using SparkEngine.Rendering;
    using SparkEngine.States;
    using SparkEngine.Utilities;

    class GridSystem : DrawSystem
    {
        private ComponentSystem[] subSystems;

        public GridSystem(params ComponentSystem[] subSystems)
            : base(typeof(Grid), typeof(Unit))
        {
            this.subSystems = subSystems;
        }

        public override void OnAddEntity(int entity, GameState state)
        {
            ComponentBatch components = state.GetAllComponentsOfEntity(entity);

            foreach (Grid grid in components)
            {
                
            }
        }

        public override void UpdateIndividual(GameState state, GameTime gameTime, InputHandler input, ComponentBatch components)
        {
            Grid grid = components.GetComponent<Grid>();

            if (grid is ArrayGrid)
            {
                UpdateArrayGrid((ArrayGrid)grid, state, gameTime, input);
            }
            else if (grid is BatchedGrid)
            {
                UpdateBatchedGrid((BatchedGrid)grid, state, gameTime, input);
            }
        }

        private void UpdateArrayGrid(ArrayGrid grid, GameState state, GameTime gameTime, InputHandler input)
        {
            ProtoEntity stamp = grid.Cells[0, 0];
            ComponentBatch stampComponents = stamp.Components;

            foreach (ComponentSystem system in subSystems)
            {
                if (!system.CanHostEntity(stampComponents))
                {
                    continue;
                }
                
                ProtoEntity[,] allCells = grid.Cells;
                ProtoEntity cell;

                for (int x = 0; x < grid.Width; x++)
                {
                    for (int y = 0; y < grid.Height; y++)
                    {
                        cell = allCells[x, y];

                        system.UpdateIndividual(state, gameTime, input, cell.Components);
                    }
                }
            }
        }

        private void UpdateBatchedGrid(BatchedGrid grid, GameState state, GameTime gameTime, InputHandler input)
        {
            foreach (ComponentSystem system in subSystems)
            {

                foreach (CellBatch batch in grid.Batches)
                {
                    ComponentBatch stampComponents = batch.Stamp.Components;

                    if (!system.CanHostEntity(stampComponents))
                    {
                        continue;
                    }

                    system.UpdateIndividual(state, gameTime, input, stampComponents);
                }
            }
        }

        public override void DrawAll(GameState state, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            base.DrawAll(state, graphicsDevice, spriteBatch);
        }

        public override void DrawIndividual(GameState state, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Vector2 cameraPosition, ComponentBatch components)
        {
            components.GetComponentsMultiType(out Grid grid, out Unit unit);
            Point viewportSize = new Point(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);

            ProtoEntity[,] drawGrid = ConvertToDrawableGrid(grid);

            switch (grid.Perspective)
            {
                case Perspective.Standard:
                    DrawStandardGrid(grid, unit, state, spriteBatch, cameraPosition, viewportSize);
                    break;
                case Perspective.Isometric:
                    DrawIsometricGrid(grid, unit, state, spriteBatch, cameraPosition, viewportSize);
                    break;
            }
        }

        private void DrawStandardGrid(Grid grid, Unit unit, ProtoEntity[,] drawGrid, GameState state, SpriteBatch spriteBatch, Vector2 cameraPosition, Point viewportSize)
        {
            DrawLayer layer = state.DrawLayers[grid.DrawLayer];
            Vector2 gridPosition = cameraPosition + GetDrawPosition(layer, grid);
            Rectangle visibleCoordinates = GetVisibleCartesianCoordinates(cameraPosition, viewportSize, unit, 0);

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

                        Texture2D texture = ((ComponentBatch)tile.Components).GetComponent<Sprite>();
                        Color colour = tile.Colour;

                        spriteBatch.Draw(texture, drawPosition, drawRectangle, colour);
                    }
                }
            }
        }

        private void DrawIsometricGrid(Grid grid, Unit unit, GameState state, SpriteBatch spriteBatch, Vector2 cameraPosition, Point viewportSize)
        {
            DrawLayer layer = state.DrawLayers[grid.DrawLayer];
            Vector2 gridPosition = cameraPosition + GetDrawPosition(layer, grid);

            Rectangle visibleCoordinates = GetVisibleIsometricCoordinates(cameraPosition, viewportSize, unit, 0);

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

                    ProtoEntity tile = tileGrid[coordinate.X, coordinate.Y];
                    Vector2 drawPosition = gridPosition + new Vector2(x * TileSize.X * 0.5f, y * TileSize.Y * 0.5f);

                    Texture2D texture = tileTextures[tile.TextureId];
                    Color colour = tile.Colour;

                    spriteBatch.Draw(texture, drawPosition, drawRectangle, colour);
                    spriteBatch.Draw(tileTextures[0], drawPosition, colour);

                    Log.AddWorldMessage("C:" + x + "," + y, drawPosition + new Vector2(8), camera);
                }
            }
        }

        private ProtoEntity[,] ConvertToDrawableGrid(Grid grid, Unit unit, GameState state, SpriteBatch spriteBatch, Vector2 cameraPosition, Point viewportSize)
        {
            DrawLayer layer = state.DrawLayers[grid.DrawLayer];
            Vector2 gridPosition = cameraPosition + GetDrawPosition(layer, grid);

            Rectangle visibleCoordinates = GetOffsetVisibleCoordinates(grid, unit, gridPosition, cameraPosition, viewportSize);
            
            // So far so good.

            Point startCoordinate = visibleCoordinates.Location;
            Point endCoordinate = startCoordinate + visibleCoordinates.Size;

            if (!grid.WrapAround)
            {
                if (grid.Perspective == Perspective.Isometric)
                {
                    startCoordinate = Projector.IsometricToCartesian(startCoordinate);
                    endCoordinate = Projector.IsometricToCartesian(endCoordinate);
                }

                startCoordinate.X = MathHelper.Clamp(startCoordinate.X, 0, grid.Width);
                startCoordinate.Y = MathHelper.Clamp(startCoordinate.Y, 0, grid.Height);
                endCoordinate.X = MathHelper.Clamp(endCoordinate.X, 0, grid.Width);
                endCoordinate.Y = MathHelper.Clamp(endCoordinate.Y, 0, grid.Height);

                if (grid.Perspective == Perspective.Isometric)
                {
                    startCoordinate = Projector.CartesianToIsometric(startCoordinate);
                    endCoordinate = Projector.CartesianToIsometric(endCoordinate);
                }
            }

            int frameX = (int)unit.LengthX * 0; // camera.Rotations;
            Rectangle drawRectangle = new Rectangle(frameX, 0, (int)unit.LengthX, (int)unit.LengthY);

            for (int x = startCoordinate.X; x < endCoordinate.X; x++)
            {
                for (int y = startCoordinate.Y; y < endCoordinate.Y; y ++)
                {
                    int tileX;
                    int tileY;

                    if (grid.Perspective == Perspective.Standard)
                    {
                        tileX = x;
                        tileY = y;
                    }
                    else
                    {
                        if ((x + y % 2) != 0)
                        {
                            continue;
                        }

                        Point index = Projector.IsometricToCartesian(new Point(x, y));
                        tileX = index.X;
                        tileY = index.Y;
                    }

                    if (grid.WrapAround)
                    {
                        bool isNegative = tileX < 0;
                        tileX %= grid.Width;

                        if (isNegative)
                        {
                            tileX = -tileX;
                        }

                        isNegative = tileY < 0;
                        tileY %= grid.Height;

                        if (isNegative)
                        {
                            tileY = -tileY;
                        }
                    }

                    ProtoEntity tile = drawGrid[tileX, tileY];

                    if (tile != null)
                    {
                        Vector2 drawPosition = gridPosition + new Vector2(x * unit.LengthX, y * unit.LengthY);

                        Texture2D texture = ((ComponentBatch)tile.Components).GetComponent<Sprite>();
                        Color colour = tile.Colour;

                        spriteBatch.Draw(texture, drawPosition, drawRectangle, colour);
                    }
                }
            }


            if (grid is ArrayGrid)
            {
                return ConvertToDrawableGrid((ArrayGrid)grid);
            }
            
            if (grid is BatchedGrid)
            {
                return ConvertToDrawableGrid((BatchedGrid)grid);
            }

            return null;
        }

        private ProtoEntity[,] ConvertToDrawableGrid(ArrayGrid grid, Rectangle visibleCoordinates)
        {
            ProtoEntity[,] drawGrid = new ProtoEntity[visibleCoordinates.Width, visibleCoordinates.Height];

            Point startCoordinate = visibleCoordinates.Location;
            Point endCoordinate = startCoordinate + visibleCoordinates.Size;

            if (grid.WrapAround)
            {
                startCoordinate.X = MathHelper.Clamp(startCoordinate.X, 0, grid.Width);
                startCoordinate.Y = MathHelper.Clamp(startCoordinate.Y, 0, grid.Height);
                endCoordinate.X = MathHelper.Clamp(endCoordinate.X, 0, grid.Width);
                endCoordinate.Y = MathHelper.Clamp(endCoordinate.Y, 0, grid.Height);

                for (int x = startCoordinate.X; x < endCoordinate.X; x++)
                {
                    for (int y = startCoordinate.Y; y < endCoordinate.Y; y++)
                    {
                        drawGrid[x, y] = grid.Cells[x, y];
                    }
                }
            }
            else
            {
                for (int x = startCoordinate.X; x < endCoordinate.X; x++)
                {
                    for (int y = startCoordinate.Y; y < endCoordinate.Y; y++)
                    {
                        int xIndex = x;
                        int yIndex = y;

                        bool isNegative = xIndex < 0;
                        xIndex %= grid.Width;

                        if (isNegative)
                        {
                            xIndex = -xIndex;
                        }

                        isNegative = yIndex < 0;
                        yIndex %= grid.Height;

                        if (isNegative)
                        {
                            yIndex = -yIndex;
                        }

                        drawGrid[xIndex, yIndex] = grid.Cells[x, y];
                    }
                }
            }
            
            return grid.Cells;
        }

        private ProtoEntity[,] ConvertToDrawableGrid(BatchedGrid grid, Rectangle visibleCoordinates)
        {
            ProtoEntity[,] drawGrid = new ProtoEntity[visibleCoordinates.Width, visibleCoordinates.Height];

            //Point startCoordinate = visibleCoordinates.Location;
            //Point endCoordinate = startCoordinate + visibleCoordinates.Size;
            
            for (int i = 0; i < grid.Batches.Length; i++)
            {
                CellBatch batch = grid.Batches[i];

                if (batch.Bounds.Intersects(visibleCoordinates))
                {
                    if (batch is BitMapBatch)
                    {
                        ParseBitMapBatch((BitMapBatch)batch, ref drawGrid, visibleCoordinates);

                        
                    }
                }
            }

            return drawGrid;
        }

        private ProtoEntity[,] ParseBitMapBatch(BitMapBatch batch, ref ProtoEntity[,] grid, Rectangle bounds)
        {
            Point endCoordinate = new Point(bounds.X + bounds.Width, bounds.Y + bounds.Height);
            Type bitMapType = batch.BitMap.BitMapType();

            BitMap bitMap = batch.BitMap;


            int endX = Math.Min(batch.X + batch.Width, endCoordinate.X);
            int endY = Math.Min(batch.Y + batch.Height, endCoordinate.Y);

            for (int x = 0; x < endX; x++)
            {
                for (int y = 0; y < endY; y++)
                {
                    bitMap.
                }
            }
        }

        private Rectangle GetOffsetVisibleCoordinates(Grid grid, Unit unit, Vector2 gridPosition, Vector2 cameraPosition, Point viewportSize)
        {
            Rectangle visibleCoordinates;
            Point offset;

            switch (grid.Perspective)
            {
                case Perspective.Standard:
                    visibleCoordinates = GetVisibleCartesianCoordinates(cameraPosition, viewportSize, unit, 0);
                    offset = Projector.PixelsToCartesian(gridPosition, unit).ToPoint();
                    break;
                case Perspective.Isometric:
                    visibleCoordinates = GetVisibleIsometricCoordinates(cameraPosition, viewportSize, unit, 0);
                    offset = Projector.PixelsToIsometric(gridPosition, unit).ToPoint();
                    break;
                default:
                    throw new InvalidOperationException($"Can not process {grid.Perspective} perspective.");
            }

            visibleCoordinates.Location -= offset; // Is this right?

            return visibleCoordinates;
        }

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
