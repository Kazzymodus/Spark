namespace SparkEngine.Entities
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using SparkEngine.DataStructures;
    using SparkEngine.Level;
    using SparkEngine.Pathfinding;
    using SparkEngine.Rendering;
    using SparkEngine.States;
    using SparkEngine.Tasks;
    using SparkEngine.World;

    public class Subject : WorldObject
    {
        #region Fields

        public const float BaseMoveSpeed = 5f;
        public const float BaseLabourStrength = 1f;

        private float moveSpeed = BaseMoveSpeed;
        private float labourStrength = BaseLabourStrength;

        private Task assignedTask;
        private bool isPerformingTask;

        private List<Vector2> path = new List<Vector2>();
        private Vector2 direction;
        private bool isRestoringGridPosition;

        #endregion

        #region Constructors

        public Subject(Vector2 coordinates, DrawData drawdata, int rotation = RenderHelper.RotationNone)
            : base(coordinates, drawdata)
        {
        }

        #endregion

        #region Properties

        public override bool IsPathBlocker
        {
            get { return false; }
        }

        public bool HasTask
        {
            get { return assignedTask != null; }
        }

        private bool FinishedPath
        {
            get { return path.Count == 0; }
        }

        #endregion

        #region Methods

        public void SetAssignedTask(Task task, bool overridePrevious = false)
        {
            if (assignedTask != null && !overridePrevious)
            {
                throw new Exception("Subject already has a task. Did you mean to override?");
            }
            else
            {
                assignedTask = task;
            }
        }

        public void ClearAssignedTask(TaskClearReason reason)
        {
            assignedTask = null;
            isPerformingTask = false;

            if (!FinishedPath)
            {
                isRestoringGridPosition = true;

                if (path.Count > 1)
                {
                    path.RemoveRange(1, path.Count - 1);
                }
            }
        }

        internal void Update(GameTime gameTime, Map map)
        {
            if (HasTask)
            {
                UpdateTask(gameTime, map);
            }

            if (!FinishedPath)
            {
                MoveDownPath(gameTime);
            }

            if (StateManager.DebugState.IsActive)
            {
                Debug.DebugTiler tiler = StateManager.DebugState.DebugTiler;

                foreach (Vector2 coordinate in path)
                {
                    tiler.AddDebugTile(coordinate, new Color(Color.Red, 0.1f));
                }
            }
        }

        private void UpdateTask(GameTime gameTime, Map map)
        {
            if (isPerformingTask)
            {
                assignedTask.AddLabour(labourStrength * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            else if (FinishedPath && !isRestoringGridPosition)
            {
                GetPath(map, assignedTask.Coordinates, true);
            }
        }

        private void GetPath(Map map, Vector2 destination, bool cullFinalNode = false)
        {
            if (Coordinates == destination)
            {
                return;
            }

            path = PathFinder.GetPath(map, Coordinates, destination);

            if (cullFinalNode)
            {
                path.RemoveAt(path.Count - 1);
            }

            direction = path[0] - Coordinates;
        }

        private void MoveDownPath(GameTime gameTime)
        {
            Translate(direction * moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (path[0] == Coordinates && TileOffset == Vector2.Zero)
            {
                path.RemoveAt(0);

                if (FinishedPath)
                {
                    isRestoringGridPosition = false;

                    if (HasTask)
                    {
                        isPerformingTask = true;
                    }

                    return;
                }

                direction = path[0] - Coordinates;
            }
        }

        #endregion
    }
}
