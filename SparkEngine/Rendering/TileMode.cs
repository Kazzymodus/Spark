namespace SparkEngine.Rendering
{
    /// <summary>
    /// Determines what shape the rendererer considers the terrain tiles to be for rendering purpose.
    /// </summary>
    public enum TileMode
    {
        /// <summary>
        /// Doesn't use tiles.
        /// </summary>
        None,
        /// <summary>
        /// Tiles are square.
        /// </summary>
        Square,
        /// <summary>
        /// Tiles are isometric.
        /// </summary>
        Isometric,
        /// <summary>
        /// Tiles are hexagonal. Let's not use this one yet.
        /// </summary>
        Hexagonal
    }
}
