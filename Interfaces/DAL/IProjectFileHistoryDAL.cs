using Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces.DAL
{
    public interface IProjectFileHistoryDAL
    {
        Task<IEnumerable<ProjectFileHistoryEntity>> ReadAllAsync(string primaryKey);
        Task<ProjectFileHistoryEntity> ReadOneAsync(string primaryKey, string rowKey);
        Task CreateAsync(ProjectFileHistoryEntity entity);
        Task UpdateAsync(ProjectFileHistoryEntity entity);
        Task DeleteAsync(ProjectFileHistoryEntity entity);
    }
}
