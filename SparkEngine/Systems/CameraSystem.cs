using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SparkEngine.Components;
using SparkEngine.Input;
using SparkEngine.States;

namespace SparkEngine.Systems
{
    public class CameraSystem : ComponentSystem<Camera>
    {
        public CameraSystem(int maxSubs = GameState.MaxEntities)
            : base(maxSubs)
        {

        }

        protected internal override void UpdateComponent(ref Camera camera, int index, UpdateInfo updateInfo)
        {
            if (camera.ConstraintMode == CameraConstraints.Constrained)
            {
                ClampCamera(ref camera);
            }
            else if (camera.ConstraintMode == CameraConstraints.WrapAround)
            {
                WrapCamera(ref camera);
            }
            camera.Transform = Matrix.CreateTranslation(new Vector3(-camera.PositionX, -camera.PositionY, 0f));
        }

        public override void OnRemoveComponent(ref Camera component, int entity, GameState state)
        {
            if (state.RenderCamera == entity)
            {
                state.ClearRenderCamera();
            }
        }

        #region Methods

        /// <summary>
        /// Calculate the camera's transform. Should only be called once per frame, at the beginning of the draw cycle.
        /// </summary>
        internal Matrix CalculateTransform(Vector2 position)
        {
            return Matrix.CreateTranslation(new Vector3(-position, 0f));
        }

        ///// <summary>
        ///// Rotates the camera around the map center.
        ///// </summary>
        ///// <param name="rotations">The amount of clockwise rotations to perform.</param>
        //internal void RotateCamera(int rotations)
        //{
        //    int modulatedRotations = ((rotations % 4) + 4) % 4; // In case a negative rotation is passed.

        //    Rotations += modulatedRotations;
        //    Rotations %= 4;

        //    CalculateTransform();
        //}

        /// <summary>
        /// Clamps the camera to its constraints.
        /// </summary>
        private void ClampCamera(ref Camera camera)
        {
            float positionX = camera.PositionX;
            float positionY = camera.PositionY;
            Rectangle constraints = camera.Constraints;

            if (positionX < constraints.Left)
            {
                camera.PositionX = constraints.Left;
            }
            else if (positionX > constraints.Right)
            {
                camera.PositionX = constraints.Right;
            }

            if (positionY < constraints.Top)
            {
                camera.PositionY = constraints.Top;
            }
            else if (positionY > constraints.Bottom)
            {
                camera.PositionY = constraints.Bottom;
            }
        }

        private void WrapCamera(ref Camera camera)
        {
            float positionX = camera.PositionX;
            float positionY = camera.PositionY;
            Rectangle constraints = camera.Constraints;

            float overshoot = 0;

            if (positionX < constraints.Left)
            {
                overshoot = positionX - constraints.Left;
                camera.PositionX = constraints.Right + overshoot;
            }
            else if (positionX > constraints.Right)
            {
                overshoot = positionX - constraints.Right;
                camera.PositionX = constraints.Left + overshoot;
            }

            if (positionY < constraints.Top)
            {
                overshoot = positionY - constraints.Top;
                camera.PositionY = constraints.Bottom + overshoot;
            }
            else if (positionY > constraints.Bottom)
            {
                overshoot = positionY - constraints.Bottom;
                camera.PositionY = constraints.Top + overshoot;
            }
        }

        #endregion
    }
}
