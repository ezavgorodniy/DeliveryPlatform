using System.Collections.Generic;
using System.Threading.Tasks;
using DeliveryPlatform.Core.Models;
using Shared.Interfaces;

namespace DeliveryPlatform.Core.Interfaces
{
    public interface IDeliveryService
    {
        Task<DeliveryDto> Create(IExecutionContext executionContext, DeliveryDto entity);

        Task<IEnumerable<DeliveryDto>> GetAll(IExecutionContext executionContext);

        Task<DeliveryDto> Get(IExecutionContext executionContext, string id);

        Task<bool> Delete(IExecutionContext executionContext, string id);

        Task<DeliveryDto> Update(IExecutionContext executionContext, DeliveryDto entity);
    }
}
