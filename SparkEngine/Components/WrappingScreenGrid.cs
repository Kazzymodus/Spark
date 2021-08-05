using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SparkEngine.Components
{
    public struct WrappingScreenGrid : IComponent
    {
        public WrappingScreenGrid(Texture2D texture, int width, int height, Vector2 cellSize, Perspective perspective)
            : this(texture, new byte[width, height], cellSize, perspective)
        {
        }

        public WrappingScreenGrid(Texture2D texture, byte[,] frames, Vector2 cellSize, Perspective perspective)
        {
            Texture = texture;
            FrameGrid = frames;
            CellSize = cellSize;
            Perspective = perspective;

            XHead = 0;
            YHead = 0;
            XOffsetWest = 0;
            XOffsetEast = 0;
            YOffsetNorth = 0;
            YOffsetSouth = 0;
            Width = frames.GetLength(0);
            Height = frames.GetLength(1);
            WrappedWidth = 0;
            WrappedHeight = 0;

            Position = Vector2.Zero;
        }

        public Texture2D Texture { get; }

        public byte[,] FrameGrid { get; }

        /// <summary>
        ///     The index of the first rendered column.
        /// </summary>
        public int XHead { get; private set; }

        /// <summary>
        ///     The index of the first rendered row.
        /// </summary>
        public int YHead { get; private set; }

        /// <summary>
        ///     The index of the first column of the left half of the screen.
        /// </summary>
        public int XOffsetWest { get; set; }

        /// <summary>
        ///     The head index of the first column of the right half of the screen.
        /// </summary>
        public int XOffsetEast { get; set; }

        /// <summary>
        ///     The head index of the first row of the virtual grid to the north.
        /// </summary>
        public int YOffsetNorth { get; set; }


        /// <summary>
        ///     The head index of the first row of the virtual grid to the south.
        /// </summary>
        public int YOffsetSouth { get; set; }

        public Vector2 Position { get; set; }

        public int Width { get; }

        public int Height { get; }

        /// <summary>
        ///     The width of the underlying grid this screen grid is wrapping.
        /// </summary>
        public int WrappedWidth { get; private set; }

        /// <summary>
        ///     The height of the underlying grid this screen grid is wrapping.
        /// </summary>
        public int WrappedHeight { get; private set; }

        public Vector2 CellSize { get; }

        public Perspective Perspective { get; }

        public void SetWrappedDimensions(int width, int height)
        {
            WrappedWidth = width;
            WrappedHeight = height;
            if (XHead == 0) XOffsetEast = Width % width;
            if (YHead == 0) YOffsetSouth = Height / height;
        }

        public void ShiftObserver(Point shift, out bool wrappedBorder)
        {
            wrappedBorder = false;

            if (shift.X != 0)
            {
                // Is the west-east border wrapping?

                if (shift.X >= Width - XHead)
                {
                    wrappedBorder = true;
                    var count = shift.X / Width + 1;
                    for (var i = 0; i < count; i++)
                    {
                        XOffsetWest = XOffsetEast;
                        XOffsetEast = (XOffsetWest + Width % WrappedWidth) % WrappedWidth;
                    }

                    Console.WriteLine($"West: {XOffsetWest} East: {XOffsetEast}");
                }
                else if (shift.X < -XHead)
                {
                    wrappedBorder = true;
                    var count = shift.X / Width - 1;
                    for (var i = count; i < 0; i++)
                    {
                        XOffsetEast = XOffsetWest;
                        XOffsetWest = (XOffsetEast - Width % WrappedWidth) % WrappedWidth;
                        if (XOffsetWest < 0) XOffsetWest += WrappedWidth;
                    }

                    Console.WriteLine($"West: {XOffsetWest} East: {XOffsetEast}");
                }

                XHead += shift.X;
                var isNegative = XHead < 0;
                XHead %= Width;

                if (isNegative) XHead += Width;
            }

            if (shift.Y != 0)
            {
                if (shift.Y >= Height - YHead)
                {
                    wrappedBorder = true;
                    var count = shift.Y / Height + 1;
                    for (var i = 0; i < count; i++)
                    {
                        YOffsetNorth = YOffsetSouth;
                        YOffsetSouth = (YOffsetNorth + Height % WrappedHeight) % WrappedHeight;
                    }

                    Console.WriteLine($"North: {YOffsetNorth} South: {YOffsetSouth}");
                }
                else if (shift.Y < -YHead)
                {
                    wrappedBorder = true;
                    var count = shift.Y / Height - 1;
                    for (var i = count; i < 0; i++)
                    {
                        YOffsetSouth = YOffsetNorth;
                        YOffsetNorth = (YOffsetSouth - Height % WrappedHeight) % WrappedHeight;
                        if (YOffsetNorth < 0) YOffsetNorth += WrappedHeight;
                    }

                    Console.WriteLine($"North: {YOffsetNorth} South: {YOffsetSouth}");
                }

                YHead += shift.Y;
                var isNegative = YHead < 0;
                YHead %= Height;

                if (isNegative) YHead += Height;
            }
        }
    }
}