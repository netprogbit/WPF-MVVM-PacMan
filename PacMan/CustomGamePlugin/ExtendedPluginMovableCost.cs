using System;
using Core.Enums;
using System.ComponentModel.Composition;
using System.Drawing;
using Core.Abstractions;

namespace CustomGamePlugin
{
  /// <summary>
  /// Custom extended movable cost plugin
  /// </summary>
  [Export(typeof(IMovableCost))]
  public class ExtendedPluginMovableCost : IMovableCost
  {
    public string Name { get; set; }

    private readonly Occupation[,] _matrix;

    private Random _random = new Random();

    [ImportingConstructor]
    public ExtendedPluginMovableCost([Import("matrix")] Occupation[,] matrix)
    {
      _matrix = matrix;
      Name = "Trial cost movement";
    }

    public Point Move(Point start, Point goal)
    {
      return FindPath(_matrix, start, goal);
    }

    private Point FindPath(Occupation[,] matrix, Point start, Point goal)
    {
      int x = _random.Next(0, matrix.GetLength(1) - goal.X);
      int y = _random.Next(0, matrix.GetLength(0) - goal.Y);

      if (matrix[x, y] != Occupation.Wall)
        return new Point(x, y);

      return start;
    }
  }
}
