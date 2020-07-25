namespace InfiniteMinesweeper.Systems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using SparkEngine.Components;
    using SparkEngine.Input;
    using SparkEngine.States;
    using SparkEngine.Systems;

    class MineCameraSystem : CameraSystem
    {
        public MineCameraSystem(int maxSubs = GameState.MaxEntities)
            : base(maxSubs)
        {

        }

        protected override void UpdateComponent(ref Camera camera, int index, UpdateInfo updateInfo)
        {
            InputHandler input = updateInfo.Input;

            if (input.IsKeyDown(Keys.Left))
            {
                camera.PositionX -= 4;
            }
            if (input.IsKeyDown(Keys.Right))
            {
                camera.PositionX += 4;
            }
            if (input.IsKeyDown(Keys.Up))
            {
                camera.PositionY -= 4;
            }
            if (input.IsKeyDown(Keys.Down))
            {
                camera.PositionY += 4;
            }

            base.UpdateComponent(ref camera, index, updateInfo);
        }
    }
}
