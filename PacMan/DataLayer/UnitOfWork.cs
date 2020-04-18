using DataLayer.DbContexts;
using DataLayer.Repositories;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace DataLayer
{
  /// <summary>
  /// UnitOfWork pattern for game database
  /// </summary>
  public class UnitOfWork : IDisposable
  {
    private readonly GameDbContext _db = new GameDbContext();
    
    private PlayerRepository _playerRepository;
    public PlayerRepository Players => _playerRepository ?? (_playerRepository = new PlayerRepository(_db));
    
    public void Save()
    {
      _db.SaveChanges();
    }

    private bool _disposed = false;

    public virtual void Dispose(bool disposing)
    {
      if (_disposed)
        return;

      if (disposing)
      {
        _db.Dispose();
      }

      _disposed = true;
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

  }
}
