using DataLayer.Entities;
using System.Data.Entity;

namespace DataLayer.DbContexts
{
  /// <summary>
  /// Game database context
  /// </summary>
  public class GameDbContext : DbContext
  {
    public GameDbContext()
        : base("DefaultConnection")
    { }

    public DbSet<Player> Players { get; set; }
  }
}
