using PacMan.ViewModels;
using PacMan.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PacMan
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var gameView = new GameView();
            gameView.DataContext = new GameViewModel();
            gameView.Show();
        }
    }
}
