using System.Collections.Generic;

namespace DataLayer.Abstractions
{
  internal interface IRepository<T> where T : class
  {
    IEnumerable<T> FindAll();
    T Find(int id);
    void Create(T item);
    void Update(T item);
    void Delete(int id);
  }
}
