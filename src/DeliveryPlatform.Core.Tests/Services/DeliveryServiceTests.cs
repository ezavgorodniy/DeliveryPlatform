using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryPlatform.Core.Exceptions;
using DeliveryPlatform.Core.Interfaces;
using DeliveryPlatform.Core.Models;
using DeliveryPlatform.Core.Services;
using DeliveryPlatform.DataLayer.DataModels;
using DeliveryPlatform.DataLayer.Interfaces;
using Identity.Contract;
using Moq;
using Shared.Interfaces;
using Xunit;

namespace DeliveryPlatform.Core.Tests.Services
{
    public class DeliveryServiceTests
    {
        private readonly DeliveryService _deliveryService;

        private readonly Mock<IDeliveryCrudRepository> _mockRepo;

        private readonly Mock<IExecutionContext> _mockExecutionContext;
        private readonly Mock<IDeliveryMapper> _mockMapper;

        private readonly Mock<IPermissionChecker> _mockPermissionChecker;
        
        public DeliveryServiceTests()
        {
            _mockPermissionChecker = new Mock<IPermissionChecker>();
            _mockMapper = new Mock<IDeliveryMapper>();
            _mockRepo = new Mock<IDeliveryCrudRepository>();

            _mockExecutionContext = new Mock<IExecutionContext>();
            _mockExecutionContext.SetupGet(context => context.IsInitialized).Returns(true);

            _deliveryService = new DeliveryService(_mockRepo.Object,
                _mockMapper.Object, _mockPermissionChecker.Object);
        }

        [Fact]
        public async Task CreateNullExecutionContextExpectArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _deliveryService.Create(null, new DeliveryDto()));
        }

        [Fact]
        public async Task CreateNullDtoExpectArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _deliveryService.Create(_mockExecutionContext.Object, null));
        }

        [Fact]
        public async Task CreateNonInitializedeExecutionContextExpectUnauthorizedException()
        {
            _mockExecutionContext.SetupGet(executionContext => executionContext.IsInitialized).Returns(false);
            await Assert.ThrowsAsync<UnauthorizedException>(async () =>
                await _deliveryService.Create(_mockExecutionContext.Object, new DeliveryDto()));
        }

        [Fact]
        public async Task CreateNotUserExpectUnauthorizedException()
        {
            _mockExecutionContext.SetupGet(executionContext => executionContext.UserRole).Returns(Role.Partner);
            await Assert.ThrowsAsync<UnauthorizedException>(async () =>
                await _deliveryService.Create(_mockExecutionContext.Object, new DeliveryDto()));
        }

        [Fact]
        public async Task CreateExpectCallToRepo()
        {
            _mockExecutionContext.SetupGet(executionContext => executionContext.UserRole).Returns(Role.User);

            var expectedDto = new DeliveryDto();
            var expectedEntity = new Delivery();
            var expectedReturnedEntity = new Delivery();
            var expectedReturnedDto = new DeliveryDto();

            _mockMapper.Setup(mapper => mapper.To(expectedDto)).Returns(expectedEntity);

            _mockRepo.Setup(repo => repo.Create(expectedEntity)).Returns(Task.FromResult(expectedReturnedEntity));

            _mockMapper.Setup(mapper => mapper.From(expectedReturnedEntity)).Returns(expectedReturnedDto);

            var result = await _deliveryService.Create(_mockExecutionContext.Object, expectedDto);

            Assert.Equal(expectedReturnedDto, result);
        }

        [Fact]
        public async Task GetAllExecutionContextExpectArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _deliveryService.GetAll(null));
        }

        [Fact]
        public async Task GetAllNonInitializedeExecutionContextExpectUnauthorizedException()
        {
            _mockExecutionContext.SetupGet(executionContext => executionContext.IsInitialized).Returns(false);
            await Assert.ThrowsAsync<UnauthorizedException>(async () =>
                await _deliveryService.GetAll(_mockExecutionContext.Object));
        }

        [Fact]
        public async Task GetAllExpectCallToRepo()
        {
            var expectedReturnedEntity = new Delivery();
            var expectedReturnedDto = new DeliveryDto();

            _mockRepo.Setup(repo => repo.GetAll())
                .Returns(Task.FromResult((IEnumerable<Delivery>) new List<Delivery> {expectedReturnedEntity}));

            _mockMapper.Setup(mapper => mapper.From(expectedReturnedEntity)).Returns(expectedReturnedDto);

            var result = await _deliveryService.GetAll(_mockExecutionContext.Object);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedReturnedDto, result.First());
        }

        [Fact]
        public async Task GetExecutionContextExpectArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _deliveryService.Get(null, "id"));
        }

        [Fact]
        public async Task GetNonInitializedeExecutionContextExpectUnauthorizedException()
        {
            _mockExecutionContext.SetupGet(executionContext => executionContext.IsInitialized).Returns(false);
            await Assert.ThrowsAsync<UnauthorizedException>(async () =>
                await _deliveryService.Get(_mockExecutionContext.Object, "id"));
        }

        [Fact]
        public async Task GetExpectCallToRepo()
        {
            const string expectedId = "expectedId";

            var expectedReturnedEntity = new Delivery();
            var expectedReturnedDto = new DeliveryDto();

            _mockRepo.Setup(repo => repo.Get(expectedId))
                .Returns(Task.FromResult(expectedReturnedEntity));

            _mockMapper.Setup(mapper => mapper.From(expectedReturnedEntity)).Returns(expectedReturnedDto);

            var result = await _deliveryService.Get(_mockExecutionContext.Object, expectedId);

            Assert.Equal(expectedReturnedDto, result);
        }

        [Fact]
        public async Task DeleteNullExecutionContextExpectArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _deliveryService.Delete(null, "id"));
        }

        [Fact]
        public async Task DeleteNullDtoExpectArgumentNullException()
        {
            _mockExecutionContext.SetupGet(executionContext => executionContext.UserRole).Returns(Role.User);
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _deliveryService.Delete(_mockExecutionContext.Object, null));
        }

        [Fact]
        public async Task DeleteNonInitializedeExecutionContextExpectUnauthorizedException()
        {
            _mockExecutionContext.SetupGet(executionContext => executionContext.IsInitialized).Returns(false);
            await Assert.ThrowsAsync<UnauthorizedException>(async () =>
                await _deliveryService.Delete(_mockExecutionContext.Object, "id"));
        }

        [Fact]
        public async Task DeleteNotUserExpectUnauthorizedException()
        {
            _mockExecutionContext.SetupGet(executionContext => executionContext.UserRole).Returns(Role.Partner);
            await Assert.ThrowsAsync<UnauthorizedException>(async () =>
                await _deliveryService.Delete(_mockExecutionContext.Object, "id"));
        }

        [Fact]
        public async Task DeleteExpectCallToRepo()
        {
            const string expectedId = "expectedId";
            _mockExecutionContext.SetupGet(executionContext => executionContext.UserRole).Returns(Role.User);

            const bool expectedResult = true;
            _mockRepo.Setup(repo => repo.Delete(expectedId)).Returns(Task.FromResult(expectedResult));

            var result = await _deliveryService.Delete(_mockExecutionContext.Object, expectedId);

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task UpdateNullExecutionContextExpectArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _deliveryService.Update(null, new DeliveryDto()));
        }

        [Fact]
        public async Task UpdateNullDtoExpectArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _deliveryService.Update(_mockExecutionContext.Object, null));
        }

        [Fact]
        public async Task UpdateNonInitializedeExecutionContextExpectUnauthorizedException()
        {
            _mockExecutionContext.SetupGet(executionContext => executionContext.IsInitialized).Returns(false);
            await Assert.ThrowsAsync<UnauthorizedException>(async () =>
                await _deliveryService.Update(_mockExecutionContext.Object, new DeliveryDto()));
        }

        [Fact]
        public async Task UpdateNoDeliveryFoundExpectNull()
        {
            _mockExecutionContext.SetupGet(executionContext => executionContext.UserRole).Returns(Role.User);

            const string expectedId = "expectedId";
            var expectedDto = new DeliveryDto { Id = expectedId };

            _mockRepo.Setup(repo => repo.Get(expectedId)).Returns(Task.FromResult(null as Delivery));

            var result = await _deliveryService.Update(_mockExecutionContext.Object, expectedDto);

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateChangeForStateIsNotAllowed()
        {
            const Role expectedRole = Role.Partner;
            _mockExecutionContext.SetupGet(executionContext => executionContext.UserRole).Returns(expectedRole);

            const string expectedId = "expectedId";
            const DeliveryState newState = DeliveryState.Approved;
            var expectedDto = new DeliveryDto { Id = expectedId, State = newState };

            const DeliveryState oldState = DeliveryState.Created;
            var existedDelivery = new Delivery
            {
                Id = expectedId,
                State = oldState
            };
            _mockRepo.Setup(repo => repo.Get(expectedId)).Returns(Task.FromResult(existedDelivery));

            _mockPermissionChecker.Setup(checker => checker.RoleHasChangePermission(expectedRole,
                oldState, newState)).Returns(false);

            await Assert.ThrowsAsync<UnauthorizedException>(async () =>
                await _deliveryService.Update(_mockExecutionContext.Object, expectedDto));
        }

        [Fact]
        public async Task UpdateExpectCallToRepo()
        {
            const Role expectedRole = Role.User;
            _mockExecutionContext.SetupGet(executionContext => executionContext.UserRole).Returns(expectedRole);

            const string expectedId = "expectedId";
            const DeliveryState newState = DeliveryState.Approved;
            var expectedDto = new DeliveryDto { Id = expectedId, State = newState };

            const DeliveryState oldState = DeliveryState.Created;
            var existedDelivery = new Delivery
            {
                Id = expectedId,
                State = oldState
            };
            _mockRepo.Setup(repo => repo.Get(expectedId)).Returns(Task.FromResult(existedDelivery));

            _mockPermissionChecker.Setup(checker => checker.RoleHasChangePermission(expectedRole,
                oldState, newState)).Returns(true);

            var expectedEntity = new Delivery();
            var expectedReturnedEntity = new Delivery();
            var expectedReturnedDto = new DeliveryDto();

            _mockMapper.Setup(mapper => mapper.To(expectedDto)).Returns(expectedEntity);

            _mockRepo.Setup(repo => repo.Update(expectedEntity)).Returns(Task.FromResult(expectedReturnedEntity));

            _mockMapper.Setup(mapper => mapper.From(expectedReturnedEntity)).Returns(expectedReturnedDto);

            var result = await _deliveryService.Update(_mockExecutionContext.Object, expectedDto);

            Assert.Equal(expectedReturnedDto, result);
        }
    }
}
