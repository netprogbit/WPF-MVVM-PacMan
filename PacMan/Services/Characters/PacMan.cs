using Core.Enums;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Core.Abstractions;
using Services.Settings;

namespace Services.Characters
{
  public class PacMan : Image, ICharacter
  {
    public int X { get; set; }
    public int Y { get; set; }

    public Direction RecommendedDirection;
    public Direction CurrentDirection;
    public bool IsVanished { get; set; }

    private readonly Occupation[,] _matrix;

    private int _angle = 0;

    public PacMan(int height, int width, Occupation[,] matrix)
    {
      this.Source = new BitmapImage(new Uri(GameSetting.PacManImg));
      Height = height;
      Width = width;
      _matrix = matrix;

      IsVanished = false;
    }

    public void Vanish()
    {
      IsVanished = true;
      this.Source = null;
    }

    /// <summary>
    /// PacMan eats dot
    /// </summary>
    /// <param name="canvas">Canvas that displays characters</param>
    /// <returns>True value if there was dot</returns>
    public bool EatDot(Canvas canvas)
    {
      if (_matrix[X, Y] != Occupation.Dot)
        return false;

      foreach (var img in canvas.Children)
      {
        var dot = img as Dot;

        if (dot == null || dot.X != X || dot.Y != Y)
          continue;

        canvas.Children.Remove(dot);

        return true;
      }

      return false;
    }

    public void Move()
    {
      if (IsVanished)
        return;

      switch (RecommendedDirection)
      {
        case Direction.Left when X - 1 >= 0 && _matrix[X - 1, Y] != Occupation.Wall:
          _angle = 0;
          X--;
          CurrentDirection = Direction.Left;
          break;
        case Direction.Right when X + 1 < GameSetting.MatrixSide && _matrix[X + 1, Y] != Occupation.Wall:
          _angle = 180;
          X++;
          CurrentDirection = Direction.Right;
          break;
        case Direction.Up when Y - 1 >= 0 && _matrix[X, Y - 1] != Occupation.Wall:
          _angle = 90;
          Y--;
          CurrentDirection = Direction.Up;
          break;
        case Direction.Down when Y + 1 < GameSetting.MatrixSide && _matrix[X, Y + 1] != Occupation.Wall:
          _angle = 270;
          Y++;
          CurrentDirection = Direction.Down;
          break;
        default:
          DirectionSearchContinuation(ref _angle);
          break;
      }

      var rt = new RotateTransform(_angle);
      this.LayoutTransform = rt;
      double oldX = Canvas.GetLeft(this);
      double oldY = Canvas.GetTop(this);
      var animationX = new DoubleAnimation(oldX, X * GameSetting.PlaceSide, TimeSpan.FromMilliseconds(GameSetting.PacManInterval));
      var animationY = new DoubleAnimation(oldY, Y * GameSetting.PlaceSide, TimeSpan.FromMilliseconds(GameSetting.PacManInterval));
      this.BeginAnimation(Canvas.LeftProperty, animationX);
      this.BeginAnimation(Canvas.TopProperty, animationY);
    }

    private void DirectionSearchContinuation(ref int angle)
    {
      switch (CurrentDirection)
      {
        case Direction.Left when X - 1 >= 0 && _matrix[X - 1, Y] != Occupation.Wall:
          angle = 0;
          X--;
          break;
        case Direction.Right when X + 1 < GameSetting.MatrixSide && _matrix[X + 1, Y] != Occupation.Wall:
          angle = 180;
          X++;
          break;
        case Direction.Up when Y - 1 >= 0 && _matrix[X, Y - 1] != Occupation.Wall:
          angle = 90;
          Y--;
          break;
        case Direction.Down when Y + 1 < GameSetting.MatrixSide && _matrix[X, Y + 1] != Occupation.Wall:
          angle = 270;
          Y++;
          break;
      }
    }
  }
}
