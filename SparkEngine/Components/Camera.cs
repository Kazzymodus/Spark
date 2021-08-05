using Microsoft.Xna.Framework;

namespace SparkEngine.Components
{
    public struct Camera : IComponent
    {
        #region Constructors

        public Camera(CameraConstraints constraintsMode, Rectangle constraints)
        {
            PositionX = 0;
            PositionY = 0;
            ConstraintMode = constraintsMode;
            Constraints = constraintsMode != CameraConstraints.Unconstrained ? constraints : default;
            Transform = default;
        }

        #endregion

        #region Properties

        public float PositionX { get; set; }

        public float PositionY { get; set; }

        public CameraConstraints ConstraintMode { get; set; }

        /// <summary>
        ///     The region in which the camera can move.
        /// </summary>
        public Rectangle Constraints { get; set; }

        public Matrix Transform { get; set; }

        #endregion
    }
}