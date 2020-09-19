using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;

    private class PathNode
    {
        public int row;
        public int col;
        public int gCost;
        public int hCost;
        public int fCost;
        public bool isWalkable = true;
        public PathNode previous;

        public PathNode(int row, int col)
        {
            this.row = row;
            this.col = col;
        }

        public override string ToString()
        {
            return row + "," + col;
        }

        public bool IsEqual(PathNode otherNode)
        {
            return row == otherNode.row && col == otherNode.col;
        }

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }
    }

    private List<PathNode> open;
    private List<PathNode> closed;
    private Dictionary<string, PathNode> grid = new Dictionary<string, PathNode>();

    public Pathfinding(Map map)
    {
        UpdateMap(map);
    }

    public void UpdateMap(Map map)
    {
        for (int i = 0; i < map.rows; i++)
        {
            for (int j = 0; j < map.cols; j++)
            {
                PathNode p = new PathNode(i, j);
                if (map.IsObjectAtPos(i, j)) p.isWalkable = false;
                grid[p.ToString()] = p;
            }
        }
    }

    public List<int[]> AStarSearch(int[] startPos, int[] endPos)
    {
        string startKey = startPos[0] + "," + startPos[1];
        string endKey = endPos[0] + "," + endPos[1];
        PathNode startNode = grid[startKey];
        PathNode endNode = grid[endKey];
        open = new List<PathNode> { startNode };
        closed = new List<PathNode>();

        foreach (PathNode p in grid.Values) {
            p.gCost = int.MaxValue;
            p.CalculateFCost();
            p.previous = null;
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        startNode.isWalkable = true;
        endNode.isWalkable = true;

        while (open.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(open);
            if (currentNode.IsEqual(endNode))
            {
                return CalculatePath(endNode);
            }

            open.Remove(currentNode);
            closed.Add(currentNode);

            foreach (PathNode neighborNode in GetNeighborList(currentNode))
            {
                if (closed.Contains(neighborNode)) continue;
                if (!neighborNode.isWalkable) continue;

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighborNode);
                if (tentativeGCost < neighborNode.gCost)
                {
                    neighborNode.previous = currentNode;
                    neighborNode.gCost = tentativeGCost;
                    neighborNode.hCost = CalculateDistanceCost(neighborNode, endNode);
                    neighborNode.CalculateFCost();

                    if (!open.Contains(neighborNode))
                    {
                        open.Add(neighborNode);
                    }
                }
            }
        }

        // Out of nodes on the open list
        return null;
    }

    List<PathNode> GetNeighborList(PathNode currentNode) {
        int[][] directions = new int[][]
        {
            new int[] { 0, - 1 },
            new int[] { 1, 0 },
            new int[] { -1, 0 },
            new int[] { 0, 1 }
        };
        List<PathNode> neighbors = new List<PathNode>();
        foreach (int[] direction in directions)
        {
            int[] neighborKey = new int[]
            {
                currentNode.row + direction[0],
                currentNode.col + direction[1]
            };
            if (GameManager.instance.map.CheckWithinBounds(neighborKey))
            {
                neighbors.Add(grid[neighborKey[0] + "," + neighborKey[1]]);
            }
        }
        return neighbors;
    }

    List<int[]> CalculatePath(PathNode node)
    {
        List<int[]> path = new List<int[]>();
        path.Add(new int[] { node.row, node.col });
        PathNode curr = node;
        while (curr.previous != null)
        {
            curr = curr.previous;
            path.Add(new int[] { curr.row, curr.col });
        }
        path.Reverse();
        return path;
    }

    int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int[] aPos = new int[] { a.row, a.col };
        int[] bPos = new int[] { b.row, b.col };
        int manhattanDistance = Map.GetDistanceBetweenPoints(aPos, bPos);
        return manhattanDistance * MOVE_STRAIGHT_COST;
    }

    PathNode GetLowestFCostNode(List<PathNode> list)
    {
        PathNode lowestFCostNode = null;
        foreach (PathNode p in list)
        {
            if (lowestFCostNode == null || p.fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = p;
            }
        }
        return lowestFCostNode;
    }
}
