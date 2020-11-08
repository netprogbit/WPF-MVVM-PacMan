using DataLayer;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using DataLayer.Entities;
using System.Collections.Generic;
using System.Linq;
using Services.Plugin;
using Services.Movables;
using Core.Enums;
using Services.Characters;
using Services.Factories;
using Core.Abstractions;
using PacMan.Helpers;
using Services.Settings;

namespace PacMan.Models
{
  /// <summary>
  /// Game features
  /// </summary>
  class GameModel : INotifyPropertyChanged
  {
    private Canvas _gameCanvas;
    public Canvas GameCanvas { get => _gameCanvas; set { _gameCanvas = value; OnPropertyChanged(); } }    

    private TimeSpan _time;
    public TimeSpan Time { get => _time; set { _time = value; OnPropertyChanged(); } }

    private int _score;
    public int Score { get => _score; set { _score = value; OnPropertyChanged(); } }

    private List<IMovableCost> _plugins;
    public List<IMovableCost> Plugins { get => _plugins; set { _plugins = value; OnPropertyChanged(); } }

    private IMovableCost _currentPlugin;
    public IMovableCost CurrentPlugin { get => _currentPlugin; set { _currentPlugin = value; OnPropertyChanged(); } }

    private List<Player> _players;
    public List<Player> Players { get => _players; set { _players = value; OnPropertyChanged(); } }

    private List<Player> _topScorers;
    public List<Player> TopScorers { get => _topScorers; set { _topScorers = value; OnPropertyChanged(); } }

    private int _selectedPlayerIndex;
    public int SelectedPlayerIndex { get => _selectedPlayerIndex; set { _selectedPlayerIndex = value; OnPropertyChanged(); } }

    private bool _isGameOver = true;
    public bool IsGameOver { get => _isGameOver; set { _isGameOver = value; OnPropertyChanged(); } }

    private string _playPauseContent = StringHelper.PlayString;
    public string PlayPauseContent { get => _playPauseContent; set { _playPauseContent = value; OnPropertyChanged(); } }

    private string _message;
    public string Message { get => _message; set { _message = value; OnPropertyChanged(); } }

    private readonly MazeFactory _mazeFactory = new MazeFactory();
    private readonly List<ICast> _casts = new List<ICast>();
    private Occupation[,] _matrix;
    private Services.Characters.PacMan _pacMan;
    private DispatcherTimer _timerPacMan;
    private DispatcherTimer _timerCasts;

    public GameModel()
    {
      Initialize();
    }

    private void DrawMaze()
    {
      for (var i = 0; i < GameSetting.MatrixSide; i++)
        for (var j = 0; j < GameSetting.MatrixSide; j++)
        {
          if (_matrix[i, j] == Occupation.Wall)
            PlacementBlock(i, j);
          else
            PlacementDot(i, j);
        }
    }

    private void PlacementBlock(int x, int y)
    {
      var block = new Block(GameSetting.BlockImg, GameSetting.PlaceSide, GameSetting.PlaceSide, x, y);
      Locate(block, x, y);
      TakePlace(x, y, Occupation.Wall);
    }

    private void PlacementDot(int x, int y)
    {
      var dot = new Dot(GameSetting.DotImg, GameSetting.PlaceSide, GameSetting.PlaceSide, x, y);
      Locate(dot, x, y);
      TakePlace(x, y, Occupation.Dot);
    }

    public void New()
    {
      IsGameOver = false;
      PacManInitialize();
      CastsInitialize();
      Time = TimeSpan.FromSeconds(GameSetting.GameDurationSeconds);
      Score = 0;
    }

    private bool _isPlaying = false;

    public void PlayPause()
    {
      if (_isPlaying)
        Stop();
      else
        Start();
    }

    public void Start()
    {
      _timerPacMan.Start();
      _timerCasts.Start();
      PlayPauseContent = StringHelper.PauseString;
      _isPlaying = true;
    }

    public void Stop()
    {
      _timerPacMan.Stop();
      _timerCasts.Stop();
      PlayPauseContent = StringHelper.PlayString;
      _isPlaying = false;
    }

    public void GameOver()
    {
      Stop();
      GameCanvas.Children.Clear();

      if (Players[SelectedPlayerIndex].Score < _score)
        Players[SelectedPlayerIndex].Score = _score;

      UpdatePlayers(Players);
      Initialize();
      IsGameOver = true;
      SendMessage(StringHelper.GameOver);
    }

    public void AddPlayer(string name)
    {
      using (var unitOfWork = new UnitOfWork())
      {
        Player player = new Player { Name = name };
        unitOfWork.Players.Create(player);
        unitOfWork.Save();
        PlayersInitialize(player.Id);
      }
    }

    public void DeletePlayer()
    {
      using (var unitOfWork = new UnitOfWork())
      {
        unitOfWork.Players.Delete(Players[SelectedPlayerIndex].Id);
        unitOfWork.Save();
      }

      PlayersInitialize();
    }

    public void PressLeftKey()
    {
      _pacMan.RecommendedDirection = Direction.Left;
    }

    public void PressRightKey()
    {
      _pacMan.RecommendedDirection = Direction.Right;
    }

    public void PressUpKey()
    {
      _pacMan.RecommendedDirection = Direction.Up;
    }

    public void PressDownKey()
    {
      _pacMan.RecommendedDirection = Direction.Down;
    }

    private void UpdatePlayers(IEnumerable<Player> players)
    {
      using (var unitOfWork = new UnitOfWork())
      {
        foreach (var p in players)
          unitOfWork.Players.Update(p);

        unitOfWork.Save();
      }
    }

    private void Initialize()
    {
      CanvasInitialize();
      TimersInitialize();
      MazeInitialize();
      PlayersInitialize();
      PluginsInitialize();
    }

    private void PlayersInitialize(int playerId = 0)
    {
      using (var unitOfWork = new UnitOfWork())
      {
        Players = unitOfWork.Players.FindAll().ToList();
      }      

      if (playerId == 0)
        SelectedPlayerIndex = 0;
      else
        SelectedPlayerIndex = Players.IndexOf(Players.SingleOrDefault(p => p.Id == playerId));

      TopScorers = Players.OrderByDescending(p => p.Score).Take(5).ToList(); // Top 5 Scorers list creating
    }



    private void CanvasInitialize()
    {
      GameCanvas = new Canvas { Height = GameSetting.CanvasSide, Width = GameSetting.CanvasSide, Background = Brushes.Black };
    }

    private void MazeInitialize()
    {
      _matrix = _mazeFactory.Create(GameSetting.MatrixSide, GameSetting.MatrixSide);
      DrawMaze();
    }

    private void PluginsInitialize()
    {
      var importer = new Importer();
      string path = System.IO.Path.Combine(Environment.CurrentDirectory, "Plugins");
      var plugins = new List<IMovableCost>(importer.GetPlugins(path, _matrix));
      var defaultMovable = new DefaultMovableCost(_matrix);
      plugins.Add(defaultMovable);
      Plugins = plugins;
      CurrentPlugin = defaultMovable;
    }

    private void TimersInitialize()
    {
      _timerPacMan = new DispatcherTimer();
      _timerPacMan.Tick += TimerPacManTick;
      _timerPacMan.Interval = TimeSpan.FromMilliseconds(GameSetting.PacManInterval);

      _timerCasts = new DispatcherTimer();
      _timerCasts.Tick += TimerCastTick;
      _timerCasts.Interval = TimeSpan.FromMilliseconds(GameSetting.CastInterval);
    }

    private void CastsInitialize()
    {
      _casts.Clear();

      var cast = new Cast(GameSetting.BlinkImg, GameSetting.PlaceSide, GameSetting.PlaceSide, CurrentPlugin, _matrix, GameSetting.BlinkShiftX, GameSetting.BlinkShiftY);
      Locate(cast, GameSetting.BlinkStartX, GameSetting.BlinkStartY);
      TakePlace(GameSetting.BlinkStartX, GameSetting.BlinkStartY, Occupation.Cast);
      _casts.Add(cast);

      cast = new Cast(GameSetting.ClydeImg, GameSetting.PlaceSide, GameSetting.PlaceSide, CurrentPlugin, _matrix, GameSetting.ClydeShiftX, GameSetting.ClydeShiftY);
      Locate(cast, GameSetting.ClydeStartX, GameSetting.ClydeStartY);
      TakePlace(GameSetting.ClydeStartX, GameSetting.ClydeStartY, Occupation.Cast);
      _casts.Add(cast);

      cast = new Cast(GameSetting.InkyImg, GameSetting.PlaceSide, GameSetting.PlaceSide, CurrentPlugin, _matrix, GameSetting.InkyShiftX, GameSetting.InkyShiftY);
      Locate(cast, GameSetting.InkyStartX, GameSetting.InkyStartY);
      TakePlace(GameSetting.InkyStartX, GameSetting.InkyStartY, Occupation.Cast);
      _casts.Add(cast);

      cast = new Cast(GameSetting.PinkyImg, GameSetting.PlaceSide, GameSetting.PlaceSide, CurrentPlugin, _matrix, GameSetting.PinkyShiftX, GameSetting.PinkyShiftY);
      Locate(cast, GameSetting.PinkStartX, GameSetting.PinkStartY);
      TakePlace(GameSetting.PinkStartX, GameSetting.PinkStartY, Occupation.Cast);
      _casts.Add(cast);
    }

    private void TimerCastTick(object sender, EventArgs e)
    {
      foreach (var cast in _casts)
      {
        TakePlace(cast.X, cast.Y, cast.Owner); // Return place owner
        cast.Move(_pacMan.X, _pacMan.Y);
        TakePlace(_pacMan.X, _pacMan.Y, Occupation.Cast); // Taking place

        if (cast.TouchedPacMan(_pacMan.X, _pacMan.Y))
          _pacMan.IsVanished = true;
      }
    }

    private void PacManInitialize()
    {
      _pacMan = new Services.Characters.PacMan(GameSetting.PlaceSide, GameSetting.PlaceSide, _matrix);
      Locate(_pacMan, GameSetting.PacManStartX, GameSetting.PacManStartY);
      _pacMan.EatDot(GameCanvas);
      TakePlace(_pacMan.X, _pacMan.Y, Occupation.PacMan);
    }

    private void TimerPacManTick(object sender, EventArgs e)
    {
      var pacManInterval = TimeSpan.FromMilliseconds(GameSetting.PacManInterval);

      if (_pacMan.IsVanished || Time.CompareTo(pacManInterval) <= 0)
      {
        GameOver();
        return;
      }

      Time = Time.Subtract(pacManInterval);
      TakePlace(_pacMan.X, _pacMan.Y, Occupation.Empty); // Clearing place
      _pacMan.Move();

      bool isEaten = _pacMan.EatDot(_gameCanvas);

      if (isEaten)
        Score += GameSetting.MinBonus;

      TakePlace(_pacMan.X, _pacMan.Y, Occupation.PacMan); // Taking place
    }

    private void Locate(ICharacter character, int x, int y)
    {
      character.X = x;
      character.Y = y;
      Canvas.SetLeft((Image)character, x * GameSetting.PlaceSide);
      Canvas.SetTop((Image)character, y * GameSetting.PlaceSide);
      GameCanvas.Children.Add((Image)character);
    }

    // Оccupation of a place
    private void TakePlace(int x, int y, Occupation occupation)
    {
      _matrix[x, y] = occupation;
    }

    private void SendMessage(string message)
    {
      Message = message;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName]string prop = "")
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
  }
}
