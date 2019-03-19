//namespace SparkEngine.Pathfinding
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using Microsoft.Xna.Framework;
//    using SparkEngine.Rendering;
//    using SparkEngine.States;
//    using SparkEngine.World;

//    public static class PathFinderTest
//    {
//        #region Fields

//        public static List<PathNode> openCells = new List<PathNode>(); // PRIVATE
//        public static List<PathNode> closedCells = new List<PathNode>(); // PRIVATE

//        #endregion

//        #region Methods

//        public static List<Vector2> GetPath(Terrain terrain, Vector2 origin, Vector2 destination)
//        {
//            Reset();

//            PathNode endNode = FindPath(terrain, origin, destination);

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

//        static int singleStepG = 0;
//        static List<PathNode> singleStepLowestFScores = new List<PathNode>(); 

//        public static void FindPathSingleStep(Terrain terrain)
//        {
//            PathNode currentNode = new PathNode(new Vector2(0, 1));
//            Vector2 destination = new Vector2(48, 48);

//            if (singleStepG == 0)
//            {
//                openCells.Add(currentNode);
//            }

//            if (singleStepLowestFScores.Count == 0)
//            {
//                double minF = openCells.Min(c => c.FScore);
//                singleStepLowestFScores = openCells.Where(c => c.FScore == minF).ToList();
//                singleStepG++;
//            }

//            if (singleStepLowestFScores.Count > 1)
//            {
//                int maxG = singleStepLowestFScores.Max(c => c.GScore);
//                currentNode = singleStepLowestFScores.First(c => c.GScore == maxG);
//            }
//            else
//            {
//                currentNode = singleStepLowestFScores[0];
//            }

//            openCells.Remove(currentNode);
//            closedCells.Add(currentNode);

//            if (currentNode.Coordinates == destination)
//            {
//                return;
//            }

//            List<PathNode> adjacentNodes = GetAdjacentNodes(currentNode.Coordinates);

//            foreach (PathNode adjacentNode in adjacentNodes)
//            {
//                if (closedCells.Any(c => c.Coordinates == adjacentNode.Coordinates))
//                {
//                    continue;
//                }

//                Vector2 direction = adjacentNode.Coordinates - currentNode.Coordinates;

//                // This ensures diagonal paths aren't obstructed.

//                if (direction.X != 0 && direction.Y != 0)
//                {
//                    Vector2 xFlank = currentNode.Coordinates + new Vector2(direction.X, 0);
//                    Vector2 yFlank = currentNode.Coordinates + new Vector2(0, direction.Y);

//                    if (terrain.IsOccupiedTile(xFlank) || terrain.IsOccupiedTile(yFlank))
//                    {
//                        continue;
//                    }
//                }

//                PathNode oldNode = openCells.FirstOrDefault(n => n.Coordinates == adjacentNode.Coordinates); // Is any of the adjacent nodes already in the Open List?

//                if (oldNode == null)
//                {
//                    // If not, add it to the list.

//                    adjacentNode.SetScores(singleStepG, currentNode, new PathNode(destination));

//                    openCells.Add(adjacentNode);
//                }
//                else
//                {
//                    // If so, update its scores if it's part of a faster path.

//                    adjacentNode.SetScores(singleStepG, currentNode, new PathNode(destination));

//                    if (singleStepG + adjacentNode.Heuristic < oldNode.FScore)
//                    {
//                        int index = openCells.IndexOf(oldNode);
//                        openCells[index] = adjacentNode;
//                    }
//                }
//            }

//            singleStepLowestFScores.RemoveAt(0);
//        }

//        public static PathNode FindPath(Terrain terrain, Vector2 origin, Vector2 destination)
//        {
//            PathNode currentNode = new PathNode(origin);
//            int g = 0;

//            openCells.Add(currentNode);

//            while (openCells.Count > 0)
//            {
//                double minF = openCells.Min(c => c.FScore);
//                List<PathNode> lowestFScores = openCells.Where(c => c.FScore == minF).ToList();
//                g++;

//                foreach (PathNode minimumF in lowestFScores)
//                {

//                    currentNode = minimumF;

//                    openCells.Remove(currentNode);
//                    closedCells.Add(currentNode);

//                    if (currentNode.Coordinates == destination)
//                    {
//                        return currentNode;
//                    }

//                    List<PathNode> adjacentNodes = GetAdjacentNodes(currentNode.Coordinates);

//                    foreach (PathNode adjacentNode in adjacentNodes)
//                    {
//                        if (closedCells.Any(c => c.Coordinates == adjacentNode.Coordinates))
//                        {
//                            continue;
//                        }

//                        Vector2 direction = adjacentNode.Coordinates - currentNode.Coordinates;

//                        // This ensures diagonal paths aren't obstructed.

//                        if (direction.X != 0 && direction.Y != 0)
//                        {
//                            Vector2 xFlank = currentNode.Coordinates + new Vector2(direction.X, 0);
//                            Vector2 yFlank = currentNode.Coordinates + new Vector2(0, direction.Y);

//                            if (terrain.IsOccupiedTile(xFlank) || terrain.IsOccupiedTile(yFlank))
//                            {
//                                continue;
//                            }
//                        }

//                        PathNode oldNode = openCells.FirstOrDefault(n => n.Coordinates == adjacentNode.Coordinates); // Is any of the adjacent nodes already in the Open List?

//                        if (oldNode == null)
//                        {
//                            // If not, add it to the list.

//                            adjacentNode.SetScores(g, currentNode, new PathNode(destination));

//                            openCells.Add(adjacentNode);
//                        }
//                        else
//                        {
//                            // If so, update its scores if it's part of a faster path.

//                            adjacentNode.SetScores(g, currentNode, new PathNode(destination));

//                            if (g + adjacentNode.Heuristic < oldNode.FScore)
//                            {
//                                int index = openCells.IndexOf(oldNode);
//                                openCells[index] = adjacentNode;
//                            }
//                        }
//                    }
//                }
//            }

//            return null;
//        }

//        public static void Reset()
//        {
//            openCells.Clear();
//            closedCells.Clear();
//            singleStepG = 0;
//        }

//        private static List<PathNode> GetAdjacentNodes(Vector2 origin)
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

//            return null;// adjacentTiles.Where(node => !map.Terrain.IsOutOfBounds(node.Coordinates)).ToList();// && !StateManager.MapState.CurrentMap.Terrain.IsBlockingTile(node.Coordinates)).ToList();
//        }

//        #endregion
//    }
//}
