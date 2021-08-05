namespace SparkEngine.Components
{
    public struct BitMaskGrid : IComponent
    {
        public const int Size = 32;

        public const uint MostSignificantBitOnly = (uint) 1 << (Size - 1);

        private unsafe fixed uint bitMask[Size];

        public unsafe uint this[int i]
        {
            get => bitMask[i];
            set => bitMask[i] = value;
        }

        public unsafe bool this[int x, int y] => (bitMask[y] & (MostSignificantBitOnly >> x)) != 0;

        public unsafe void ResetToZero()
        {
            for (var i = 0; i < Size; i++) bitMask[i] = 0;
        }

        public unsafe void ResetToOne()
        {
            for (var i = 0; i < Size; i++) bitMask[i] = uint.MaxValue;
        }
    }
}