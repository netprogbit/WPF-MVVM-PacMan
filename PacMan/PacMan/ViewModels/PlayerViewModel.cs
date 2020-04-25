using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PacMan.ViewModels
{
  public class PlayerViewModel : INotifyPropertyChanged
  {
    public int Id { get; set; }

    private string _name;
    public string Name
    {
      get { return _name; }
      set
      {
        _name = value;
        OnPropertyChanged();
      }
    }

    private int _score;
    public int Score
    {
      get { return _score; }
      set
      {
        _score = value;
        OnPropertyChanged();
      }
    }        
    
    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName]string prop = "")
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
  }
}
