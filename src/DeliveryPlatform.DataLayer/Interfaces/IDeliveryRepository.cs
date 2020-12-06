using System.Threading.Tasks;
using DeliveryPlatform.DataLayer.DataModels;
using Shared.Interfaces;

namespace DeliveryPlatform.DataLayer.Interfaces
{
    public interface IDeliveryRepository : ICrudRepository<Delivery>
    {
        Task MarkExpiredDeliveries();
    }
}
