using DataLayer.DbContexts;
using DataLayer.Entities;
using DataLayer.Repositories;
using System;

namespace DataLayer
{
  /// <summary>
  /// UnitOfWork pattern for game database
  /// </summary>
  public class UnitOfWork : IDisposable
  {
    private readonly GameDbContext _db = new GameDbContext();
    
    private IRepository<Player> _playerRepository;
    public IRepository<Player> Players => _playerRepository ?? (_playerRepository = new PlayerRepository(_db));
    
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
