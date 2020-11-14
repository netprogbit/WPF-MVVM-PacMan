using System.Collections.Generic;

namespace DataLayer.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> FindAll();
        T Find(int id);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
    }
}
