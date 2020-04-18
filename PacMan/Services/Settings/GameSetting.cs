namespace Services.Settings
{
  public class GameSetting
  {
    public static readonly int MinBonus = 500;

    // Maze parameters
    public static readonly int PacManInterval = 500;
    public static readonly int CastInterval = 600;
    public static readonly int CanvasSide = 400;
    public static readonly int PlaceSide = 16;
    public static readonly int MatrixSide = CanvasSide / PlaceSide;
    public static readonly int GameDurationSeconds = 30;

    // Starting coordinates
    public static readonly int PacManStartX = 0;
    public static readonly int PacManStartY = 0;
    public static readonly int BlinkStartX = MatrixSide - 1;
    public static readonly int BlinkStartY = MatrixSide - 1;
    public static readonly int ClydeStartX = MatrixSide - 1;
    public static readonly int ClydeStartY = MatrixSide - 3;
    public static readonly int InkyStartX = MatrixSide - 3;
    public static readonly int InkyStartY = MatrixSide - 1;
    public static readonly int PinkStartX = MatrixSide - 1;
    public static readonly int PinkStartY = MatrixSide - 5;

    // Shifts for goal point relative to PacMan
    public static readonly int BlinkShiftX = 0;
    public static readonly int BlinkShiftY = 0;
    public static readonly int ClydeShiftX = -3;
    public static readonly int ClydeShiftY = 0;
    public static readonly int InkyShiftX = 3;
    public static readonly int InkyShiftY = 0;
    public static readonly int PinkyShiftX = 0;
    public static readonly int PinkyShiftY = -3;

    // Image paths
    public static readonly string PacManImg = "pack://application:,,,/Services;component/Images/PacMan.png";
    public static readonly string BlinkImg = "pack://application:,,,/Services;component/Images/Blink.png";
    public static readonly string ClydeImg = "pack://application:,,,/Services;component/Images/Clyde.png";
    public static readonly string InkyImg = "pack://application:,,,/Services;component/Images/Inky.png";
    public static readonly string PinkyImg = "pack://application:,,,/Services;component/Images/Pinky.png";
    public static readonly string DotImg = "pack://application:,,,/Services;component/Images/Dot.png";
    public static readonly string BlockImg = "pack://application:,,,/Services;component/Images/Block.png";
  }
}
