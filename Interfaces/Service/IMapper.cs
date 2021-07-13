using Entities.Entities;
using Entities.Models;
using System.Threading.Tasks;

namespace Interfaces.Service
{
    public interface IMapper<T,U>
    {
        //T is an object
        T MapObject(U _entity);
        //U is an entity
        U MapEntity(T _object);
    }
}
