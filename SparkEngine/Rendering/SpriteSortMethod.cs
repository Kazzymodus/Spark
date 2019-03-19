namespace SparkEngine.Rendering
{
    public enum SpriteSortMethod
    {
        /// <summary>
        /// Sprites with this method aren't sorted at all.
        /// </summary>
        None,
        /// <summary>
        /// Sprites with this method are sorted by their height mapped as distance to the camera.
        /// </summary>
        HeightAsDistance,
    }
}
