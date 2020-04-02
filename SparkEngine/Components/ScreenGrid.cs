namespace SparkEngine.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public struct ScreenGrid : IComponent
    {
        public ScreenGrid(Texture2D texture, int width, int height, Vector2 cellSize, Perspective perspective)
            : this(texture, new byte[width, height], cellSize, perspective)
        {
        }

        public ScreenGrid(Texture2D texture, byte[,] frames, Vector2 cellSize, Perspective perspective)
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
            
            
            Position = Vector2.Zero;
        }

        public Texture2D Texture { get; }

        public byte[,] FrameGrid { get; private set; }

        public int XHead { get; private set; }

        public int YHead { get; private set; }

        public int XOffsetWest { get; set; }

        public int XOffsetEast { get; set; }

        public int YOffsetNorth { get; set; }
        
        public int YOffsetSouth { get; set; }

        public Vector2 Position { get; set; }

        public int Width { get; }

        public int Height { get; }

        public Vector2 CellSize { get; }

        public Perspective Perspective { get; }

        public void ShiftTiles(Point shift)
        {
            if (shift.X != 0)
            {
                XHead += shift.X;
                bool isNegative = XHead < 0;
                XHead %= Width;

                if (isNegative)
                {
                    XHead += Width;
                }

                if (XHead == 0)
                {
                    if (shift.X < 0)
                    {
                        //XOffsetEast = XOffsetWest;
                    }
                    else
                    {
                        //XOffsetWest = XOffsetEast;
                    }
                }
            }

            if (shift.Y != 0)
            {
                YHead += shift.Y;
                bool isNegative = YHead < 0;
                YHead %= Height;

                if (isNegative)
                {
                    YHead += Height;
                }

                if (YHead == 0)
                {
                    if (shift.Y < 0)
                    {
                        YOffsetSouth = YOffsetNorth;
                    }
                    else
                    {
                        YOffsetNorth = YOffsetSouth;
                    }
                }
            }
        }
    }
}
