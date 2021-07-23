using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces.DAL
{
    public interface ITableDAL<T>
    {
        Task<IEnumerable<T>> ReadAllAsync();
        Task<IEnumerable<T>> ReadById(string primaryKey);
        Task<T> ReadOneAsync(string primaryKey);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
