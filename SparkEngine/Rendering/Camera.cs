namespace SparkEngine.Rendering
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using SparkEngine.Debug;
    using SparkEngine.Input;
    using SparkEngine.States;

    public class Camera
    {
        #region Fields

        /// <summary>
        /// The region in which the camera can move.
        /// </summary>
        private Rectangle constraints;

        private static Log Log { get; } = new Log();

        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new camera.
        /// </summary>
        /// <param name="viewportWidth">The width of the viewport.</param>
        /// <param name="viewportHeight">The height of the viewport.</param>
        public Camera(GraphicsDeviceManager graphics)
        {
            ViewportSize = new Point(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            CalculateTransform();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The position of the camera, with the upper-left corner being the origin.
        /// </summary>
        public Vector2 Position { get; private set; }

        /// <summary>
        /// The amount of (clockwise) rotations of the camera.
        /// </summary>
        public int Rotations { get; private set; }

        /// <summary>
        /// A generic transform matrix used for world drawing.
        /// </summary>
        public Matrix Transform { get; private set; }

        /// <summary>
        /// The size of the viewport.
        /// </summary>
        public Point ViewportSize { get; private set; }
        
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
                return new Rectangle(Position.ToPoint(), ViewportSize);
            }
        }

        #endregion

        #region Methods

        public static void DrawMessages(SpriteBatch spriteBatch) // Bad coupling is bad but this is not a priority right now.
        {
            Log.DrawMessages(spriteBatch);
        }

        /// <summary>
        /// Moves the camera a set amount of pixels.
        /// </summary>
        /// <param name="x">The amount of pixels to move right.</param>
        /// <param name="y">The amount of pixels to move down.</param>
        public void MoveCamera(float x, float y)
        {
            MoveCamera(new Vector2(x, y));
        }

        /// <summary>
        /// Moves the camera a set amount of pixels. 
        /// </summary>
        /// <param name="translation">The amount of pixels to move (x = right, y = down).</param>
        public void MoveCamera(Vector2 translation)
        {
            Position += translation;

            //ClampCameraToBounds();
            CalculateTransform();
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
        /// Gets the range of coordinates currently in camera view.
        /// </summary>
        /// <returns>A rectangle containing all visible coordinates.</returns>
        internal Rectangle GetVisibleCartesianCoordinates(Vector2 unit, int padding)
        {
            Point location = Projector.PixelsToCartesian(Position.ToPoint(), unit) - new Point(padding);
            Point size = Projector.PixelsToCartesian(ViewportSize, unit) + new Point(padding * 2);

            return new Rectangle(location, size);
        }

        internal Rectangle GetVisibleIsometricCoordinates(Vector2 unit, int padding = 0)
        {
            Point location = Projector.PixelsToIsometric(Position, unit).ToPoint() - new Point(padding);
            Point size = Projector.PixelsToIsometric(ViewportSize, unit) + new Point(padding * 2);

            return new Rectangle(location, size);
        }

        /// <summary>
        /// Calculate the camera's transform. Should only be called once per frame, at the beginning of the draw cycle.
        /// </summary>
        internal void CalculateTransform()
        {
            Transform = Matrix.CreateTranslation(new Vector3(-Position, 0f));
        }
        
        /// <summary>
        /// Rotates the camera around the map center.
        /// </summary>
        /// <param name="rotations">The amount of clockwise rotations to perform.</param>
        internal void RotateCamera(int rotations)
        {
            int modulatedRotations = ((rotations % 4) + 4) % 4; // In case a negative rotation is passed.

            Rotations += modulatedRotations;
            Rotations %= 4;

            CalculateTransform();
        }
        
        /// <summary>
        /// Clamps the camera to its constraints.
        /// </summary>
        private void ClampCameraToBounds()
        {
            if (Position.X < constraints.Left)
            {
                SetCameraPosition(new Vector2(constraints.Left, Position.Y));
            }
            else if (Position.X > constraints.Right)
            {
                SetCameraPosition(new Vector2(constraints.Right, Position.Y));
            }

            if (Position.Y < constraints.Top)
            {
                SetCameraPosition(new Vector2(Position.X, constraints.Top));
            }
            else if (Position.Y > constraints.Bottom)
            {
                SetCameraPosition(new Vector2(Position.X, constraints.Bottom));
            }
        }

        #endregion
    }
}
