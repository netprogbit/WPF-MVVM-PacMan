using PacMan.Helpers;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PacMan.ViewModels
{
  public class PlayerViewModel : INotifyPropertyChanged, IDataErrorInfo
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

    private bool _isEnable = false;
    public bool IsEnable
    {
      get { return _isEnable; }
      set
      {
        _isEnable = value;
        OnPropertyChanged();
      }
    }

    public string Error
    {
      get => null;
    }

    public string this[string columnName]
    {
      get
      {
        string result = null;

        if (columnName == nameof(Name))
        {
          if (string.IsNullOrEmpty(Name))
            result = StringHelper.NameRequired;
        }

        if (string.IsNullOrEmpty(result))
          IsEnable = true;
        else
          IsEnable = false;

        return result;
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
