﻿using System;
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
    class CameraSystem : ComponentSystem
    {
        public CameraSystem()
            : base(typeof(Camera), typeof(ScreenPosition))
        {

        }

        public override void UpdateIndividual(GameState state, GameTime gameTime, InputHandler input, ComponentBatch components)
        {
            components.GetComponentsMultiType(out Camera camera, out ScreenPosition position);

            // TEMP

            if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                position.ScreenX -= 2;
            }
            if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                position.ScreenY += 2;
            }
            if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                position.ScreenX += 2;
            }
            if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                position.ScreenY -= 2;
            }

            // TEMP

            if (camera.ConstraintMode == CameraConstraints.Constrained)
            {
                ClampCamera(position, camera.Constraints);
            }
            else if (camera.ConstraintMode == CameraConstraints.WrapAround)
            {
                WrapCamera(position, camera.Constraints);
            }
            camera.Transform = Matrix.CreateTranslation(new Vector3(-position, 0f));
        }

        public override void OnRemoveEntity(int entity, GameState state)
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
        private void ClampCamera(ScreenPosition position, Rectangle constraints)
        {
            if (position.ScreenX < constraints.Left)
            {
                position.ScreenX = constraints.Left;
            }
            else if (position.ScreenX > constraints.Right)
            {
                position.ScreenX = constraints.Right;
            }

            if (position.ScreenY < constraints.Top)
            {
                position.ScreenY = constraints.Top;
            }
            else if (position.ScreenY > constraints.Bottom)
            {
                position.ScreenY = constraints.Bottom;
            }
        }

        private void WrapCamera(ScreenPosition position, Rectangle constraints)
        {
            int overshoot = 0;

            if (position.ScreenX < constraints.Left)
            {
                overshoot = (int)position.ScreenX - constraints.Left;
                position.ScreenX = constraints.Right + overshoot;
            }
            else if (position.ScreenX > constraints.Right)
            {
                overshoot = (int)position.ScreenX - constraints.Right;
                position.ScreenX = constraints.Left + overshoot;
            }

            if (position.ScreenY < constraints.Top)
            {
                overshoot = (int)position.ScreenY - constraints.Top;
                position.ScreenY = constraints.Bottom + overshoot;
            }
            else if (position.ScreenY > constraints.Bottom)
            {
                overshoot = (int)position.ScreenY - constraints.Bottom;
                position.ScreenY = constraints.Top + overshoot;
            }
        }

        #endregion
    }
}
