//namespace SparkEngine.Level
//{
//    using System.Collections.Generic;
//    using Microsoft.Xna.Framework;
//    using Microsoft.Xna.Framework.Graphics;
//    using SparkEngine.Rendering;
//    using SparkEngine.States;
//    using SparkEngine.World;

//    public class ObjectManager
//    {
//        #region Fields

//        private List<WorldObject> drawOrder = new List<WorldObject>();

//        #endregion

//        #region Properties

//        #endregion

//        #region Methods

//        public void RegisterStructure(Structure structure, Map map)
//        {
//            StructureManager.AddStructure(structure);
//            RegisterWorldObject(structure, map);
//        }

//        public void RegisterSubject(Subject subject, Map map)
//        {
//            SubjectManager.AddSubject(subject);
//            RegisterWorldObject(subject, map);
//        }

//        public void RegisterTask(Task task, Map map)
//        {
//            TaskManager.AddTask(task);
//            RegisterWorldObject(task, map);
//        }

//        public void RemoveWorldObject(WorldObject worldObject)
//        {
//            drawOrder.Remove(worldObject);
//        }

//        public WorldObject GetCursorObject(Point mousePoint, Map map)
//        {
//            WorldObject cursorObject = null;
//            int rotations = map.Rotations;

//            foreach (WorldObject worldObject in drawOrder)
//            {
//                Rectangle pixelBounds = worldObject.GetPixelBounds(rotations);

//                if (pixelBounds.Contains(mousePoint))
//                {
//                    Rectangle pixel = new Rectangle(mousePoint - pixelBounds.Location, new Point(1, 1));
//                    byte[] pixelData = new byte[4];
//                    worldObject.DrawData.Texture.GetData(0, pixel, pixelData, 0, 4);

//                    if(pixelData[3] == 255)
//                    {
//                        cursorObject = worldObject;
//                        break;
//                    }
//                }
//            }

//            return cursorObject;
//        }

//        internal void ResetDrawOrderIndividual(WorldObject worldObject, Map map)
//        {
//            int rotations = map.Rotations;

//            drawOrder.Remove(worldObject);

//            int index = GetDrawOrderIndex(worldObject, drawOrder, rotations);

//            drawOrder.Insert(index, worldObject);
//        }

//        internal void ResetDrawOrderAll(Map map)
//        {
//            int rotations = map.Rotations;

//            List<WorldObject> newOrder = new List<WorldObject>();

//            foreach (WorldObject worldObject in drawOrder)
//            {
//                int index = GetDrawOrderIndex(worldObject, newOrder, rotations);
//                newOrder.Insert(index, worldObject);
//            }

//            drawOrder = newOrder;
//        }

//        internal void DrawWorldObjects(SpriteBatch spriteBatch, Map map, Rectangle visibleCoordinates)
//        {
//            foreach (WorldObject worldObject in drawOrder)
//            {
//                worldObject.Draw(spriteBatch, Color.White, map.Rotations);
//            }
//        }

//        private void RegisterWorldObject(WorldObject worldObject, Map map)
//        {
//            int rotations = map.Rotations;
//            int index = GetDrawOrderIndex(worldObject, drawOrder, rotations);

//            drawOrder.Insert(index, worldObject);
//        }

//        private int GetDrawOrderIndex(WorldObject worldObject, List<WorldObject> list, int rotations)
//        {
//            Vector2 newDimensions = worldObject.DrawData.Dimensions;
//            int newHeight = worldObject.GetDrawHeight(rotations);

//            for (int i = 0; i < list.Count; i++)
//            {
//                int oldHeight = list[i].GetDrawHeight(rotations);

//                // Larger structures need to be drawn before smaller ones on the same row.

//                if (newHeight == oldHeight && newDimensions.X > 1)
//                {
//                    Vector2 oldDimensions = list[i].DrawData.Dimensions;

//                    if (newDimensions.X > oldDimensions.X)
//                    {
//                        return i;
//                    }
//                }

//                // Structures that are farther away (higher up) need to be drawn before ones that are closer.

//                if (newHeight < oldHeight)
//                {
//                    return i;
//                }
//            }

//            return list.Count;
//        }

//        #endregion
//    }
//}
