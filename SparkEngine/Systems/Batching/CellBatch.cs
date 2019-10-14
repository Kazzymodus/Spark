namespace SparkEngine.Systems.Batching
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;

    public abstract class CellBatch
    {
        #region Fields

        public ProtoEntity Stamp { get; }
        public int X { get; }
        public int Y { get; }
        public bool IsStencil { get; }

        #endregion

        #region Constructors

        public CellBatch(ProtoEntity stamp, int x, int y, bool isStencil)
        {
            Stamp = stamp;
            X = x;
            Y = y;
            IsStencil = isStencil;
        }

        #endregion

        #region Properties



        #endregion
    }

    public class BitMapBatch<T> : CellBatch
    {
        public int Dimension { get; }
        private T[] bitMap;

        public BitMapBatch(ProtoEntity stamp, int x, int y, bool isStencil)
            : base(stamp, x, y, isStencil)
        {
            TypeCode typeCode = Type.GetTypeCode(typeof(T));

            switch(typeCode)
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                    Dimension = 8;
                    break;
                case TypeCode.Int16:
                case TypeCode.UInt16:
                    Dimension = 16;
                    break;
                case TypeCode.Int32:
                case TypeCode.UInt32:
                    Dimension = 32;
                    break;
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    Dimension = 64;
                    break;
                default:
                    throw new ArgumentException("BitMapBatch can only support the following types: sbyte, byte, short, ushort, int, uint, long, ulong");
            }

            bitMap = new T[Dimension];
        }
    }

    public class RectangularBatch : CellBatch
    {
        public int Width { get; }
        public int Height { get; }

        public RectangularBatch(ProtoEntity stamp, int x, int y, int width, int height, bool isStencil)
            : base(stamp, x, y, isStencil)
        {
            Width = width;
            Height = height;
        }

        public Rectangle Bounds => new Rectangle(X, Y, Width, Height);
    }

    public class SquareBatch : CellBatch
    {
        public int Size { get; }

        public SquareBatch(ProtoEntity stamp, int x, int y, int size, bool isStencil)
            : base(stamp, x, y, isStencil)
        {
            Size = size;
        }
    }

    public class LineBatch : CellBatch
    {
        public int Direction { get; }
        public int Length { get; }

        public LineBatch(ProtoEntity stamp, int x, int y, int direction, int length, bool isStencil)
            : base(stamp, x, y, isStencil)
        {
            Direction = direction;
            Length = length;
        }
    }
}
