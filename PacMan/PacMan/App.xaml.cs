using NLog;
using PacMan.Helpers;
using PacMan.ViewModels;
using PacMan.Views;
using System.Windows;
using System.Windows.Threading;

namespace PacMan
{
  public partial class App : Application
  {
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public App()
    {
      var gameView = new GameView();
      gameView.DataContext = new GameViewModel();
      gameView.Show();
    }

    /// <summary>
    /// Global exception handling
    /// </summary>
    private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
      _logger.Error("{0}: {1} {2}", StringHelper.Error, e.Exception.Message, e.Exception.StackTrace);
      MessageBox.Show(StringHelper.InternalAppError, StringHelper.Error, MessageBoxButton.OK, MessageBoxImage.Error);
      e.Handled = true;
    }

  }
}
