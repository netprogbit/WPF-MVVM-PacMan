using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DataLayer.Entities
{
  public class Player : INotifyPropertyChanged
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
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(prop));
    }

  }
}
