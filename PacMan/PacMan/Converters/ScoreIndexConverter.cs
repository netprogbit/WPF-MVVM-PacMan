using PacMan.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace PacMan.Converters
{
  public class ScoreIndexConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      if (values == null || values.Length != 2)
        return null;

      int? index = values[0] as int?;
      var players = values[1] as ObservableCollection<PlayerViewModel>;

      if (!index.HasValue || index < 0 || players == null || players.Count <= 0)
        return null;

      if (index >= players.Count)
        index = 0;

      return players[index.Value].Score;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
