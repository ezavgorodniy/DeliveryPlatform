using System;
using System.Threading.Tasks;
using DeliveryPlatform.Core.Interfaces;
using DeliveryPlatform.DataLayer.Interfaces;

namespace DeliveryPlatform.Core.Services
{
    public class ExpirationService : IExpirationService
    {
        private readonly IDeliveryRepository _repo;

        public ExpirationService(IDeliveryRepository repo)
        {
            _repo = repo;
        }

        public Task UpdateExpirations()
        {
            return _repo.MarkExpiredDeliveries();
        }
    }
}
