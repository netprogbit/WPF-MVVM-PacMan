using Core.Abstractions;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace Services.Movables
{
  /// <summary>
  /// Default cost behavior
  /// </summary>
  public class DefaultMovableCost : IMovableCost
  {
    public string Name { get; set; }

    private readonly Occupation[,] _matrix;

    public DefaultMovableCost(Occupation[,] matrix)
    {
      _matrix = matrix;
      Name = "Default cost movement";
    }

    /// <summary>
    /// Method calculates coordinates of next step
    /// </summary>
    /// <param name="start">Starting point coordinates</param>
    /// <param name="goal">Destination coordinates</param>
    /// <returns>Coordinates of next step</returns>
    public Point Move(Point start, Point goal)
    {
      return FindPath(_matrix, start, goal);
    }

    // Method finds next step coordinates 
    private static Point FindPath(Occupation[,] matrix, Point start, Point goal)
    {
      var closedSet = new Collection<PathNode>();
      var openSet = new Collection<PathNode>();

      PathNode startNode = new PathNode()
      {
        Position = start,
        CameFrom = null,
        PathLengthFromStart = 0,
        HeuristicEstimatePathLength = GetHeuristicPathLength(start, goal)
      };

      openSet.Add(startNode);

      while (openSet.Count > 0)
      {
        var currentNode = openSet.OrderBy(node => node.EstimateFullPathLength).First();

        if (currentNode.Position == goal)
        {
          List<Point> points = GetPathForNode(currentNode);

          if (points.Count == 1)
            return points[0]; // Is at destination

          return points[points.Count - 2]; // Return point of next step
        }

        openSet.Remove(currentNode);
        closedSet.Add(currentNode);

        foreach (var neighborNode in GetNeighbors(currentNode, goal, matrix))
        {
          if (closedSet.Count(node => node.Position == neighborNode.Position) > 0)
            continue;

          var openNode = openSet.FirstOrDefault(node => node.Position == neighborNode.Position);

          if (openNode == null)
          {
            openSet.Add(neighborNode);
          }
          else
          {
            if (openNode.PathLengthFromStart <= neighborNode.PathLengthFromStart)
              continue;

            openNode.CameFrom = currentNode;
            openNode.PathLengthFromStart = neighborNode.PathLengthFromStart;
          }
        }
      }

      return start; // No move
    }

    // Distance between neighbors
    private static int GetDistanceBetweenNeighbors()
    {
      return 1;
    }

    // Approximate estimate of the distance to the target
    private static int GetHeuristicPathLength(Point from, Point to)
    {
      return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
    }

    // Getting a list of neighbors for a point
    private static Collection<PathNode> GetNeighbors(PathNode pathNode, Point goal, Occupation[,] matrix)
    {
      var result = new Collection<PathNode>();

      Point[] neighborPoints = new Point[4];
      neighborPoints[0] = new Point(pathNode.Position.X + 1, pathNode.Position.Y);
      neighborPoints[1] = new Point(pathNode.Position.X - 1, pathNode.Position.Y);
      neighborPoints[2] = new Point(pathNode.Position.X, pathNode.Position.Y + 1);
      neighborPoints[3] = new Point(pathNode.Position.X, pathNode.Position.Y - 1);

      foreach (var point in neighborPoints)
      {
        if (point.X < 0 || point.X >= matrix.GetLength(0))
          continue;

        if (point.Y < 0 || point.Y >= matrix.GetLength(1))
          continue;

        if (matrix[point.X, point.Y] == Occupation.Wall)
          continue;

        var neighborNode = new PathNode()
        {
          Position = point,
          CameFrom = pathNode,
          PathLengthFromStart = pathNode.PathLengthFromStart + GetDistanceBetweenNeighbors(),
          HeuristicEstimatePathLength = GetHeuristicPathLength(point, goal)
        };

        result.Add(neighborNode);
      }
      return result;
    }

    // Getting route
    private static List<Point> GetPathForNode(PathNode pathNode)
    {
      var result = new List<Point>();
      var currentNode = pathNode;

      while (currentNode != null)
      {
        result.Add(currentNode.Position);
        currentNode = currentNode.CameFrom;
      }

      return result;
    }
  }

  public class PathNode
  {
    public Point Position { get; set; } // Point coordinates
    public int PathLengthFromStart { get; set; } // Path length from start
    public PathNode CameFrom { get; set; } // Previous node from which you came to this point
    public int HeuristicEstimatePathLength { get; set; } // Approximate distance to the target        
    public int EstimateFullPathLength => PathLengthFromStart + HeuristicEstimatePathLength; // Expected total distance to the target
  }
}
