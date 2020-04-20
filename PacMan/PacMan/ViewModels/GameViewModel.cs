using System.Collections.Generic;
using Core.Abstractions;
using PacMan.Commands;
using PacMan.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using DataLayer.Entities;
using System;
using System.Linq;

namespace PacMan.ViewModels
{
  /// <summary>
  /// Layer beetwin game model and views
  /// </summary>
  public class GameViewModel : INotifyPropertyChanged
  {
    private readonly GameModel _gameModel = new GameModel();

    public string Message { get { return _gameModel.Message; } }

    private bool _isOpenPopup = false;
    public bool IsOpenPopup { get => _isOpenPopup; set { _isOpenPopup = value; OnPropertyChanged(); } }

    public ObservableCollection<IMovableCost> Plugins
    {
      get => new ObservableCollection<IMovableCost>(_gameModel.Plugins);
      set => _gameModel.Plugins = new List<IMovableCost>(value);
    }

    public IMovableCost CurrentPlugin
    {
      get => _gameModel.CurrentPlugin;
      set => _gameModel.CurrentPlugin = value;
    }

    public ObservableCollection<PlayerViewModel> TopScorers
    {
      get => new ObservableCollection<PlayerViewModel>(_gameModel.TopScorers.Select(p => new PlayerViewModel { Id = p.Id, Name = p.Name, Score = p.Score }));
      set => _gameModel.TopScorers = new List<Player>(value.Select(p => new Player { Id = p.Id, Name = p.Name, Score = p.Score }));
    }

    private ObservableCollection<PlayerViewModel> _players;
    public ObservableCollection<PlayerViewModel> Players
    {
      get { _players = new ObservableCollection<PlayerViewModel>(_gameModel.Players.Select(p => new PlayerViewModel { Id = p.Id, Name = p.Name, Score = p.Score })); return _players; }
      set => _gameModel.Players = new List<Player>(value.Select(p => new Player { Id = p.Id, Name = p.Name, Score = p.Score }));
    }    

    public PlayerViewModel CurrentPlayer
    {
      get => _players.SingleOrDefault(p => p.Id == _gameModel.CurrentPlayer?.Id);
      set => _gameModel.CurrentPlayer = _gameModel.Players.SingleOrDefault(p => p.Id == value?.Id);
    }

    private PlayerViewModel _newPlayer = new PlayerViewModel();
    public PlayerViewModel NewPlayer { get => _newPlayer; set { _newPlayer = value; OnPropertyChanged(); } }

    public TimeSpan Time
    {
      get => _gameModel.Time;
      set => _gameModel.Time = value;
    }

    public int Score
    {
      get => _gameModel.Score;
      set => _gameModel.Score = value;
    }

    public string PlayPauseContent
    {
      get => _gameModel.PlayPauseContent;
      set => _gameModel.PlayPauseContent = value;
    }

    public bool IsGameOver { get => _gameModel.IsGameOver; set => _gameModel.IsGameOver = value; }

    public Canvas GameCanvas => _gameModel.GameCanvas;

    public ICommand PlayPauseCommand { get; set; }
    public ICommand LeftKeyCommand { get; set; }
    public ICommand RightKeyCommand { get; set; }
    public ICommand UpKeyCommand { get; set; }
    public ICommand DownKeyCommand { get; set; }
    public ICommand NewGameCommand { get; set; }
    public ICommand GameOverCommand { get; set; }
    public ICommand AddPlayerCommand { get; set; }
    public ICommand DeletePlayerCommand { get; set; }
    public ICommand PopupCancelCommand { get; set; }

    public GameViewModel()
    {
      _gameModel.PropertyChanged += (s, e) =>
      {
        if (e.PropertyName.Equals(nameof(Message)))
          IsOpenPopup = true;

        OnPropertyChanged(e.PropertyName);
      };

      InitializeCommands();
    }

    private void PressLeftKey()
    {
      _gameModel.PressLeftKey();
    }

    private void PressRightKey()
    {
      _gameModel.PressRightKey();
    }

    private void PressUpKey()
    {
      _gameModel.PressUpKey();
    }

    private void PressDownKey()
    {
      _gameModel.PressDownKey();
    }

    private void NewGame()
    {
      _gameModel.New();
    }

    private void PlayPause()
    {
      _gameModel.PlayPause();
    }

    private void GameOver()
    {
      _gameModel.GameOver();
    }

    private void AddPlayer()
    {
      _gameModel.AddPlayer(_newPlayer.Name);
      NewPlayer = new PlayerViewModel();
    }

    private void DeletePlayer()
    {
      _gameModel.DeletePlayer();
    }

    private void PopupCancel()
    {
      IsOpenPopup = false;
    }

    private void InitializeCommands()
    {
      PlayPauseCommand = new RelayCommand(arg => PlayPause());
      LeftKeyCommand = new RelayCommand(arg => PressLeftKey());
      RightKeyCommand = new RelayCommand(arg => PressRightKey());
      UpKeyCommand = new RelayCommand(arg => PressUpKey());
      DownKeyCommand = new RelayCommand(arg => PressDownKey());
      NewGameCommand = new RelayCommand(arg => NewGame());
      GameOverCommand = new RelayCommand(arg => GameOver());
      AddPlayerCommand = new RelayCommand(arg => AddPlayer());
      DeletePlayerCommand = new RelayCommand(arg => DeletePlayer());
      PopupCancelCommand = new RelayCommand(arg => PopupCancel());
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName]string prop = "")
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
  }
}
