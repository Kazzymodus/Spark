//namespace SparkEngine.Debug
//{
//    using Microsoft.Xna.Framework;
//    using Microsoft.Xna.Framework.Graphics;
//    using SparkEngine.Rendering;

//    public struct DebugTile
//    {
//        #region Fields

//        private Texture2D sprite;
//        private Vector2 coordinates;
//        private Color colour;

//        #endregion

//        #region Constructors

//        public DebugTile(Texture2D sprite, Vector2 coordinates, Color colour)
//        {
//            this.sprite = sprite;
//            this.coordinates = coordinates;
//            this.colour = colour;
//        }

//        #endregion

//        #region Methods

//        internal void Draw(SpriteBatch spriteBatch, int rotations)
//        {
//            //Vector2 rotatedCoords = Projector.RotateCoordsInMap(coordinates, rotations);
//            //Vector2 drawPosition = Projector.CoordsToPixels(rotatedCoords);

//            Vector2 drawPosition = Projector.CartesianToPixels(coordinates);

//            int frameX = Projector.DefaultTileWidth * rotations;
//            Rectangle drawRectangle = new Rectangle(frameX, 0, Projector.DefaultTileWidth, Projector.DefaultTileHeight);

//            spriteBatch.Draw(sprite, drawPosition, drawRectangle, colour);
//        }

//        #endregion
//    }
//}

