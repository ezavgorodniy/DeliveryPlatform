using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface ICrudRepository<TType>
    {
        Task<TType> Create(TType entity);

        Task<IEnumerable<TType>> GetAll();

        Task<TType> Get(string id);

        Task<bool> Delete(string id);

        Task<TType> Update(TType entity);
    }
}
