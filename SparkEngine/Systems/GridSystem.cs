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
    using SparkEngine.Systems.Batching;
    
    public class GridSystem : DrawSystem
    {
        private readonly List<ComponentUpdateMethod> updateMethods;
        private readonly List<ComponentDrawMethod> drawMethods;

        public GridSystem(params ComponentSystem[] systems)
            : base(false, typeof(Grid), typeof(Unit))
        {
            updateMethods = new List<ComponentUpdateMethod>();
            drawMethods = new List<ComponentDrawMethod>();

            for (int i = 0; i < systems.Length; i++)
            {
                if (systems[i] is DrawSystem drawSystem)
                {
                    drawMethods.Add(drawSystem.ExtractDraw());

                    if (drawSystem.NoUpdate)
                    {
                        continue;
                    }
                }

                updateMethods.Add(ExtractUpdate());                
            }
        }

        public override void OnAddEntity(int entity, GameState state)
        {
            ComponentBatch components = state.GetComponentsOfEntity<Grid>(entity);

            foreach (Grid grid in components)
            {
                if (grid is ArrayGrid arrayGrid && arrayGrid.IsHomogenous == null)
                {
                    List<Type> requiredTypes = new List<Type>();

                    for (int i = 0; i < updateMethods.Count + drawMethods.Count; i++)
                    {
                        Type[] requiredComponents;
                        
                        if (updateMethods.Count == 0)
                        {
                            requiredComponents = drawMethods[i].RequiredComponents;
                        }
                        else
                        {
                            requiredComponents = i < updateMethods.Count ? updateMethods[i].RequiredComponents : drawMethods[i % updateMethods.Count].RequiredComponents;
                        }                    

                        for (int j = 0; j < requiredComponents.Length; j++)
                        {
                            if (!requiredTypes.Contains(requiredComponents[j]))
                            {
                                requiredTypes.Add(requiredComponents[j]);
                            }
                        }
                    
                    }

                    arrayGrid.DetermineHomogenity(requiredTypes.ToArray());
                }
            }
        }

        public override void UpdateAll(GameState state, GameTime gameTime, InputHandler input)
        {
            if (updateMethods.Count == 0)
            {
                return;
            }

            base.UpdateAll(state, gameTime, input);
        }

        public override void UpdateIndividual(GameState state, GameTime gameTime, InputHandler input, ComponentBatch components)
        {
            Grid grid = components.GetComponent<Grid>();

            if (grid is ArrayGrid arrayGrid)
            {
                if (arrayGrid.IsHomogenous == true)
                {
                    UpdateArrayGridUnsafe(arrayGrid, state, gameTime, input);
                }
                else
                {
                    UpdateArrayGridSafe(arrayGrid, state, gameTime, input);
                }
            }
            else if (grid is BatchedGrid)
            {
                UpdateBatchedGrid((BatchedGrid)grid, state, gameTime, input);
            }
        }

        private void UpdateArrayGridUnsafe(ArrayGrid grid, GameState state, GameTime gameTime, InputHandler input)
        {
            ProtoEntity sampleCell = grid.Cells[0, 0];
            ComponentBatch stampComponents = sampleCell.Components;

            try
            {
                foreach (ComponentUpdateMethod method in updateMethods)
                {
                    if (!stampComponents.ContainsAll(method.RequiredComponents))
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

                            method.Update(state, gameTime, input, cell.Components);
                        }
                    }
                }
            }
            catch (NullReferenceException)
            {
                throw new InvalidOperationException("Array is not homogenous.");
            }
        }

        private void UpdateArrayGridSafe(ArrayGrid grid, GameState state, GameTime gameTime, InputHandler input)
        {
            foreach (ComponentUpdateMethod method in updateMethods)
            {
                ProtoEntity[,] allCells = grid.Cells;
                ProtoEntity cell;

                for (int x = 0; x < grid.Width; x++)
                {
                    for (int y = 0; y < grid.Height; y++)
                    {
                        cell = allCells[x, y];

                        if (((ComponentBatch)cell.Components).ContainsAll(RequiredComponents))
                        {
                            method.Update(state, gameTime, input, cell.Components);
                        }
                    }
                }
            }
        }

        private void UpdateBatchedGrid(BatchedGrid grid, GameState state, GameTime gameTime, InputHandler input)
        {
            throw new NotImplementedException();

            //foreach (ComponentSystem system in subSystems)
            //{
            //    foreach (CellBatch batch in grid.Batches)
            //    {
            //        ComponentBatch stampComponents = batch.Stamp.Components;

            //        if (!system.CanHostEntity(stampComponents))
            //        {
            //            continue;
            //        }

            //        system.UpdateIndividual(state, gameTime, input, stampComponents);
            //    }
            //}
        }

        public override void DrawAll(GameState state, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Matrix cameraTransform)
        {
            if (drawMethods.Count == 0)
            {
                return;
            }

            base.DrawAll(state, graphicsDevice, spriteBatch, cameraTransform);
        }

        public override void DrawIndividual(GameState state, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Vector2 drawOffset, ComponentBatch components)
        {
            components.GetComponentsMultiType(out Grid grid, out Unit unit);
            Point viewportSize = new Point(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);

            Rectangle visibleCoordinates = GetVisibleTiles(grid, unit, state, state.CameraPosition, viewportSize);

            if (grid is ArrayGrid arrayGrid)
            {
                DrawArrayGrid(arrayGrid, unit, state, graphicsDevice, spriteBatch, visibleCoordinates);
            }
        }

        private Rectangle GetVisibleTiles(Grid grid, Unit unit, GameState state, Vector2 cameraPosition, Point viewportSize)
        {
            DrawLayer layer = state.DrawLayers[grid.DrawLayer];
            Vector2 gridPosition = GetDrawPosition(layer, grid);

            return GetOffsetVisibleCoordinates(grid, unit, gridPosition, cameraPosition, viewportSize);
        }

        private ProtoEntity[,] ConvertToDrawableGrid(BatchedGrid grid, Rectangle visibleCoordinates)
        {
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
        }

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
                                method.Draw(state, graphics, spriteBatch, new Vector2(unit.LengthX * x, unit.LengthY * y), grid[x, y].Components);
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
                                int xIndex = x;
                                int yIndex = y;

                                bool isNegative = xIndex < 0;
                                xIndex %= grid.Width;

                                if (isNegative)
                                {
                                    xIndex = (grid.Width + xIndex) % grid.Width;
                                }

                                isNegative = yIndex < 0;
                                yIndex %= grid.Height;

                                if (isNegative)
                                {
                                    yIndex = (grid.Height + yIndex) % grid.Height;
                                }

                                method.Draw(state, graphics, spriteBatch, new Vector2(unit.LengthX * x, unit.LengthY * y), grid[xIndex, yIndex].Components);
                            }
                        }
                    }
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
                    visibleCoordinates = GetVisibleCartesianCoordinates(cameraPosition, viewportSize, unit, 1);
                    offset = Projector.PixelsToCartesian(gridPosition, unit).ToPoint();
                    break;
                case Perspective.Isometric:
                    visibleCoordinates = GetVisibleIsometricCoordinates(cameraPosition, viewportSize, unit, 1);
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
