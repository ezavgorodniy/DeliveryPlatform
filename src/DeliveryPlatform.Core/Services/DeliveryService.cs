using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryPlatform.Core.Exceptions;
using DeliveryPlatform.Core.Interfaces;
using DeliveryPlatform.Core.Models;
using DeliveryPlatform.DataLayer.Interfaces;
using Identity.Contract;
using Shared.Interfaces;

namespace DeliveryPlatform.Core.Services
{
    internal class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _deliveryRepo;
        private readonly IDeliveryMapper _deliveryMapper;
        private readonly IPermissionChecker _permissionChecker;

        public DeliveryService(IDeliveryRepository deliveryRepo,
            IDeliveryMapper deliveryMapper,
            IPermissionChecker permissionChecker)
        {
            _deliveryRepo = deliveryRepo;
            _deliveryMapper = deliveryMapper;
            _permissionChecker = permissionChecker;
        }

        public async Task<DeliveryDto> Create(IExecutionContext executionContext, DeliveryDto entity)
        {
            AssertExecutionContext(executionContext);

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (executionContext.UserRole != Role.User)
            {
                throw new UnauthorizedException("Only user can create delivery");
            }

            var created = await _deliveryRepo.Create(_deliveryMapper.To(entity));
            return _deliveryMapper.From(created);
        }

        public async Task<IEnumerable<DeliveryDto>> GetAll(IExecutionContext executionContext)
        {
            AssertExecutionContext(executionContext);

            var all = await _deliveryRepo.GetAll();
            return all.Select(delivery => _deliveryMapper.From(delivery));
        }

        public async Task<DeliveryDto> Get(IExecutionContext executionContext, string id)
        {
            AssertExecutionContext(executionContext);

            var entity = await _deliveryRepo.Get(id);
            return _deliveryMapper.From(entity);
        }


        public Task<bool> Delete(IExecutionContext executionContext, string id)
        {
            AssertExecutionContext(executionContext);
            if (executionContext.UserRole != Role.User)
            {
                throw new UnauthorizedException("Only user can delete delivery");
            }

            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return _deliveryRepo.Delete(id);
        }

        public async Task<DeliveryDto> Update(IExecutionContext executionContext, DeliveryDto entity)
        {
            AssertExecutionContext(executionContext);

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var existedEntity = await _deliveryRepo.Get(entity.Id);
            if (existedEntity == null)
            {
                return null;
            }

            var oldState = existedEntity.State;
            var newState = entity.State;
            if (!_permissionChecker.RoleHasChangePermission(executionContext.UserRole, 
                oldState, newState))
            {
                throw new UnauthorizedException(
                    $"This role has no permission to change the role from {oldState} to {newState}");

            }
            
            var dbEntity = await _deliveryRepo.Update(_deliveryMapper.To(entity));
            return _deliveryMapper.From(dbEntity);
        }

        private void AssertExecutionContext(IExecutionContext executionContext)
        {
            if (executionContext == null)
            {
                throw new ArgumentNullException(nameof(executionContext));
            }

            if (!executionContext.IsInitialized)
            {
                throw new UnauthorizedException("ExecutionContext is not initialized");
            }
        }
    }
}
