using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces.DAL
{
    public interface IDALBase<T>
    {
        Task<IEnumerable<T>> ReadAllAsync();
        Task<T> ReadOneAsync(string primaryKey);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
