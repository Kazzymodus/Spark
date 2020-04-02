using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkEngine.Components
{
    public struct BitMaskGrid : IComponent
    {
        public const int Size = 32;

        public const uint MostSignificantBitOnly = (uint)1 << (Size - 1);

        private unsafe fixed uint bitMask[Size];

        public unsafe uint this[int i]
        {
            get => bitMask[i];
            set => bitMask[i] = value;
        }

        public unsafe bool this[int x, int y]
        {
            get => (bitMask[y] & MostSignificantBitOnly >> x) != 0;
        }

        public unsafe void ResetToZero()
        {
            for (int i = 0; i < Size; i++)
            {
                bitMask[i] = 0;
            }
        }

        public unsafe void ResetToOne()
        {
            for (int i = 0; i < Size; i++)
            {
                bitMask[i] = uint.MaxValue;
            }
        }
    }
}
