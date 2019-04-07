namespace SparkEngine.Rendering
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The Projector class supplies methods for projecting world elements to the screen.
    /// </summary>
    public static class Projector
    {
        #region Fields

        public const float SqrtTwoReciprocal = 1.41421356237f; // Move this somewhere better.

        public const int RotationNone = 0;
        public const int RotationClockwise = 1;
        public const int Rotation180 = 2;
        public const int RotationCounterClockwise = 3;

        #endregion

        #region Properties

        public static Vector2 Dimension1X1
        {
            get { return Vector2.One; }
        }

        public static Vector2 Dimension2X2
        {
            get { return new Vector2(2); }
        }

        public static Vector2 Dimension3X3
        {
            get { return new Vector2(3); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts cartesian coordinates to isometric coordinates (integer).
        /// </summary>
        /// <param name="coordinates">The cartesian coordinates to convert.</param>
        /// <returns>A set of isometric coordinates.</returns>
        public static Point CartesianToIsometric(Point coordinates)
        {
            int xIso = coordinates.X - coordinates.Y;
            int yIso = coordinates.X + coordinates.Y;

            return new Point(xIso, yIso);
        }

        /// <summary>
        /// Converts cartesian coordinates to isometric coordinates (non-integer).
        /// </summary>
        /// <param name="coordinates">The cartesian coordinates to convert.</param>
        /// <returns>A set of isometric coordinates.</returns>
        public static Vector2 CartesianToIsometric(Vector2 coordinates)
        {
            float xIso = coordinates.X - coordinates.Y;
            float yIso = coordinates.X + coordinates.Y;

            return new Vector2(xIso, yIso);
        }

        /// <summary>
        /// Converts isometric coordinates to cartesian coordinates (integer).
        /// </summary>
        /// <param name="coordinates">The isometric coordinates to convert.</param>
        /// <returns>A set of cartesian coordinates.</returns>
        public static Point IsometricToCartesian(Point coordinates)
        {
            int xCoord = (coordinates.X + coordinates.Y) / 2;
            int yCoord = (coordinates.Y - coordinates.X) / 2;

            return new Point(xCoord, yCoord);
        }

        /// <summary>
        /// Converts isometric coordinates to cartesian coordinates (non-integer).
        /// </summary>
        /// <param name="coordinates">The isometric coordinates to convert.</param>
        /// <returns>A set of cartesian coordinates.</returns>
        public static Vector2 IsometricToCartesian(Vector2 coordinates)
        {
            float xCoord = (coordinates.X + coordinates.Y) * 0.5f;
            float yCoord = (coordinates.Y - coordinates.X) * 0.5f;

            return new Vector2(xCoord, yCoord);
        }

        /// <summary>
        /// Converts cartesian coordinates to pixel coordinates (integer).
        /// </summary>
        /// <param name="coordinates">The coordinates to convert.</param>
        /// <param name="cellSize">The size of a single cell.</param>
        /// <returns>A set of pixel coordinates.</returns>
        public static Point CartesianToPixels(Point coordinates, Vector2 cellSize)
        {
            int xPixel = (int)(coordinates.X * cellSize.X);
            int yPixel = (int)(coordinates.Y * cellSize.Y);

            return new Point(xPixel, yPixel);
        }

        /// <summary>
        /// Converts cartesian coordinates to pixel coordinates (non-integer).
        /// </summary>
        /// <param name="coordinates">The coordinates to convert.</param>
        /// <param name="cellSize">The size of a single cell.</param>
        /// <returns>A set of pixel coordinates.</returns>
        public static Vector2 CartesianToPixels(Vector2 coordinates, Vector2 cellSize)
        {
            float xPixel = coordinates.X * cellSize.X;
            float yPixel = coordinates.Y * cellSize.Y;

            return new Vector2(xPixel, yPixel);
        }

        /// <summary>
        /// Converts pixel coordinates to cartesian coordinates (integer).
        /// </summary>
        /// <param name="pixels">The coordinates to convert.</param>
        /// <param name="cellSize">The size of a cartesian cell.</param>
        /// <returns>A set of cartesian coordinates.</returns>
        public static Point PixelsToCartesian(Point pixels, Vector2 cellSize)
        {
            int xIso = pixels.X / (int)cellSize.X;
            int yIso = pixels.Y / (int)cellSize.Y;

            return new Point(xIso, yIso);
        }

        /// <summary>
        /// Converts pixel coordinates to cartesian coordinates (non-integer).
        /// </summary>
        /// <param name="pixels">The coordinates to convert.</param>
        /// <param name="cellSize">The size of a cartesian cell.</param>
        /// <returns>A set of cartesian coordinates.</returns>
        public static Vector2 PixelsToCartesian(Vector2 pixels, Vector2 cellSize)
        {
            float xIso = pixels.X / cellSize.X;
            float yIso = pixels.Y / cellSize.Y;

            return new Vector2(xIso, yIso);
        }

        /// <summary>
        /// Converts isometric coordinates to pixel coordinates (integer).
        /// </summary>
        /// <param name="coordinates">The coordinates to convert.</param>
        /// <param name="cellSize">The size of an isometric cell.</param>
        /// <returns>A set of pixel coordinates.</returns>
        public static Point IsometricToPixels(Point coordinates, Vector2 cellSize)
        {
            int xPixel = (int)(coordinates.X * cellSize.X) / 2;
            int yPixel = (int)(coordinates.Y * cellSize.Y) / 2;

            return new Point(xPixel, yPixel);
        }

        /// <summary>
        /// Converts isometric coordinates to pixel coordinates (non-integer).
        /// </summary>
        /// <param name="coordinates">The coordinates to convert.</param>
        /// <param name="cellSize">The size of an isometric cell.</param>
        /// <returns>A set of pixel coordinates.</returns>
        public static Vector2 IsometricToPixels(Vector2 coordinates, Vector2 cellSize)
        {
            float xPixel = coordinates.X * cellSize.X * 0.5f;
            float yPixel = coordinates.Y * cellSize.Y * 0.5f;

            return new Vector2(xPixel, yPixel);
        }

        /// <summary>
        /// Converts pixel coordinates to isometric coordinates (integer).
        /// </summary>
        /// <param name="pixels">The coordinates to convert.</param>
        /// <param name="cellSize">The size of an isometric cell.</param>
        /// <returns>A set of isometric coordinates.</returns>
        public static Point PixelsToIsometric(Point pixels, Vector2 tileSize)
        {
            int xIso = pixels.X / ((int)tileSize.X / 2);
            int yIso = pixels.Y / ((int)tileSize.Y / 2);

            return new Point(xIso, yIso);
        }

        /// <summary>
        /// Converts pixel coordinates to isometric coordinates (non-integer).
        /// </summary>
        /// <param name="pixels">The coordinates to convert.</param>
        /// <param name="cellSize">The size of an isometric cell.</param>
        /// <returns>A set of isometric coordinates.</returns>
        public static Vector2 PixelsToIsometric(Vector2 pixels, Vector2 cellSize)
        {
            float xIso = pixels.X / (cellSize.X * 0.5f);
            float yIso = pixels.Y / (cellSize.Y * 0.5f);

            return new Vector2(xIso, yIso);
        }

        /// <summary>
        /// Converts cartesian coordinates to isometric coordinates, then convert those to pixels.
        /// </summary>
        /// <param name="coordinates">The coordinates to convert.</param>
        /// <param name="cellSize">The size of an isometric cell.</param>
        /// <returns>A set of pixel coordinates.</returns>
        public static Point CartesianToIsometricToPixels(Point coordinates, Vector2 cellSize)
        {
            int xPixel = (coordinates.X - coordinates.Y) * ((int)cellSize.X / 2);
            int yPixel = (coordinates.X + coordinates.Y) * ((int)cellSize.Y / 2);

            return new Point(xPixel, yPixel);
        }

        /// <summary>
        /// Converts cartesian coordinates to isometric coordinates, then convert those to pixels.
        /// </summary>
        /// <param name="coordinates">The coordinates to convert.</param>
        /// <param name="cellSize">The size of an isometric cell.</param>
        /// <returns>A set of pixel coordinates.</returns>
        public static Vector2 CartesianToIsometricToPixels(Vector2 coordinates, Vector2 cellSize)
        {
            float xPixel = (coordinates.X - coordinates.Y) * cellSize.X * 0.5f;
            float yPixel = (coordinates.X + coordinates.Y) * cellSize.Y * 0.5f;

            return new Vector2(xPixel, yPixel);
        }

        //public static Vector2 IsometricPixelsToCartesian(Vector2 pixels, Vector2 tileSize)
        //{
        //    float xCoord = ((pixels.X / (tileSize.X * 0.5f)) + (pixels.Y / (tileSize.Y * 0.5f))) * 0.5f;
        //    float yCoord = ((pixels.Y / (tileSize.Y * 0.5f)) - (pixels.X / (tileSize.X * 0.5f))) * 0.5f;

        //    xCoord = (float)Math.Floor(xCoord - 0.5f);
        //    yCoord = (float)Math.Floor(yCoord + 0.5f);

        //    return new Vector2(xCoord, yCoord);
        //}

        //public static Vector2 RotateCoordsInMap(Vector2 coords, int rotations)
        //{
        //    Vector2 rotatedCoords = coords;

        //    for (int i = 0; i < rotations; i++)
        //    {
        //        Vector2 oldCoords = rotatedCoords;

        //        // Rotate counterclockwise because we need to establish where the coord is GOING to be.

        //        rotatedCoords.X = (TerrainSize.X - 1) - oldCoords.Y;
        //        rotatedCoords.Y = oldCoords.X;
        //    }

        //    return rotatedCoords;
        //}

        public static Vector2 RotateCoordsInRectangle(Vector2 coords, int size, int rotations)
        {
            Vector2 rotatedPosition = coords;

            rotatedPosition.X += (rotations / 2) * (size - 1);
            rotatedPosition.Y += rotations == 1 || rotations == 2 ? 1 * (size - 1) : 0;

            return rotatedPosition;
        }

        #endregion
    }
}
