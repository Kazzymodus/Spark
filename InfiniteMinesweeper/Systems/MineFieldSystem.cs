using InfiniteMinesweeper.Components;
using SparkEngine.Systems;

namespace InfiniteMinesweeper.Systems
{
    internal sealed class MineFieldSystem : ComponentSystem<MineField>
    {
        internal MineFieldSystem()
            : base(1)
        {
        }

        protected override void UpdateComponent(ref MineField mineField, int index, UpdateInfo updateInfo)
        {
            // Moved to MineScreenSystem
            /*
            InputHandler input = updateInfo.Input;

            Point cursorTile = GetCursorTile(mineField.Position, mineField.TileSize, (Vector2)updateInfo.State.CameraPosition, input.MouseScreenPosition);

            if (mineField.WrapAround)
            {
                if (cursorTile.X < 0)
                {
                    cursorTile.X += mineField.Width;
                }

                if (cursorTile.Y < 0)
                {
                    cursorTile.Y += mineField.Height;
                }

                cursorTile.X %= mineField.Width;
                cursorTile.Y %= mineField.Height;
            }

            if (input.IsMousePressed(MouseButtons.LMB))
            {
                //if (firstClick)
                //{
                //    OffsetPointToBlankTile(ref mineGrid, ref cursorTile);

                //    firstClick = false;
                //}

                //if (mineGrid.Cells[cursorTile.X, cursorTile.Y].Batch.GetComponent<Sprite>().FrameX == (int)MineFrame.Unknown)
                //{
                //    RevealCell(ref mineGrid, cursorTile);
                //}
            }
            else if (input.IsMousePressed(MouseButtons.RMB))
            {
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
            */
        }
    }
}