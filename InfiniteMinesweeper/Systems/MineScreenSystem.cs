namespace InfiniteMinesweeper.Systems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using SparkEngine.Components;
    using SparkEngine.States;
    using SparkEngine.Systems;
    using Components;
    using SparkEngine.Input;
    using SparkEngine.Systems.Tasks;

    class MineScreenSystem : WrappingScreenGridSystem
    {
        private Vector2 oldCameraPosition;
        private bool safeClick;

        public MineScreenSystem()
            : base(1)
        {
            safeClick = true;
        }

        public override void OnAddComponent(ref WrappingScreenGrid grid, int owner, GameState gameState)
        {
            MineField mineField = ((MineFieldSystem)gameState.ComponentSystems[typeof(MineFieldSystem)]).GetComponents()[0]; // Bad, fix this someday.

            RecalculateAll(ref grid, mineField);
        }

        protected override void UpdateComponent(ref WrappingScreenGrid grid, int index, UpdateInfo updateInfo)
        {
            // Recalculate frames if necessary.

            Vector2 cameraPosition = (Vector2)updateInfo.State.CameraPosition;

            Point oldCoordinates = (oldCameraPosition / grid.CellSize).ToPoint();
            Point newCoordinates = (cameraPosition / grid.CellSize).ToPoint();

            Point coordinateShift = newCoordinates - oldCoordinates;

            MineField mineField = ((MineFieldSystem)updateInfo.State.ComponentSystems[typeof(MineFieldSystem)]).GetComponents()[0]; // Bad, fix this someday.

            if (coordinateShift.X % mineField.Width != 0 || coordinateShift.Y % mineField.Height != 0)
            {
                RecalculateFrames(ref grid, mineField, coordinateShift, cameraPosition);
            }

            oldCameraPosition = cameraPosition;

            //

            InputHandler input = updateInfo.Input;

            Point cursorMineFieldTile = GetCursorTile(grid.Position, grid.CellSize, (Vector2)updateInfo.State.CameraPosition, input.MouseScreenPosition);
            Point cursorScreenTile = ((input.MouseScreenPosition + grid.Position) / grid.CellSize).ToPoint(); // Not exactly right, will do for now.

            if (mineField.WrapAround)
            {
                if (cursorMineFieldTile.X < 0)
                {
                    cursorMineFieldTile.X += grid.Width;
                }

                if (cursorMineFieldTile.Y < 0)
                {
                    cursorMineFieldTile.Y += grid.Height;
                }

                cursorMineFieldTile.X %= mineField.Width;
                cursorMineFieldTile.Y %= mineField.Height;
            }

            if (false && input.IsMousePressed(MouseButtons.LMB))
            {
                if (safeClick)
                {
                    OffsetPointToBlankTile(ref grid, mineField, ref cursorMineFieldTile);

                    safeClick = false;
                }

                if (!mineField.IsRevealed(cursorMineFieldTile))
                {
                    RevealCell(ref grid, mineField, cursorMineFieldTile, cursorScreenTile);
                }
            }
            else if (input.IsMousePressed(MouseButtons.RMB))
            {
                Console.WriteLine(cursorMineFieldTile);
                //Sprite orig = grid[cursorMineFieldTile.X, cursorMineFieldTile.Y];
                //orig.SetFrames(orig.FrameX == 11 ? 12 : 11, 0);

                //    MineFrame frame = (MineFrame)mineGrid.Cells[cursorTile.X, cursorTile.Y].Batch.GetComponent<Sprite>().FrameX;

                //    if (frame == MineFrame.Unknown)
                //    {
                //        mineGrid.Cells[cursorTile.X, cursorTile.Y].Batch.GetComponent<Sprite>().SetFrames((int)MineFrame.Flag, 0);
                //    }
                //    else if (frame == MineFrame.Flag)
                //    {
                //        mineGrid.Cells[cursorTile.X, cursorTile.Y].Batch.GetComponent<Sprite>().SetFrames((int)MineFrame.Unknown, 0);
                //    }
            }

            base.UpdateComponent(ref grid, index, updateInfo);
        }

        public void RevealCell(ref WrappingScreenGrid grid, MineField mineField, Point mineFieldTile, Point screenTile)
        {
            if (mineField.IsMine(mineFieldTile))
            {
                //Sprite mine = grid[screenTile.X, screenTile.Y];
                //mine.SetFrames(10, 0);
                //grid[screenTile.X, screenTile.Y] = mine;
                //return;
            }

            int adjacentMines = CalculateAdjacents(mineField, mineFieldTile);

            mineField.RevealCell(mineFieldTile);

            //grid[screenTile.X, screenTile.Y].SetFrames(adjacentMines, 0);

            //Sprite orig = grid[screenTile.X, screenTile.Y];
            //orig.SetFrames(adjacentMines, 0);
            //grid[screenTile.X, screenTile.Y] = orig;

            if (adjacentMines == 0)
            {
                RevealAdjacents(ref grid, mineField, mineFieldTile, screenTile);
            }

            //int adjacentMines = CalculateAdjacents(tile, coordinates);

            //for (int i = 0; i < cellComponents.Length; i++)
            //{
            //    if (cellComponents[i] is Sprite)
            //    {
            //        ((Sprite)cellComponents[i]).SetFrames(adjacentMines, 0);
            //    }
            //}

            //if (adjacentMines == 0)
            //{
            //    RevealAdjacents(ref tile, coordinates);
            //}
        }

        private void RevealAdjacents(ref WrappingScreenGrid grid, MineField mineField, Point mineFieldTile, Point screenTile)
        {
            for (int x = mineFieldTile.X - 1; x < mineFieldTile.X + 2; x++)
            {
                for (int y = mineFieldTile.Y - 1; y < mineFieldTile.Y + 2; y++)
                {
                    if (x == mineFieldTile.X && y == mineFieldTile.Y)
                    {
                        continue;
                    }

                    if (mineField.WrapAround)
                    {
                        int xIndex = x % mineField.Width;
                        int yIndex = y % mineField.Height;

                        if (xIndex < 0)
                        {
                            xIndex += mineField.Width;
                        }

                        if (yIndex < 0)
                        {
                            yIndex += mineField.Height;
                        }

                        if (!mineField.IsRevealed(xIndex, yIndex))
                        {
                            Point newScreenTile = screenTile + new Point(xIndex, yIndex) - mineFieldTile;
                            newScreenTile.X %= grid.Width;
                            newScreenTile.Y %= grid.Height;

                            if (newScreenTile.X < 0)
                            {
                                newScreenTile.X += grid.Width;
                            }

                            if (newScreenTile.Y < 0)
                            {
                                newScreenTile.Y += grid.Height;
                            }

                            RevealCell(ref grid, mineField, new Point(xIndex, yIndex), newScreenTile);
                        }
                    }
                }
            }
        }

        private int CalculateAdjacents(MineField mineField, Point mineFieldTile)
        {
            int adjacentMines = 0;

            for (int x = mineFieldTile.X - 1; x < mineFieldTile.X + 2; x++)
            {
                for (int y = mineFieldTile.Y - 1; y < mineFieldTile.Y + 2; y++)
                {
                    if (x == mineFieldTile.X && y == mineFieldTile.Y)
                    {
                        continue;
                    }

                    if (mineField.WrapAround)
                    {
                        int xIndex = x % mineField.Width;
                        int yIndex = y % mineField.Height;

                        if (xIndex < 0)
                        {
                            xIndex += mineField.Width;
                        }

                        if (yIndex < 0)
                        {
                            yIndex += mineField.Height;
                        }

                        if (mineField.IsMine(xIndex, yIndex))
                        {
                            adjacentMines++;
                        }
                    }
                }
            }

            return adjacentMines;
        }

        public void OffsetPointToBlankTile(ref WrappingScreenGrid grid, MineField mineField, ref Point coordinates)
        {
            Point searchCoordinates = coordinates;
            int maskIndex = mineField.GetMaskIndex(coordinates);
            bool tileFound = false;

            for (int maxAdjacents = 1; maxAdjacents < 9; maxAdjacents++)
            {
                do
                {
                    searchCoordinates.X++;

                    if (searchCoordinates.X >= mineField.Width)
                    {
                        searchCoordinates.X = 0;
                        searchCoordinates.Y++;

                        if (searchCoordinates.Y >= mineField.Height)
                        {
                            searchCoordinates.Y = 0;
                        }

                        if (searchCoordinates.Y % BitMaskGrid.Size == 0)
                        {
                            maskIndex += mineField.MasksPerRow;
                        }
                    }

                    if (searchCoordinates.X % BitMaskGrid.Size == 0)
                    {
                        maskIndex++;

                        if (maskIndex >= mineField.MaskAmount)
                        {
                            maskIndex = 0;
                        }
                    }

                    if (!mineField.MineMasks[maskIndex][searchCoordinates.X, searchCoordinates.Y] && CalculateAdjacents(mineField, searchCoordinates) < maxAdjacents)
                    {
                        Point offset = coordinates - searchCoordinates; // Swap?
                        grid.ShiftObserver(offset, out bool wrappedBorder);

                        coordinates = searchCoordinates;
                        tileFound = true;
                        break;
                    }
                }
                while (searchCoordinates != coordinates);

                if (tileFound)
                {
                    break;
                }
            }
        }

        private void RecalculateAll(ref WrappingScreenGrid grid, MineField mineField)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    Point fieldCoordinate = new Point(x % mineField.Width, y % mineField.Height);
                    grid.FrameGrid[x, y] = mineField.IsMine(fieldCoordinate) ? (byte)MineFrame.Mine : (byte)CalculateAdjacents(mineField, fieldCoordinate);
                }
            }
        }

        private void RecalculateFrames(ref WrappingScreenGrid grid, MineField mineField, Point shift, Vector2 cameraPosition)
        {
            grid.ShiftObserver(shift, out bool wrappedBorder);

            if (shift.X != 0)
            {
                RecalculateHorizontalFrames(ref grid, mineField, shift.X, wrappedBorder);
            }
            if (shift.Y != 0)
            {
                RecalculateVerticalFrames(ref grid, mineField, shift.Y, wrappedBorder);
            }

        }

        private void RecalculateHorizontalFrames(ref WrappingScreenGrid grid, MineField mineField, int xShift, bool wrappedBorder)
        {
            if (xShift > 0)
            {
                // This means tiles on the right end side of the screen need to be recalculated.
                // xScreenTile is the first tile that will need to be recalculated.

                int xScreenTile = grid.XHead - xShift;

                if (xScreenTile < 0)
                {
                    xScreenTile = (xScreenTile % grid.Width) + grid.Width;
                }

                // The minefield tile index corresponding to this first screen tile.

                int offset = wrappedBorder ? grid.XOffsetWest : grid.XOffsetEast;
                int xMineTile = (offset + xScreenTile) % mineField.Width;

                xScreenTile--;
                xMineTile--;

                for (int x = 0; x < xShift; x++)
                {
                    if (++xMineTile >= mineField.Width)
                    {
                        xMineTile = 0;
                    }

                    if (++xScreenTile >= grid.Width)
                    {
                        xScreenTile = 0;
                    }

                    for (int y = 0; y < grid.Height; y++)
                    {
                        grid.FrameGrid[xScreenTile, y] = mineField.IsMine(xMineTile, y % mineField.Height) ? (byte)MineFrame.Mine : (byte)CalculateAdjacents(mineField, new Point(xMineTile, y));
                    }
                }
            }
            else if (xShift < 0)
            {
                int xScreenTile = grid.XHead;

                // This is always XOffsetWest for some stupid reason I don't understand.

                int offset = grid.XOffsetWest;
                int xMineTile = (offset + xScreenTile) % mineField.Width;

                xScreenTile++;
                xMineTile++;

                for (int x = 0; x > xShift; x--)
                {
                    if (--xMineTile < 0)
                    {
                        xMineTile = mineField.Width - 1;
                    }

                    if (--xScreenTile < 0)
                    {
                        System.Diagnostics.Debug.Assert(!wrappedBorder);
                        xScreenTile = grid.Width - 1;
                    }

                    for (int y = 0; y < grid.Height; y++)
                    {
                        grid.FrameGrid[xScreenTile, y] = mineField.IsMine(xMineTile, y % mineField.Height) ? (byte)MineFrame.Mine : (byte)CalculateAdjacents(mineField, new Point(xMineTile, y));
                    }
                }
            }
        }

        private void RecalculateVerticalFrames(ref WrappingScreenGrid grid, MineField mineField, int yShift, bool wrappedBorder)
        {
            if (yShift > 0)
            {
                // This means tiles on the bottom end side of the screen need to be recalculated.
                // yScreenTile is the first tile that will need to be recalculated.

                int yScreenTile = grid.YHead - yShift;

                if (yScreenTile < 0)
                {
                    yScreenTile = (yScreenTile % grid.Height) + grid.Height;
                }

                // The minefield tile index corresponding to this first screen tile.

                int offset = wrappedBorder ? grid.YOffsetNorth : grid.YOffsetSouth;
                int yMineTile = (offset + yScreenTile) % mineField.Height;

                yScreenTile--;
                yMineTile--;

                for (int y = 0; y < yShift; y++)
                {
                    if (++yMineTile >= mineField.Height)
                    {
                        yMineTile = 0;
                    }

                    if (++yScreenTile >= grid.Height)
                    {
                        yScreenTile = 0;
                    }

                    for (int x = 0; x < grid.Width; x++)
                    {
                        grid.FrameGrid[x, yScreenTile] = mineField.IsMine(x % mineField.Width, yMineTile) ? (byte)MineFrame.Mine : (byte)CalculateAdjacents(mineField, new Point(x, yMineTile));
                    }
                }
            }
            else if (yShift < 0)
            {
                int yScreenTile = grid.YHead;

                // This is always YOffsetNorth for some stupid reason I don't understand.

                int offset = grid.YOffsetNorth;
                int yMineTile = (offset + yScreenTile) % mineField.Height;

                yScreenTile++;
                yMineTile++;

                for (int y = 0; y > yShift; y--)
                {
                    if (--yMineTile < 0)
                    {
                        yMineTile = mineField.Height - 1;
                    }

                    if (--yScreenTile < 0)
                    {
                        System.Diagnostics.Debug.Assert(!wrappedBorder);
                        yScreenTile = grid.Height - 1;
                    }

                    for (int x = 0; x < grid.Width; x++)
                    {
                        grid.FrameGrid[x, yScreenTile] = mineField.IsMine(x % mineField.Width, yMineTile) ? (byte)MineFrame.Mine : (byte)CalculateAdjacents(mineField, new Point(x, yMineTile));
                    }
                }
            }
        }

        //private void ShiftFrames(ref ScreenTextureGrid grid, int shiftX, int shiftY)
        //{
        //    int directionX, directionY;
        //    int gridStartX, gridStartY, gridEndX, gridEndY;

        //    if (shiftX <= 0)
        //    {
        //        directionX = 1;
        //        gridStartX = 0;
        //        gridEndX = grid.Width + shiftX;
        //    }
        //    else
        //    {
        //        directionX = -1;
        //        gridStartX = grid.Width - 1;
        //        gridEndX = shiftX - 1;
        //    }

        //    if (shiftY <= 0)
        //    {
        //        directionY = 1;
        //        gridStartY = 0;
        //        gridEndY = grid.Height + shiftY;
        //    }
        //    else
        //    {
        //        directionY = -1;
        //        gridStartY = grid.Height - 1;
        //        gridEndY = shiftY - 1;
        //    }

        //    for (int x = gridStartX; x != gridEndX; x += directionX)
        //    {
        //        for (int y = gridStartY; y != gridEndY; y += directionY)
        //        {
        //            grid[x, y] = grid[x - shiftX, y - shiftY];
        //        }
        //    }
        //}

        private void BufferSearchVertical(ArrayGrid<Sprite> grid, MineField mineField, Vector2 cameraPosition, int shiftX, int shiftY)
        {
            int startX, endX;

            if (shiftX <= 0)
            {
                startX = grid.Width - 1;
                endX = startX + shiftX;
            }
            else
            {
                startX = 0;
                endX = shiftX;
            }

            bool[] searchBuffer = new bool[9];
            int bufferHead = 0;

            BitMaskGrid[] mineMasks = mineField.MineMasks;
            BitMaskGrid[] revealMasks = mineField.RevealMasks;

            Point startCoordinate = ((-mineField.Position + cameraPosition) / grid.TileSize).ToPoint();
            startCoordinate.X %= mineField.Width;
            startCoordinate.Y %= mineField.Height;
            Point bufferTopLeftCoordinate = startCoordinate - new Point(1);
            if (bufferTopLeftCoordinate.X < 0)
            {
                bufferTopLeftCoordinate.X += mineField.Width;
            }
            if (bufferTopLeftCoordinate.Y < 0)
            {
                bufferTopLeftCoordinate.Y += mineField.Height;
            }
            int maskIndex = mineField.GetMaskIndex(bufferTopLeftCoordinate); // The index of the mask our start tile is in (top left corner of buffer).

            for (int x = 0; x < shiftX; x++) // For all (now) empty columns
            {
                int mineCount = 0;

                // First, we put the six tiles above our first sample tile into the buffer.

                Point bufferCoordinate = startCoordinate;

                for (int bufferY = -1; bufferY < 1;)
                {
                    bufferCoordinate.Y = startCoordinate.Y + bufferY;
                    if (bufferCoordinate.Y < 0)
                    {
                        bufferCoordinate.Y += mineField.Height;
                    }
                    else if (bufferCoordinate.Y >= mineField.Height)
                    {
                        bufferCoordinate.Y -= mineField.Height;
                    }

                    uint line = mineMasks[maskIndex][bufferCoordinate.Y % BitMaskGrid.Size];
                    bool resetMaskIndex = false;

                    for (int bufferX = 0; bufferX < 3;)
                    {
                        bufferCoordinate.X = bufferTopLeftCoordinate.X + bufferX;

                        if (bufferCoordinate.X >= mineField.Width)
                        {
                            bufferCoordinate.X -= mineField.Width;
                        }

                        bool isMine = (line & (BitMaskGrid.MostSignificantBitOnly >> (bufferCoordinate.X % BitMaskGrid.Size))) != 0;
                        searchBuffer[bufferHead++] = isMine;
                        if (isMine)
                        {
                            mineCount++;
                        }

                        if ((++bufferX + startCoordinate.X) % BitMaskGrid.Size == 0)
                        {
                            maskIndex++;

                            if (maskIndex % mineField.MasksPerRow == 0)
                            {
                                maskIndex -= mineField.MasksPerRow;
                            }
                            resetMaskIndex = true;
                        }
                    }

                    if (resetMaskIndex)
                    {
                        if (maskIndex % mineField.MasksPerRow == 0)
                        {
                            maskIndex += mineField.MasksPerRow;
                        }

                        maskIndex--;
                    }

                    if ((++bufferY + startCoordinate.Y) % BitMaskGrid.Size == 0)
                    {
                        maskIndex += mineField.MasksPerRow;

                        if (maskIndex >= mineField.MaskAmount)
                        {
                            maskIndex %= mineField.MasksPerRow;
                        }
                    }
                }

                bufferCoordinate.Y = startCoordinate.Y;

                for (int y = 0; y < grid.Height;)
                {
                    uint line = mineMasks[maskIndex][bufferCoordinate.Y];
                    bool resetMaskIndex = false;

                    for (int lineX = 0; lineX < 3;)
                    {
                        bufferCoordinate.X = startCoordinate.X + lineX;
                        if (bufferCoordinate.X < 0)
                        {
                            bufferCoordinate.X += mineField.Width;
                        }
                        else if (bufferCoordinate.X >= mineField.Width)
                        {
                            bufferCoordinate.X -= mineField.Width;
                        }

                        bool isMine = (line & (BitMaskGrid.MostSignificantBitOnly >> (startCoordinate.X + lineX) % BitMaskGrid.Size)) != 0;
                        if (isMine ^ searchBuffer[bufferHead])
                        {
                            if (isMine)
                            {
                                mineCount++;
                            }
                            else
                            {
                                mineCount--;
                            }
                        }

                        searchBuffer[bufferHead++] = isMine;

                        if (bufferHead > 8)
                        {
                            bufferHead = 0;
                        }

                        if ((++lineX + startCoordinate.X) % BitMaskGrid.Size == 0)
                        {
                            maskIndex++;

                            if (maskIndex % mineField.MasksPerRow == 0)
                            {
                                maskIndex -= mineField.MasksPerRow;
                            }

                            resetMaskIndex = true;
                        }
                    }

                    Sprite orig = grid[x, y];
                    orig.SetFrames(mineField.IsRevealed(bufferCoordinate) ? mineCount : 12, 0);
                    grid[x, y] = orig;

                    if (mineCount == 0)
                    {

                    }

                    if (resetMaskIndex)
                    {
                        if (maskIndex % mineField.MasksPerRow == 0)
                        {
                            maskIndex += mineField.MasksPerRow;
                        }

                        maskIndex--;
                    }

                    if (++y + startCoordinate.Y % BitMaskGrid.Size == 0)
                    {
                        maskIndex += mineField.MasksPerRow;

                        if (maskIndex >= mineField.MaskAmount)
                        {
                            maskIndex %= mineField.MasksPerRow;
                        }
                    }

                    bufferCoordinate.Y++;

                    if (bufferCoordinate.Y >= mineField.Height)
                    {
                        bufferCoordinate.Y -= mineField.Height;
                    }
                }
            }
        }
    }
}
