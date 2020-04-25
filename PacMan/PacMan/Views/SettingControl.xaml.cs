using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace PacMan.Views
{
  /// <summary>
  /// Interaction logic for SettingControl view
  /// </summary>
  public partial class SettingControl : UserControl, INotifyPropertyChanged
  {
    public SettingControl()
    {
      InitializeComponent();
    }

    private bool _hasControl = false;
    public bool HasControl
    {
      get { return _hasControl; }
      set
      {
        _hasControl = value;
        OnPropertyChanged();
      }
    }
    
    private int _errors = 0;

    private void Validation_Error(object sender, ValidationErrorEventArgs e)
    {
      if (e.Action == ValidationErrorEventAction.Added)
        _errors++;
      else
        _errors--;

      HasControl = _errors == 0;      
    }

    private void TextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
    {
      ((Control)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource(); // For validation after get texbox focus
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName]string prop = "")
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }    
  }
}
