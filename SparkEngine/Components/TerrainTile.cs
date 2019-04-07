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
        #region Fields

        private byte r, g, b;

        #endregion

        #region Constructors

        public TerrainTile(byte textureId)
        {
            TextureId = textureId;
            r = g = b = 0xFF;
        }

        public TerrainTile(byte textureId, Color color)
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

    }
}
