using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces.Service
{
    public interface IServiceBase<T>
    {
        Task<IEnumerable<T>> ReadAllAsync();
        Task<T> ReadOneAsync(string primaryKey);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
    }
}
