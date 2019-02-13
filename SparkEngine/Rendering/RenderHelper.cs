namespace SparkEngine.Rendering
{
    using System;
    using Microsoft.Xna.Framework;
    using SparkEngine.DataStructures;

    public static class RenderHelper
    {
        #region Fields

        public const int DefaultTileWidth = 64;
        public const int DefaultTileHeight = 32;

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

        public static Vector2 TerrainSize { get; private set; }

        #region Methods

        public static Vector2 CoordsToIsometric(Vector2 coords)
        {
            float xIso = coords.X - coords.Y;
            float yIso = coords.X + coords.Y;

            return new Vector2(xIso, yIso);
        }

        public static Vector2 IsometricToCoords(Vector2 iso)
        {
            float xCoord = (iso.X + iso.Y) * 0.5f;
            float yCoord = (iso.Y - iso.X) * 0.5f;

            return new Vector2(xCoord, yCoord);
        }

        public static Vector2 CoordsToPixels(Vector2 coords)
        {
            float xPixel = (coords.X - coords.Y) * DefaultTileWidth * 0.5f;
            float yPixel = (coords.X + coords.Y) * DefaultTileHeight * 0.5f;

            return new Vector2(xPixel, yPixel);
        }

        public static Vector2 IsoToPixels(Vector2 coords)
        {
            float xPixel = coords.X * DefaultTileWidth * 0.5f;
            float yPixel = coords.Y * DefaultTileHeight * 0.5f;

            return new Vector2(xPixel, yPixel);
        }

        public static Vector2 PixelsToCoords(Vector2 pixels)
        {
            float xCoord = ((pixels.X / (RenderHelper.DefaultTileWidth * 0.5f)) + (pixels.Y / (RenderHelper.DefaultTileHeight * 0.5f))) * 0.5f;
            float yCoord = ((pixels.Y / (RenderHelper.DefaultTileHeight * 0.5f)) - (pixels.X / (RenderHelper.DefaultTileWidth * 0.5f))) * 0.5f;

            xCoord = (float)Math.Floor(xCoord - 0.5f);
            yCoord = (float)Math.Floor(yCoord + 0.5f);

            return new Vector2(xCoord, yCoord);
        }

        public static Vector2 PixelsToIso(Vector2 pixels)
        {
            float xIso = (float)Math.Round(pixels.X / (DefaultTileWidth * 0.5f));
            float yIso = (float)Math.Round(pixels.Y / (DefaultTileHeight * 0.5f));

            return new Vector2(xIso, yIso);
        }

        public static Vector2 RotateCoordsInMap(Vector2 coords, int rotations)
        {
            Vector2 rotatedCoords = coords;

            for (int i = 0; i < rotations; i++)
            {
                Vector2 oldCoords = rotatedCoords;

                // Rotate counterclockwise because we need to establish where the coord is GOING to be.

                rotatedCoords.X = (TerrainSize.X - 1) - oldCoords.Y;
                rotatedCoords.Y = oldCoords.X;
            }

            return rotatedCoords;
        }

        public static Vector2 RotateCoordsInSquare(Vector2 coords, int size, int rotations)
        {
            Vector2 rotatedPosition = coords;

            rotatedPosition.X += (rotations / 2) * (size - 1);
            rotatedPosition.Y += rotations == 1 || rotations == 2 ? 1 * (size - 1) : 0;

            return rotatedPosition;
        }

        public static void Initialise(GameSettings settings)
        {
            TerrainSize = settings.TerrainSize;
        }

        #endregion
    }
}
