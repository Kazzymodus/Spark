namespace SparkEngine.DataStructures
{
    using Microsoft.Xna.Framework;

    public struct GameSettings
    {
        #region Constructors

        public GameSettings(int cameraSpeed, int terrainSize)
        {
            CameraSpeed = cameraSpeed;
            TerrainSize = new Vector2(terrainSize);

            DrawGrid = true;
            DrawUI = false;
        }

        public GameSettings(int cameraSpeed, Vector2 terrainSize)
        {
            CameraSpeed = cameraSpeed;
            TerrainSize = terrainSize;

            DrawGrid = true;
            DrawUI = true;
        }

        #endregion

        #region Properties

        public float CameraSpeed { get; }

        public Vector2 TerrainSize { get; }

        public bool DrawGrid { get; private set; }

        public bool DrawUI { get; private set; }

        #endregion

        #region Methods

        public void ToggleGrid()
        {
            DrawGrid = !DrawGrid;
        }

        public void ToggleUI()
        {
            DrawUI = !DrawUI;
        }

        #endregion
    }
}
