using System;
using System.Threading.Tasks;
using DeliveryPlatform.Core.Interfaces;
using DeliveryPlatform.DataLayer.Interfaces;

namespace DeliveryPlatform.Core.Services
{
    public class ExpirationService : IExpirationService
    {
        private readonly IDeliveryCrudRepository _repo;

        public ExpirationService(IDeliveryCrudRepository repo)
        {
            _repo = repo;
        }

        public Task UpdateExpirations()
        {
            return _repo.MarkExpiredDeliveries();
        }
    }
}
