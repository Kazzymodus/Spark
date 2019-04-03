namespace SparkEngine.Rendering
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public static class Projector
    {
        #region Fields

        public const float SqrtTwoReciprocal = 1.41421356237f; // Move this somewhere better.

        public const int RotationNone = 0;
        public const int RotationClockwise = 1;
        public const int Rotation180 = 2;
        public const int RotationCounterClockwise = 3;

        public static Dictionary<string, Func<Vector2, Vector2, Vector2>> ToPixelConversions = new Dictionary<string, Func<Vector2, Vector2, Vector2>>();

        #endregion

        static Projector()
        {
            ToPixelConversions.Add(TileMode.None.ToString(), CarthesianToPixels);
            ToPixelConversions.Add(TileMode.Isometric.ToString(), IsometricToPixels);
        }

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
        /// Converts carthesian coordinates to isometric coordinates.
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        public static Vector2 CarthesianToIsometric(Vector2 coordinates)
        {
            float xIso = coordinates.X - coordinates.Y;
            float yIso = coordinates.X + coordinates.Y;

            return new Vector2(xIso, yIso);
        }

        /// <summary>
        /// Converts isometric coordinates to carthesian coordinates.
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        public static Vector2 IsometricToCarthesian(Vector2 coordinates)
        {
            float xCoord = (coordinates.X + coordinates.Y) * 0.5f;
            float yCoord = (coordinates.Y - coordinates.X) * 0.5f;

            return new Vector2(xCoord, yCoord);
        }

        /// <summary>
        /// Converts carthesian coordinates to pixel coordinates.
        /// </summary>
        /// <param name="coordinates"></param>
        /// <param name="tileSize"></param>
        /// <returns></returns>
        public static Vector2 CarthesianToPixels(Vector2 coordinates, Vector2 tileSize)
        {
            float xPixel = (coordinates.X - coordinates.Y) * tileSize.X * 0.5f;
            float yPixel = (coordinates.X + coordinates.Y) * tileSize.Y * 0.5f;

            return new Vector2(xPixel, yPixel);
        }

        /// <summary>
        /// Converts pixel coordinates to carthesian coordinates.
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns></returns>
        public static Vector2 PixelsToCarthesian(Vector2 pixels, Vector2 tileSize)
        {
            float xCoord = ((pixels.X / (tileSize.X * 0.5f)) + (pixels.Y / (tileSize.Y * 0.5f))) * 0.5f;
            float yCoord = ((pixels.Y / (tileSize.Y * 0.5f)) - (pixels.X / (tileSize.X * 0.5f))) * 0.5f;

            xCoord = (float)Math.Floor(xCoord - 0.5f);
            yCoord = (float)Math.Floor(yCoord + 0.5f);

            return new Vector2(xCoord, yCoord);
        }


        /// <summary>
        /// Converts isometric coordinates to pixel coordinates.
        /// </summary>
        /// <param name="coordinates"></param>
        /// <param name="tileSize"></param>
        /// <returns></returns>
        public static Vector2 IsometricToPixels(Vector2 coordinates, Vector2 tileSize)
        {
            float xPixel = coordinates.X * tileSize.X * 0.5f;
            float yPixel = coordinates.Y * tileSize.Y * 0.5f;

            return new Vector2(xPixel, yPixel);
        }

        /// <summary>
        /// Converts pixel coordinates to isometric coordinates.
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns></returns>
        public static Vector2 PixelsToIsometric(Vector2 pixels, Vector2 tileSize)
        {
            float xIso = (float)Math.Round(pixels.X / (tileSize.X * 0.5f));
            float yIso = (float)Math.Round(pixels.Y / (tileSize.Y * 0.5f));

            return new Vector2(xIso, yIso);
        }

        public static Vector2 DrawPositionAtCoords(Vector2 coordinates, Vector2 anchor, Vector2 unit)
        {
            Vector2 drawPosition = CarthesianToPixels(coordinates, unit);
            drawPosition -= anchor;

            return drawPosition;
        }

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
