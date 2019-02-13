namespace SparkEngine.States.Primary
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using SparkEngine.Debug;
    using SparkEngine.IDs;
    using SparkEngine.Input;
    using SparkEngine.Rendering;

    public class DebugState : GameState
    {
        #region Constructors

        public DebugState(bool isActive)
            : base(isActive)
        {
        }

        #endregion

        #region Properties

        public override int ID
        {
            get { return StateIDs.DebugState; }
        }

        public override bool CanHaveMultiples
        {
            get { return false; }
        }

        public DebugLog DebugLog { get; private set; } = new DebugLog();

        public DebugTiler DebugTiler { get; private set; } = new DebugTiler();

        public CellPositionInfo CellDataMode { get; private set; }

        #endregion

        #region Methods

        protected internal override void Update(GameTime gameTime)
        {
            //Camera mainCamera = StateManager.MainCamera;
            //Vector2 cursorTile = mainCamera.GetCursorTile(new Vector2(50)); // FIX THIS
            //Vector2 cursorIso = RenderHelper.CoordsToIsometric(cursorTile);

            DebugLog.AddListMessage("FPS: " + (1.0 / gameTime.ElapsedGameTime.TotalSeconds));
            //DebugLog.AddListMessage("Camera Position: " + (mainCamera.Position - mainCamera.MapCenter));
            //DebugLog.AddListMessage("Cursor Position: " + mainCamera.MouseWorldPosition);
            //DebugLog.AddListMessage("Cursor Tile: " + cursorTile);
            //DebugLog.AddListMessage("Rotations: " + mainCamera.Rotations);
            //DebugLog.AddListMessage("CellDataMode: " + CellDataMode);

            //switch (mainCamera.Rotations)
            //{
            //    case 0:
            //        DebugLog.AddListMessage("Cursor Height: " + cursorIso.Y);
            //        break;
            //    case 1:
            //        DebugLog.AddListMessage("Cursor Height: " + (cursorIso.X + RenderHelper.TerrainSize.X - 1));
            //        break;
            //    case 2:
            //        DebugLog.AddListMessage("Cursor Height: " + (((RenderHelper.TerrainSize.Y - 1) * 2) - cursorIso.Y));
            //        break;
            //    case 3:
            //        DebugLog.AddListMessage("Cursor Height: " + (((RenderHelper.TerrainSize.X - 1) * 2) - (cursorIso.X + (RenderHelper.TerrainSize.X - 1))));
            //        break;
            //}

            base.Update(gameTime);
        }

        protected internal override void DrawWorld(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera.GetTransform());

            DebugLog.DrawWorldMessages(spriteBatch);
            //DebugTiler.DrawTiles(spriteBatch);

            spriteBatch.End();
        }

        protected internal override void DrawScreen(SpriteBatch spriteBatch)
        {
            DebugLog.DrawScreenMessages(spriteBatch);
            DebugLog.DrawListMessages(spriteBatch);
        }

        protected internal override void ProcessInput(GameTime gameTime)
        {
            if (InputHandler.IsKeyPressed(Keys.F10))
            {
                CycleCellDataMethod();
            }
        }

        private void CycleCellDataMethod()
        {
            CellDataMode++;

            if ((int)CellDataMode >= Enum.GetValues(typeof(CellPositionInfo)).Length)
            {
                CellDataMode = 0;
            }
        }

        #endregion
    }
}
