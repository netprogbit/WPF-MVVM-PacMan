using DataLayer.Abstractions;
using DataLayer.DbContexts;
using DataLayer.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataLayer.Repositories
{
  /// <summary>
  /// Player repository
  /// </summary>
  public class PlayerRepository : IRepository<Player>
  {
    private readonly GameDbContext _db;

    public PlayerRepository(GameDbContext db)
    {
      _db = db;
    }

    public IEnumerable<Player> FindAll()
    {
      return _db.Players.AsNoTracking().OrderBy(p => p.Name).ToList();
    }

    public Player Find(int id)
    {
      return _db.Players.Find(id);
    }

    public void Create(Player player)
    {
      _db.Players.Add(player);
    }

    public void Update(Player player)
    {
      _db.Entry(player).State = EntityState.Modified;
    }

    public void Delete(int id)
    {
      Player player = _db.Players.Find(id);

      if (player == null)
        return;

      _db.Players.Remove(player);
    }
  }
}
