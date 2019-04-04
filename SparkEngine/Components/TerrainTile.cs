namespace SparkEngine.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Rendering;

    public class TerrainTile
    {
        #region Private Fields

        #endregion

        #region Constructors

        public TerrainTile(Vector2 coordinates, Texture2D spriteSheet)
        {
            Coordinates = coordinates;
            SpriteSheet = spriteSheet;
        }

        #endregion

        #region Public Properties

        public Texture2D SpriteSheet { get; }

        public Color Color { get; set; } = Color.White;

        public Vector2 Coordinates { get; }

        public GridObject Occupant { get; private set; }

        public bool IsOccupied
        {
            get { return Occupant != null; }
        }

        #endregion

        #region Internal Methods

        internal void Occupy(GridObject occupant)
        {
            Occupant = occupant;
        }

        internal void Unoccupy()
        {
            Occupant = null;
        }

        internal void Draw(SpriteBatch spriteBatch, Camera camera, Vector2 tileSize)
        {
            //Vector2 correctedCoords = Coordinates;
            //correctedCoords.X -= Projector.TerrainSize.X;

            //Vector2 rotatedCoords = Projector.RotateCoordsInMap(Coordinates, camera.Rotations);
            //Vector2 drawPosition = Projector.CoordsToPixels(rotatedCoords);

            Vector2 drawPosition = Projector.CartesianToIsometricPixels(Coordinates, tileSize);

            int frameX = (int)tileSize.X * camera.Rotations;

            Rectangle drawRectangle = new Rectangle(frameX, 0, (int)tileSize.X, (int)tileSize.Y);

            Color colour = Color.White;

            spriteBatch.Draw(SpriteSheet, drawPosition, drawRectangle, colour);
        }

        #endregion
    }
}
