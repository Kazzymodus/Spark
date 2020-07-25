namespace InfiniteMinesweeper.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using SparkEngine.Components;

    public struct MineField : IComponent
    {
        public BitMaskGrid[] MineMasks { get; private set; }
        public BitMaskGrid[] RevealMasks { get; private set; }

        public int MasksPerRow { get; }

        public Vector2 Position { get; }
        public Point TileSize { get; }

        public int Width { get; }
        public int Height { get; }

        public bool WrapAround { get; }

        public MineField(Vector2 position, Point tileSize, int width, int height, bool wrapAround, float mineRatio)
        {
            Position = position;
            TileSize = tileSize;
            Width = width;
            Height = height;
            WrapAround = wrapAround;

            MasksPerRow = width / BitMaskGrid.Size + 1;
            int maskAmountY = height / BitMaskGrid.Size + 1;
            int maskAmountTotal = MasksPerRow * maskAmountY;
            MineMasks = SeedMineField(width, height, tileSize, mineRatio, maskAmountTotal);
            RevealMasks = new BitMaskGrid[maskAmountTotal];
        }

        public int MaskAmount => MasksPerRow * (Height / (BitMaskGrid.Size + 1));

        public void RevealCell(int x, int y)
        {
            int maskIndex = GetMaskIndex(x, y);
            RevealMasks[maskIndex][y % BitMaskGrid.Size] |= BitMaskGrid.MostSignificantBitOnly >> x % BitMaskGrid.Size;
        }

        public void RevealCell(Point coordinates)
        {
            RevealCell(coordinates.X, coordinates.Y);
        }

        public int GetMaskIndex(int x, int y)
        {
            return y / BitMaskGrid.Size * MasksPerRow + x / BitMaskGrid.Size;
        }

        public int GetMaskIndex(Point coordinates)
        {
            return GetMaskIndex(coordinates.X, coordinates.Y);
        }

        public bool IsMine(int x, int y)
        {
            int maskIndex = GetMaskIndex(x, y);
            return (MineMasks[maskIndex][y % BitMaskGrid.Size] & BitMaskGrid.MostSignificantBitOnly >> x % BitMaskGrid.Size) != 0;
        }

        public bool IsMine(Point coordinates)
        {
            return IsMine(coordinates.X, coordinates.Y);
        }

        public bool IsRevealed(int x, int y)
        {
            int maskIndex = GetMaskIndex(x, y);
            return (RevealMasks[maskIndex][y % BitMaskGrid.Size] & BitMaskGrid.MostSignificantBitOnly >> x % BitMaskGrid.Size) != 0;
        }

        public bool IsRevealed(Point coordinates)
        {
            return IsRevealed(coordinates.X, coordinates.Y);
        }

        private static BitMaskGrid[] SeedMineField(int width, int height, Point tileSize, float mineRatio, int totalMasks)
        {
            mineRatio = MathHelper.Clamp(mineRatio, 0, 1);
            bool invertSeeding = mineRatio > 0.5f;

            BitMaskGrid[] mineMasks = new BitMaskGrid[totalMasks];

            if (invertSeeding)
            {
                for (int i = 0; i < mineMasks.Length; i++)
                {
                    mineMasks[i].ResetToOne();
                }
            }

            int seedAmount = (int)(width * height * (invertSeeding ? 1 - mineRatio : mineRatio));
            uint seedValue = (uint)(invertSeeding ? 0 : 1);
            Random mineLayer = new Random();

            while (seedAmount > 0)
            {
                int x = mineLayer.Next(width);
                int y = mineLayer.Next(height);

                int maskIndex = y / BitMaskGrid.Size * (width / BitMaskGrid.Size + 1) + x / BitMaskGrid.Size;

                if ((mineMasks[maskIndex][y % BitMaskGrid.Size] & BitMaskGrid.MostSignificantBitOnly >> x % BitMaskGrid.Size) == 0 ^ invertSeeding)
                {
                    mineMasks[maskIndex][y % BitMaskGrid.Size] ^= BitMaskGrid.MostSignificantBitOnly >> x;
                    //Console.WriteLine(mask[y % BitMaskGrid.Size]);
                    seedAmount--;
                }
            }

            return mineMasks;
        }
    }
}
