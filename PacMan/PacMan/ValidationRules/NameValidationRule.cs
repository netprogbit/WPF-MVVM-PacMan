using PacMan.Helpers;
using PacMan.ViewModels;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace PacMan.ValidationRules
{
  public class NameValidationRule : ValidationRule
  {
    public CollectionViewSource CurrentCollection { get; set; } // Collection for name unique check

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
      string name = value as string;

      // Verify name presence
      if (string.IsNullOrEmpty(name))
        return new ValidationResult(false, StringHelper.NameRequired);

      if (CurrentCollection != null)
      {
        var players = CurrentCollection.Source as ObservableCollection<PlayerViewModel>;

        if (players != null)
        {
          // Verify name uniqueness
          if (players.Any(p => p.Name == name))
            return new ValidationResult(false, StringHelper.NameUnique);
        }
      }
           
      return new ValidationResult(true, null);
    }
  }
}
