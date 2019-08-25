namespace SparkEngine.Components
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using SparkEngine.Debug;
    using SparkEngine.Input;
    using SparkEngine.States;

    public class Camera : Component
    {
        #region Constructors
        /// <summary>
        /// Creates a new camera.
        /// </summary>
        /// <param name="viewportWidth">The width of the viewport.</param>
        /// <param name="viewportHeight">The height of the viewport.</param>
        public Camera()
        {
            ConstraintMode = CameraConstraints.Unconstrained;
            Constraints = default(Rectangle);
        }

        public Camera(CameraConstraints constraintsMode, Rectangle constraints)
        {
            ConstraintMode = constraintsMode;
            if (constraintsMode != CameraConstraints.Unconstrained)
            {
                Constraints = constraints;
            }
        }

        #endregion

        #region Properties

        public CameraConstraints ConstraintMode { get; set; }

        /// <summary>
        /// The region in which the camera can move.
        /// </summary>
        public Rectangle Constraints { get; set; }

        public Matrix Transform { get; set; }

        #endregion
    }
}
