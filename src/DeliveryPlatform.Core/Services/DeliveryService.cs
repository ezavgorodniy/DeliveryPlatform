using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryPlatform.Core.Interfaces;
using DeliveryPlatform.Core.Models;
using DeliveryPlatform.DataLayer.Interfaces;
using Shared.Interfaces;

namespace DeliveryPlatform.Core.Services
{
    internal class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryCrudRepository _deliveryRepo;
        private readonly IDeliveryMapper _deliveryMapper;

        public DeliveryService(IDeliveryCrudRepository deliveryRepo,
            IDeliveryMapper deliveryMapper)
        {
            _deliveryRepo = deliveryRepo;
            _deliveryMapper = deliveryMapper;
        }

        public async Task<DeliveryDto> Create(IExecutionContext executionContext, DeliveryDto entity)
        {
            // TODO: check eligibility
            var created = await _deliveryRepo.Create(_deliveryMapper.To(entity));
            return _deliveryMapper.From(created);
        }

        public async Task<IEnumerable<DeliveryDto>> GetAll(IExecutionContext executionContext)
        {
            // TODO: check eligibility
            var all = await _deliveryRepo.GetAll();
            return all.Select(delivery => _deliveryMapper.From(delivery));
        }

        public async Task<DeliveryDto> Get(IExecutionContext executionContext, string id)
        {
            // TODO: check eligibility
            var entity = await _deliveryRepo.Get(id);
            return _deliveryMapper.From(entity);
        }

        public Task<bool> Delete(IExecutionContext executionContext, string id)
        {
            // TODO: check eligibility
            return _deliveryRepo.Delete(id);
        }

        public async Task<DeliveryDto> Update(IExecutionContext executionContext, DeliveryDto entity)
        {
            // TODO: check eligibility
            var dbEntity = await _deliveryRepo.Update(_deliveryMapper.To(entity));
            return _deliveryMapper.From(dbEntity);
        }
    }
}
