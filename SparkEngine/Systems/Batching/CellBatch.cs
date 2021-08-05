using System;
using Microsoft.Xna.Framework;
using SparkEngine.Entities;

namespace SparkEngine.Systems.Batching
{
    public abstract class CellBatch
    {
        #region Constructors

        public CellBatch(ProtoEntity stamp, int x, int y, bool isStencil)
        {
            Stamp = stamp;
            X = x;
            Y = y;
            IsStencil = isStencil;
        }

        #endregion

        #region Fields

        public ProtoEntity Stamp { get; }
        public int X { get; }
        public int Y { get; }
        public bool IsStencil { get; }

        #endregion

        #region Properties

        #endregion
    }

    public class BitMapBatch<T> : CellBatch
    {
        private T[] bitMap;

        public BitMapBatch(ProtoEntity stamp, int x, int y, bool isStencil)
            : base(stamp, x, y, isStencil)
        {
            var typeCode = Type.GetTypeCode(typeof(T));

            switch (typeCode)
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
                    throw new ArgumentException(
                        "BitMapBatch can only support the following types: sbyte, byte, short, ushort, int, uint, long, ulong");
            }

            bitMap = new T[Dimension];
        }

        public int Dimension { get; }
    }

    public class RectangularBatch : CellBatch
    {
        public RectangularBatch(ProtoEntity stamp, int x, int y, int width, int height, bool isStencil)
            : base(stamp, x, y, isStencil)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; }
        public int Height { get; }

        public Rectangle Bounds => new Rectangle(X, Y, Width, Height);
    }

    public class SquareBatch : CellBatch
    {
        public SquareBatch(ProtoEntity stamp, int x, int y, int size, bool isStencil)
            : base(stamp, x, y, isStencil)
        {
            Size = size;
        }

        public int Size { get; }
    }

    public class LineBatch : CellBatch
    {
        public LineBatch(ProtoEntity stamp, int x, int y, int direction, int length, bool isStencil)
            : base(stamp, x, y, isStencil)
        {
            Direction = direction;
            Length = length;
        }

        public int Direction { get; }
        public int Length { get; }
    }
}