using Entities.Entities;
using System.Threading.Tasks;

namespace Interfaces.DAL
{
    public interface IProjectDAL : ITableDAL<ProjectEntity>
    {
        Task<int> GetHighest();
    }
}
