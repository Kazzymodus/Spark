using System;
using Microsoft.Xna.Framework;
using SparkEngine.Components;

namespace InfiniteMinesweeper.Components
{
    public struct MineField : IComponent
    {
        public BitMaskGrid[] MineMasks { get; }
        public BitMaskGrid[] RevealMasks { get; }

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
            var maskAmountY = height / BitMaskGrid.Size + 1;
            var maskAmountTotal = MasksPerRow * maskAmountY;
            MineMasks = SeedMineField(width, height, tileSize, mineRatio, maskAmountTotal);
            RevealMasks = new BitMaskGrid[maskAmountTotal];
        }

        public int MaskAmount => MasksPerRow * (Height / (BitMaskGrid.Size + 1));

        public void RevealCell(int x, int y)
        {
            var maskIndex = GetMaskIndex(x, y);
            RevealMasks[maskIndex][y % BitMaskGrid.Size] |=
                BitMaskGrid.MostSignificantBitOnly >> (x % BitMaskGrid.Size);
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
            var maskIndex = GetMaskIndex(x, y);
            return (MineMasks[maskIndex][y % BitMaskGrid.Size] &
                    (BitMaskGrid.MostSignificantBitOnly >> (x % BitMaskGrid.Size))) != 0;
        }

        public bool IsMine(Point coordinates)
        {
            return IsMine(coordinates.X, coordinates.Y);
        }

        public bool IsRevealed(int x, int y)
        {
            var maskIndex = GetMaskIndex(x, y);
            return (RevealMasks[maskIndex][y % BitMaskGrid.Size] &
                    (BitMaskGrid.MostSignificantBitOnly >> (x % BitMaskGrid.Size))) != 0;
        }

        public bool IsRevealed(Point coordinates)
        {
            return IsRevealed(coordinates.X, coordinates.Y);
        }

        private static BitMaskGrid[] SeedMineField(int width, int height, Point tileSize, float mineRatio,
            int totalMasks)
        {
            mineRatio = MathHelper.Clamp(mineRatio, 0, 1);
            var invertSeeding = mineRatio > 0.5f;

            var mineMasks = new BitMaskGrid[totalMasks];

            if (invertSeeding)
                for (var i = 0; i < mineMasks.Length; i++)
                    mineMasks[i].ResetToOne();

            var seedAmount = (int) (width * height * (invertSeeding ? 1 - mineRatio : mineRatio));
            var seedValue = (uint) (invertSeeding ? 0 : 1);
            var mineLayer = new Random();

            while (seedAmount > 0)
            {
                var x = mineLayer.Next(width);
                var y = mineLayer.Next(height);

                var maskIndex = y / BitMaskGrid.Size * (width / BitMaskGrid.Size + 1) + x / BitMaskGrid.Size;

                if (!(((mineMasks[maskIndex][y % BitMaskGrid.Size] &
                        (BitMaskGrid.MostSignificantBitOnly >> (x % BitMaskGrid.Size))) == 0) ^
                      invertSeeding)) continue;

                mineMasks[maskIndex][y % BitMaskGrid.Size] ^= BitMaskGrid.MostSignificantBitOnly >> x;
                //Console.WriteLine(mask[y % BitMaskGrid.Size]);
                seedAmount--;
            }

            return mineMasks;
        }
    }
}