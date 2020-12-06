using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryPlatform.DataLayer.DataModels;
using DeliveryPlatform.DataLayer.Interfaces;

namespace DeliveryPlatform.DataLayer.Repositories
{
    internal class DeliveryCrudRepository : IDeliveryCrudRepository
    {
        private readonly List<Delivery> _deliveries = new List<Delivery>();

        public Task<Delivery> Create(Delivery entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.Id = Guid.NewGuid().ToString();
            _deliveries.Add(entity);
            return Task.FromResult(entity);
        }

        public Task<IEnumerable<Delivery>> GetAll()
        {
            return Task.FromResult(_deliveries as IEnumerable<Delivery>);
        }

        public Task<Delivery> Get(string id)
        {
            var foundDelivery = _deliveries.FirstOrDefault(delivery => delivery.Id == id);
            return Task.FromResult(foundDelivery);
        }

        public Task<bool> Delete(string id)
        {
            var foundDelivery = _deliveries.FirstOrDefault(delivery => delivery.Id == id);
            if (foundDelivery != null)
            {
                _deliveries.Remove(foundDelivery);
            }
            return Task.FromResult(foundDelivery != null);
        }

        public Task<Delivery> Update(Delivery entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var foundDelivery = _deliveries.FirstOrDefault(delivery => delivery.Id == entity.Id);
            if (foundDelivery != null)
            {
                // at this moment only state can be changed
                foundDelivery.State = entity.State;
            }
            return Task.FromResult(foundDelivery);
        }
    }
}
