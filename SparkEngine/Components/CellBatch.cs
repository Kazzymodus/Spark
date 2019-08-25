namespace SparkEngine.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;

    public class CellBatch<T> where T : GridCell
    {
        /*
        #region Fields

        T stamp;
        private Point location;
        private Point size;
        private Point[] clones;
        private bool isStencil;

        #endregion

        #region Constructors

        public CellBatch(T stamp, Point location, Point size, Point[] clones, bool isStencil)
        {
            this.stamp = stamp;
            this.location = location;
            this.size = size;
            this.clones = clones;
            this.isStencil = isStencil;
        }

        #endregion

        #region Properties

        public Rectangle Bounds => new Rectangle(location, size);

        #endregion

        #region Methods

        private static CellBatch<T> GetBatchFromStamp(T stamp, Grid<T> sampleGrid)
        {
            return GetBatchFromStamp(stamp, sampleGrid, Point.Zero, sampleGrid.Dimensions);
        }

        private static CellBatch<T> GetBatchFromStamp(T stamp, Grid<T> sampleGrid, Point startPoint, Point sampleSize)
        {
            List<Point> clones = new List<Point>();

            for (int i = startPoint.X; i < sampleSize.X; i++)
            {
                for (int j = startPoint.Y; j < startPoint.Y; j++)
                {
                    if (sampleGrid.GetTile(i, j).Equals(stamp))
                    {
                        clones.Add(new Point(i, j));
                    }
                }
            }

            int cloneAmount = clones.Count;
            int totalCells = sampleSize.X * sampleSize.Y;

            bool isStencil = false;

            if (cloneAmount > totalCells / 2)
            {
                isStencil = true;

                

            }

            return new CellBatch<T>(stamp, startPoint, sampleSize, clones.ToArray(), isStencil);
        }
        #endregion
        */
    }
}
