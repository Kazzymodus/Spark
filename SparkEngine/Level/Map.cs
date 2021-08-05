//namespace SparkEngine.Level
//{
//    using System.Collections.Generic;
//    using Microsoft.Xna.Framework;
//    using Microsoft.Xna.Framework.Graphics;
//    using SparkEngine.Rendering;
//    using SparkEngine.States;
//    using SparkEngine.World;

//    public class Map
//    {
//        #region Constructors

//        public Map(TileData tileData, MapData mapData)
//        {
//            Terrain = new Terrain(tileData, mapData.Dimensions);
//        }

//        #endregion

//        #region Properties


//        public Terrain Terrain { get; }

//        public ObjectManager ObjectManager { get; } = new ObjectManager();

//        #endregion

//        #region Methods

//        public void Rotate(int rotations, Camera camera)
//        {
//            int modulatedRotations = rotations % 4;

//            Rotations += modulatedRotations + 4; // In case a negative rotation is passed.
//            Rotations %= 4;

//            camera.RotateCamera(modulatedRotations);

//            ObjectManager.ResetDrawOrderAll(this);
//        }

//        public Vector2 GetCursorCoordinates(Camera camera)
//        {
//            Vector2 coordinate = RenderHelper.PixelsToCoords(camera.MouseWorldPosition);

//            for (int i = 0; i < Rotations; i++)
//            {
//                Vector2 oldCoord = coordinate;

//                coordinate.X = oldCoord.Y;
//                coordinate.Y = (Terrain.Dimensions.X - 1) - oldCoord.X;
//            }

//            return coordinate;
//        }

//        public void SpawnStructure(Structure structure)
//        {
//            Terrain.OccupyTiles(structure.Coordinates, structure);

//            ObjectManager.RegisterStructure(structure, this);
//        }

//        public void SpawnSubject(Vector2 coordinates, DrawData drawData, int rotation = RenderHelper.RotationNone)
//        {
//            Subject subject = new Subject(coordinates, drawData, rotation);
//            ObjectManager.RegisterSubject(subject, this);
//        }

//        public void CreateTask(Task task)
//        {
//            ObjectManager.RegisterTask(task, this);
//            Terrain.OccupyTiles(task.Coordinates, task);
//        }

//        public Vector2 RotateCoordsInMap(Vector2 coords)
//        {
//            Vector2 rotatedCoords = coords;

//            for (int i = 0; i < Rotations; i++)
//            {
//                Vector2 oldCoords = rotatedCoords;

//                // Rotate counterclockwise because we need to establish where the coord is GOING to be.

//                rotatedCoords.X = (Terrain.Dimensions.X - 1) - oldCoords.Y;
//                rotatedCoords.Y = oldCoords.X;
//            }

//            return rotatedCoords;
//        }

//        internal void DrawTerrain(SpriteBatch spriteBatch, Map map, Rectangle visibleCoordinates, bool drawGrid)
//        {
//            Terrain.DrawTiles(spriteBatch, map, visibleCoordinates, drawGrid);
//        }

//        #endregion
//    }
//}

