//namespace SparkEngine.UI
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Text;
//    using System.Threading.Tasks;
//    using Microsoft.Xna.Framework;
//    using Microsoft.Xna.Framework.Graphics;
//    using SparkEngine.Input;

//    public class Tooltip : UIElement
//    {
//        #region Fields

//        private const int Height = 10;
//        private const int HorizontalPadding = 5;
//        private const int WidthPerCharacter = 4;
        
//        private string text;

//        #endregion

//        #region Constructors

//        public Tooltip(UIPanel parent = null)
//            : base(Vector2.Zero, parent)
//        {
//            text = "";
//        }

//        public Tooltip(string text, UIPanel parent = null)
//            : base(Vector2.Zero, parent)
//        {
//            this.text = text;
//        }

//        #endregion

//        #region Properties

//        private static Vector2 TooltipOffset
//        {
//            get { return new Vector2(16, 10); }
//        }

//        public string Text
//        {
//            set { text = value; }
//        }

//        #endregion

//        #region Methods

//        internal override void Draw(SpriteBatch spriteBatch)
//        {
//            Vector2 drawPosition = InputHandler.MousePosition.ToVector2() + TooltipOffset;
//            spriteBatch.DrawString(FontDictionary.GetFont(FontIDs.CourierNew), text, drawPosition, Color.White);
//        }

//        #endregion
//    }
//}
