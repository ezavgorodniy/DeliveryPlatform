using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeliveryPlatform.DataLayer.DataModels;
using DeliveryPlatform.DataLayer.Interfaces;

namespace DeliveryPlatform.DataLayer.Repositories
{
    internal class DeliveryCrudRepository : IDeliveryCrudRepository
    {
        public Task<Delivery> Create(Delivery entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Delivery>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Delivery> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Delivery> Update(Delivery entity)
        {
            throw new NotImplementedException();
        }
    }
}
