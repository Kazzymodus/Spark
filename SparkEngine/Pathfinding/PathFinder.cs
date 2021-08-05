//namespace SparkEngine.Pathfinding
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using Microsoft.Xna.Framework;
//    using SparkEngine.Level;
//    using SparkEngine.Rendering;
//    using SparkEngine.States;

//    public static class PathFinder
//    {
//        #region Fields

//        private static List<PathNode> openCells = new List<PathNode>();
//        private static List<PathNode> closedCells = new List<PathNode>();

//        #endregion

//        #region Methods

//        public static List<Vector2> GetPath(Map map, Vector2 origin, Vector2 destination)
//        {
//            PathNode endNode = FindPath(map, origin, destination);

//            if (endNode == null)
//            {
//                throw new Exception("Cannot find a path.");
//            }

//            List<PathNode> path = new List<PathNode>();
//            PathNode checkNode = endNode;

//            while (checkNode != null)
//            {
//                path.Add(checkNode);
//                checkNode = checkNode.Parent;
//            }

//            List<Vector2> coordinates = new List<Vector2>();

//            for (int i = path.Count - 1; i >= 0; i--)
//            {
//                coordinates.Add(path[i].Coordinates);
//            }

//            return coordinates;
//        }

//        private static PathNode FindPath(Map map, Vector2 origin, Vector2 destination)
//        {
//            Reset();

//            PathNode currentNode = new PathNode(origin);
//            int gScore = 0;

//            openCells.Add(currentNode);

//            while (openCells.Count > 0)
//            {
//                double minF = openCells.Min(x => x.FScore);
//                currentNode = openCells.First(x => x.FScore == minF);

//                openCells.Remove(currentNode);
//                closedCells.Add(currentNode);

//                if (currentNode.Coordinates == destination)
//                {
//                    return currentNode;
//                }

//                List<PathNode> adjacentNodes = GetAdjacentNodes(map, currentNode.Coordinates);
//                gScore++;

//                foreach (PathNode adjacentNode in adjacentNodes)
//                {
//                    if (closedCells.Any(c => c.Coordinates == adjacentNode.Coordinates))
//                    {
//                        continue;
//                    }

//                    Vector2 direction = adjacentNode.Coordinates - currentNode.Coordinates;

//                    // This ensures diagonal paths aren't obstructed.
//                    // Is bugged, disabled until I fix it.

//                    //if (direction.X != 0 && direction.Y != 0)
//                    //{
//                    //    if (StateManager.MapState.CurrentMap.Terrain.IsOccupiedTile(currentNode.Coordinates + new Vector2(direction.X, 0)) || StateManager.MapState.CurrentMap.Terrain.IsOccupiedTile(currentNode.Coordinates + new Vector2(0, direction.Y)))
//                    //    {
//                    //        continue;
//                    //    }
//                    //}

//                    PathNode oldNode = openCells.FirstOrDefault(n => n.Coordinates == adjacentNode.Coordinates); // Is any of the adjacent nodes already in the Open List?

//                    if (oldNode == null)
//                    {
//                        // If not, add it to the list.

//                        adjacentNode.SetScores(gScore, currentNode, new PathNode(destination));

//                        openCells.Add(adjacentNode);
//                    }
//                    else
//                    {
//                        // If so, update its scores if it's part of a faster path.

//                        adjacentNode.SetScores(gScore, currentNode, new PathNode(destination));

//                        if (gScore + adjacentNode.Heuristic < oldNode.FScore)
//                        {
//                            int index = openCells.IndexOf(oldNode);
//                            openCells[index] = adjacentNode;
//                        }
//                    }
//                }
//            }

//            return null;
//        }

//        private static void Reset()
//        {
//            openCells.Clear();
//            closedCells.Clear();
//        }

//        private static List<PathNode> GetAdjacentNodes(Map map, Vector2 origin)
//        {
//            int x = (int)origin.X;
//            int y = (int)origin.Y;

//            List<PathNode> adjacentTiles = new List<PathNode>
//            {
//                new PathNode(x - 1, y - 1),
//                new PathNode(x - 1, y),
//                new PathNode(x - 1, y + 1),
//                new PathNode(x, y - 1),
//                new PathNode(x, y + 1),
//                new PathNode(x + 1, y - 1),
//                new PathNode(x + 1, y),
//                new PathNode(x + 1, y + 1)
//            };

//            return adjacentTiles.Where(node => !map.Terrain.IsWithinTerrainBounds(node.Coordinates) && !map.Terrain.IsBlockingTile(node.Coordinates)).ToList();
//        }

//        #endregion
//    }
//}

