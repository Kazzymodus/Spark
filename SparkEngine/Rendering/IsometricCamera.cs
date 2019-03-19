namespace SparkEngine.Rendering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class IsometricCamera : Camera
    {
        /// <summary>
        /// The maximum distance in tiles that the camera can pan away from the map.
        /// </summary>
        public const int VoidTileAmount = 8;

        /// <summary>
        /// The radius of off-screen tiles that are still being drawn.
        /// </summary>
        public const int TileCullPadding = 2;

        public IsometricCamera(GraphicsDeviceManager graphics)
            : base(graphics)
        {

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

    }
}
