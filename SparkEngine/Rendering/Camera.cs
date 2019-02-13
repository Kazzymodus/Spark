namespace SparkEngine.Rendering
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using SparkEngine.Input;
    using SparkEngine.States;

    public class Camera
    {
        #region Public Fields

        /// <summary>
        /// The maximum distance in tiles that the camera can pan away from the map.
        /// </summary>
        public const int VoidTileAmount = 8;
        /// <summary>
        /// The radius of off-screen tiles that are still being drawn.
        /// </summary>
        public const int TileCullPadding = 2;
        /// <summary>
        /// A generic transform matrix used for world drawing.
        /// </summary>
        private Matrix transform;
        /// <summary>
        /// 
        /// </summary>
        private int minXPos;
        /// <summary>
        /// The minimum y-position the camera can move to.
        /// </summary>
        private int minYPos;
        /// <summary>
        /// The maximum x-position the camera can move to.
        /// </summary>
        private int maxXPos;
        /// <summary>
        /// The maximum y-position the camera can move to.
        /// </summary>
        private int maxYPos;

        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new camera.
        /// </summary>
        /// <param name="viewportWidth">The width of the viewport.</param>
        /// <param name="viewportHeight">The height of the viewport.</param>
        public Camera(int viewportWidth, int viewportHeight)
        {
            ViewportSize = new Vector2(viewportWidth, viewportHeight);
        }

        #endregion

        #region Properties
        /// <summary>
        /// The position of the camera, with the upper-left corner being the origin.
        /// </summary>
        public Vector2 Position { get; private set; }
        /// <summary>
        /// The size of the viewport.
        /// </summary>
        public Vector2 ViewportSize { get; private set; }
        /// <summary>
        /// The center of the map (in pixels) relative to the upper-left corner.
        /// </summary>
        public Vector2 MapCenter { get; private set; }
        /// <summary>
        /// The position of the mouse in the world in pixels.
        /// </summary>
        public Vector2 MouseWorldPosition
        {
            get
            {
                return InputHandler.MousePosition.ToVector2() + Position;
            }
        }
        /// <summary>
        /// The area of the map currently in the viewport.
        /// </summary>
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(Position.ToPoint(), ViewportSize.ToPoint());
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Moves the camera a set amount of pixels. 
        /// </summary>
        /// <param name="translation">The amount of pixels to move (x = right, y = down).</param>
        public void MoveCamera(Vector2 translation)
        {
            MoveCamera((int)translation.X, (int)translation.Y);
        }
        /// <summary>
        /// Moves the camera a set amount of pixels.
        /// </summary>
        /// <param name="x">The amount of pixels to move right.</param>
        /// <param name="y">The amount of pixels to move down.</param>
        public void MoveCamera(float x, float y)
        {
            Vector2 translation = new Vector2(x, y);

            Position += translation;

            ClampCameraToBounds();
        }
        /// <summary>
        /// Sets the camera's position to the given value.
        /// </summary>
        /// <param name="position">The position in pixels to set the camera to.</param>
        public void SetCameraPosition(Vector2 position)
        {
            Position = position;
        }
        /// <summary>
        /// Gets the camera's transform.
        /// </summary>
        /// <returns>The camera's transform.</returns>
        public Matrix GetTransform()
        {
            transform = Matrix.CreateTranslation(new Vector3(-Position, 0f));
            return transform;
        }
        /// <summary>
        /// Rotates the camera around the map center.
        /// </summary>
        /// <param name="rotations">The amount of clockwise rotations to perform.</param>
        internal void RotateCamera(int rotations)
        {
            System.Diagnostics.Debug.Assert(rotations >= -4 && rotations <= 4, "Rotations < -4 || > 4");

            Vector2 centerOffset = Position - MapCenter;

            for (int i = 0; i < 4 - (rotations % 4); i++)
            {
                centerOffset = new Vector2(centerOffset.Y * 2, (int)-centerOffset.X / 2);
            }

            SetCameraPosition(MapCenter + centerOffset);
        }
        /// <summary>
        /// Initialises map-specific values.
        /// </summary>
        /// <param name="terrainSize">The size of the map's terrain in tiles.</param>
        internal void InitialiseMapDependancies(Vector2 terrainSize)
        {
            Vector2 tileRange = terrainSize + new Vector2(VoidTileAmount);

            minXPos = (int)(-tileRange.X / 2f) * RenderHelper.DefaultTileWidth;
            maxXPos = (int)(((tileRange.X / 2f) + 1) * RenderHelper.DefaultTileWidth) - (int)ViewportSize.X;

            minYPos = -(VoidTileAmount * RenderHelper.DefaultTileHeight);
            maxYPos = (int)(tileRange.Y * RenderHelper.DefaultTileHeight) - (int)ViewportSize.Y;

            MapCenter = new Vector2((-ViewportSize.X * 0.5f) + (RenderHelper.DefaultTileWidth * 0.5f), (-ViewportSize.Y * 0.5f) + ((terrainSize.Y / 2) * RenderHelper.DefaultTileHeight));
            SetCameraPosition(MapCenter);
        }
        /// <summary>
        /// Gets the range of coordinates currently in camera view.
        /// </summary>
        /// <returns>A rectangle containing all visible coordinates.</returns>
        internal Rectangle GetVisibleCoordinates()
        {
            Vector2 startCoordinate = RenderHelper.PixelsToIso(Position) - new Vector2(TileCullPadding);
            Vector2 endCoordinate = RenderHelper.PixelsToIso(ViewportSize) + new Vector2(TileCullPadding + 1);

            return new Rectangle(startCoordinate.ToPoint(), endCoordinate.ToPoint());
        }

        /// <summary>
        /// Clamps the camera to its constraints.
        /// </summary>
        private void ClampCameraToBounds()
        {
            if (Position.X < minXPos)
            {
                SetCameraPosition(new Vector2(minXPos, Position.Y));
            }
            else if (Position.X > maxXPos)
            {
                SetCameraPosition(new Vector2(maxXPos, Position.Y));
            }

            if (Position.Y < minYPos)
            {
                SetCameraPosition(new Vector2(Position.X, minYPos));
            }
            else if (Position.Y > maxYPos)
            {
                SetCameraPosition(new Vector2(Position.X, maxYPos));
            }
        }

        #endregion
    }
}
