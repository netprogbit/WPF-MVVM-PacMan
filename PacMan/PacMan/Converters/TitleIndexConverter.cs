using PacMan.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace PacMan.Converters
{
  public class TitleIndexConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      if (values == null || values.Length != 2)
        return null;

      int? index = values[0] as int?;
      var players = values[1] as ObservableCollection<PlayerViewModel>;

      if (!index.HasValue || index < 0 || players == null || players.Count <= 0)
        return null;

      if (index >= players.Count)
        index = 0;

      return string.Format("Pac-Man ({0})", players[index.Value].Name);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
