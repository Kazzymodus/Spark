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

    public class GridCell
    {
        #region Fields

        private byte r, g, b;

        #endregion

        #region Constructors

        public GridCell(byte textureId)
        {
            TextureId = textureId;
            r = g = b = 0xFF;
        }

        public GridCell(byte textureId, Color color)
        {
            TextureId = textureId;
            r = color.R;
            g = color.G;
            b = color.B;
        }

        #endregion

        #region Public Properties

        public byte TextureId { get; }

        public Color Colour => new Color(r, g, b, (byte)0xFF);

        #endregion

        #region Methods

        public void SetColour(Color colour)
        {
            r = colour.R;
            g = colour.G;
            b = colour.B;
        }

        #endregion

    }
}
