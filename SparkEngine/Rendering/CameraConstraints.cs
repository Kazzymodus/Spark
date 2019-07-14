namespace SparkEngine.Rendering
{
    public enum CameraConstraints
    {
        /// <summary>
        /// Camera has no constraints. Use with caution, camera position can overflow.
        /// </summary>
        Unconstrained,
        /// <summary>
        /// Camera is constrained within the bounds of the constraints variable.
        /// </summary>
        Constrained,
        /// <summary>
        /// When the camera goes outside its constraints, it jumps over to the other side.
        /// </summary>
        WrapAround

    }
}
