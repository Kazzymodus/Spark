using Microsoft.Xna.Framework.Input;
using SparkEngine.Components;
using SparkEngine.States;
using SparkEngine.Systems;

namespace InfiniteMinesweeper.Systems
{
    internal class MineCameraSystem : CameraSystem
    {
        private const int Speed = 8;

        public MineCameraSystem(int maxSubs = GameState.MaxEntities)
            : base(maxSubs)
        {
        }

        protected override void UpdateComponent(ref Camera camera, int index, UpdateInfo updateInfo)
        {
            var input = updateInfo.Input;

            if (input.IsKeyDown(Keys.Left)) camera.PositionX -= Speed;
            if (input.IsKeyDown(Keys.Right)) camera.PositionX += Speed;
            if (input.IsKeyDown(Keys.Up)) camera.PositionY -= Speed;
            if (input.IsKeyDown(Keys.Down)) camera.PositionY += Speed;

            base.UpdateComponent(ref camera, index, updateInfo);
        }
    }
}