﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SparkEngine.Entities;

namespace SparkEngine.Systems.Batching
{
    public class GridBatcher
    {
        private const int MaxTileAmount = 1000000;

        private const int BatchCost = 128;

        private const int MaxBitsPerTile = 16;

        private const int MaxUniqueTiles = 128;

        //    private BatchedGrid BatchGrid(ArrayGrid grid)
        //    {
        //        int tileAmount = 0;

        //        try
        //        {
        //            tileAmount = grid.Width * grid.Height;
        //        }
        //        catch (OverflowException)
        //        {
        //            throw new ArgumentException("The grid is too large to batch.");
        //        }

        //        if (tileAmount > MaxTileAmount)
        //        {

        //            throw new ArgumentException("The grid is too large to batch.");
        //        }

        //        int bitmapDimensions = 8;

        //        while (grid.Width > bitmapDimensions && grid.Height > bitmapDimensions && bitmapDimensions < 64)
        //        {
        //            bitmapDimensions *= 2;
        //        }

        //        int maxBatches = ((bitmapDimensions * bitmapDimensions) + BatchCost) / BatchCost; 

        //        BatchTracker[] trackers = new BatchTracker[MaxUniqueTiles];

        //        int nextIndex = 0;

        //        for (int x = 0; x < grid.Width; x++)
        //        {
        //            for (int y = 0; y < grid.Height; y++)
        //            {
        //                ProtoEntity currentTile = grid[x,y];
        //                int trackerIndex = nextIndex;

        //                for (int i = 0; i < trackerIndex; i++)
        //                {
        //                    if (trackers[i].Stamp.Equals(currentTile))
        //                    {
        //                        trackerIndex = i;
        //                    }
        //                }

        //                BatchTracker tracker = trackerIndex == nextIndex ? new BatchTracker(currentTile, x, y) : trackers[trackerIndex];

        //                if (!tracker.BatchOpen)
        //                {
        //                    tracker.OpenBatch();
        //                }

        //                //tracker.AddTile
        //            }
        //        }

        //        throw new Exception("Whatever");
        //    }
    }

    public class BatchTracker
    {
        private const float BaseHeuristic = 1f;
        private readonly List<Point> tiles = new List<Point>();
        private int height;
        private int width;

        private int x;
        private int y;

        public BatchTracker(ProtoEntity stamp, int x, int y)
        {
            Stamp = stamp;
            tiles.Add(new Point(x, y));
        }

        public ProtoEntity Stamp { get; }
        public bool BatchOpen { get; private set; }
        public float Heuristics { get; private set; } = BaseHeuristic;

        public CellBatch CloseBatch()
        {
            //

            if (!BatchOpen)
                Console.WriteLine("ERROR: Batch not open!");

            //

            BatchOpen = false;

            return null; //////// TEMP
        }

        public void OpenBatch()
        {
            //

            if (BatchOpen)
                Console.WriteLine("ERROR: Batch already open!");

            //

            BatchOpen = true;
            Heuristics = BaseHeuristic;
            tiles.Clear();
        }

        public float GetTileHeuristicsScore(int x, int y)
        {
            for (var i = 0; i < tiles.Count; i++)
            {
                var coordinates = tiles[i];
            }

            return 1; // TEMP
        }
    }
}