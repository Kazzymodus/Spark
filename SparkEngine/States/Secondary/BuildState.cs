namespace SparkEngine.States.Secondary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Dictionaries;
    using SparkEngine.IDs;
    using SparkEngine.Rendering;
    using SparkEngine.UI;

    internal class BuildState : GameState
    {
        #region Constructors

        public BuildState(bool isActive = true)
            : base(isActive)
        {
            UIPanel buildingList = new UIPanel(TextureDictionary.GetUITexture(UserInterfaceIDs.PanelBackdrops.BuildTask), Pivot.Center, Camera.ViewportSize);
            List<UIElement> structureButtons = new List<UIElement>();

            for (int i = 0; i < StructureIDs.Count; i++)
            {
                //Button
            }

            //buildingList.AddElements();

            StateUI = new StateUI(buildingList);
        }

        #endregion

        #region Properties

        public override int ID
        {
            get { return StateIDs.BuildState; }
        }
        
        public override bool CanHaveMultiples
        {
            get { return false; }
        }

        #endregion

        #region Methods

        protected internal override void Update(GameTime gameTime)
        {
        }

        protected internal override void DrawWorld(SpriteBatch spriteBatch)
        {
        }

        protected internal override void DrawScreen(SpriteBatch spriteBatch)
        {
            base.DrawScreen(spriteBatch);
        }

        protected internal override void ProcessInput(GameTime gametime)
        {
        }

        #endregion
    }
}
