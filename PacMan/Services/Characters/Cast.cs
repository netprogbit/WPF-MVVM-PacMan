using Core.Abstractions;
using Core.Enums;
using Services.Settings;
using System;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Services.Characters
{
  public class Cast : System.Windows.Controls.Image, ICast
  {
    public int X { get; set; }
    public int Y { get; set; }

    public Occupation Owner { get; set; } = Occupation.Dot; // Place owner

    private readonly IMovableCost _movableCost;
    private readonly int _shiftPacManX;
    private readonly int _shiftPacManY;
    private readonly Occupation[,] _matrix;

    public Cast(string imagePath, int height, int width, IMovableCost movableCost, Occupation[,] matrix, int shiftPacManX, int shiftPacManY)
    {
      this.Source = new BitmapImage(new Uri(imagePath));
      Height = height;
      Width = width;
      _movableCost = movableCost;
      _matrix = matrix;
      _shiftPacManX = shiftPacManX;
      _shiftPacManY = shiftPacManY;
    }

    public void Move(int pacManX, int pacManY)
    {
      var startPoint = new Point(X, Y);
      var goalPoint = GetFreePlaceCoordinates(pacManX, pacManY, _shiftPacManX, _shiftPacManY);
      var point = _movableCost.Move(startPoint, goalPoint);
      X = point.X;
      Y = point.Y;
      Owner = _matrix[X, Y]; // Save owner
      var oldX = Canvas.GetLeft(this);
      var oldY = Canvas.GetTop(this);
      var animationX = new DoubleAnimation(oldX, X * GameSetting.PlaceSide, TimeSpan.FromMilliseconds(GameSetting.CastInterval));
      var animationY = new DoubleAnimation(oldY, Y * GameSetting.PlaceSide, TimeSpan.FromMilliseconds(GameSetting.CastInterval));
      this.BeginAnimation(Canvas.LeftProperty, animationX);
      this.BeginAnimation(Canvas.TopProperty, animationY);
    }

    private Point GetFreePlaceCoordinates(int pacManX, int pacManY, int shiftPacManX, int shiftPacManY)
    {
      bool isHorizontal = true;

      while (pacManX + shiftPacManX < 0 || pacManX + shiftPacManX >= GameSetting.MatrixSide ||
             pacManY + shiftPacManY < 0 || pacManY + shiftPacManY >= GameSetting.MatrixSide ||
             _matrix[pacManX + shiftPacManX, pacManY + shiftPacManY] == Occupation.Wall)
      {
        if (isHorizontal)
        {
          if (shiftPacManX < 0)
            shiftPacManX++;

          if (shiftPacManX > 0)
            shiftPacManX--;

          isHorizontal = false;
        }
        else
        {
          if (shiftPacManY < 0)
            shiftPacManY++;

          if (shiftPacManY > 0)
            shiftPacManY--;

          isHorizontal = true;
        }
      }

      return new Point(pacManX + shiftPacManX, pacManY + shiftPacManY);
    }

    public bool TouchedPacMan(int pacManX, int pacManY)
    {
      if (X == pacManX && Y == pacManY)
        return true;

      return false;
    }
  }
}
