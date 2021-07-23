using Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Helpers;

namespace Interfaces.DAL
{
    public interface IProjectFileDAL
    {
        Task<IEnumerable<ProjectFileEntity>> ReadAllAsync(string primaryKey);
        Task<ProjectFileEntity> ReadOneAsync(string primaryKey, string rowKey);
        Task<TaskResult> CreateAsync(ProjectFileEntity entity);
        Task UpdateAsync(ProjectFileEntity entity);
        Task DeleteAsync(ProjectFileEntity entity);
        Task<bool> Exists(ProjectFileEntity entity);
    }
}
