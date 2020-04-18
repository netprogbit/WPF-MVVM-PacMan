using Core.Abstractions;
using Core.Enums;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Services.Plugin
{
  /// <summary>
  /// Plagins importer
  /// </summary>
  public class Importer
  {
    [ImportMany(typeof(IMovableCost))]
    public IEnumerable<IMovableCost> ImportedMembers { get; set; }

    public List<IMovableCost> GetPlugins(string path, Occupation[,] matrix)
    {
      var catalog = new DirectoryCatalog(path);
      var compositionContainer = new CompositionContainer(catalog);
      compositionContainer.ComposeExportedValue("matrix", matrix);
      compositionContainer.ComposeParts(this);
      return new List<IMovableCost>(ImportedMembers);
    }
  }
}
