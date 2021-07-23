using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Entities;

namespace Interfaces.DAL
{
    public interface IUserDAL
    {
        Task<UserEntity> ReadAsync(string userName);
        Task CreateAsync(UserEntity entity);
        Task UpdateAsync(UserEntity entity);
        Task DeleteAsync(UserEntity entity);
    }
}
