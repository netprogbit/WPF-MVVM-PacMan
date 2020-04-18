using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Abstractions
{
  internal interface IRepository<T> where T : class
  {
    IQueryable<T> GetAll();
    T Get(int id);
    void Create(T item);
    void Update(T item);
    void Delete(int id);
  }
}
