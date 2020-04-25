using PacMan.Helpers;
using PacMan.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace PacMan.Converters
{
  public class TitleIndexConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      if (values == null || values.Length != 2)
        return StringHelper.Title;

      int? index = values[0] as int?;
      var players = values[1] as ObservableCollection<PlayerViewModel>;

      if (!index.HasValue || index < 0 || players == null || players.Count <= 0)
        return StringHelper.Title;

      if (index >= players.Count)
        index = 0;

      return string.Format("{0} ({1})", StringHelper.Title, players[index.Value].Name);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
