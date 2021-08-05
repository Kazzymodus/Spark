namespace SparkEngine.Rendering
{
    public enum LayerSortMethod
    {
        /// <summary>
        ///     Sprites with this method aren't sorted at all.
        /// </summary>
        None,

        /// <summary>
        ///     Sprites with this method will be drawn before all other sprites.
        /// </summary>
        First,

        /// <summary>
        ///     Sprites with this method will be drawn after all other sprites.
        /// </summary>
        Last,

        /// <summary>
        ///     Sprites with this method are sorted by their height mapped as distance to the camera.
        /// </summary>
        HeightAsDistance
    }
}