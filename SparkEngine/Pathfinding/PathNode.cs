namespace SparkEngine.Pathfinding
{
    using System;
    using Microsoft.Xna.Framework;

    public class PathNode
    {
        private const double TileCostAdjacent = 1;
        private static readonly double TileCostDiagonal = Math.Sqrt(TileCostAdjacent * 2);

        #region Constructors

        public PathNode(int x, int y)
        {
            Coordinates = new Vector2(x, y);
        }

        public PathNode(Vector2 coordinates)
        {
            Coordinates = coordinates;
        }

        #endregion

        #region Properties

        public Vector2 Coordinates { get; }

        public double FScore { get; private set; }

        public int GScore { get; private set; }

        public double Heuristic { get; private set; }

        public PathNode Parent { get; private set; }

        #endregion

        #region Methods

        public void SetScores(int gScore, PathNode parent, PathNode target)
        {
            int xManhattan = (int)Math.Abs(Coordinates.X - target.Coordinates.X);
            int yManhattan = (int)Math.Abs(Coordinates.Y - target.Coordinates.Y);
            Heuristic = (TileCostAdjacent * (xManhattan + yManhattan)) + ((TileCostDiagonal - 2 * TileCostAdjacent) * Math.Min(xManhattan, yManhattan));
            //Heuristic *= 1 + 0.02;

            //int dx1 = (int)(Coordinates.X - target.Coordinates.X);
            //int dy1 = (int)(Coordinates.Y - target.Coordinates.Y);
            //int dx2 = 0 - (int)target.Coordinates.X;
            //int dy2 = 0 - (int)target.Coordinates.Y;
            //int cross = Math.Abs(dx1 * dy2 - dx2 * dy1);
            //Heuristic += cross * 0.02;

            GScore = gScore;
            FScore = GScore + Heuristic;

            Parent = parent;
        }

        #endregion
    }
}
