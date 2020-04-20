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
    
    private void TextBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
    {
      ((Control)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource(); // For validation
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName]string prop = "")
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(prop));
    }    
  }
}
