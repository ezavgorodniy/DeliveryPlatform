using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeliveryPlatform.Controllers;
using DeliveryPlatform.Core.Exceptions;
using DeliveryPlatform.Core.Interfaces;
using DeliveryPlatform.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Interfaces;
using Xunit;

namespace DeliveryPlatform.Api.Tests.Controllers
{
    public class DeliveriesControllerTests
    {
        private readonly DeliveriesController _deliveriesController;
        
        private readonly Mock<IDeliveryService> _mockDeliveryService;
        private readonly Mock<IExecutionContext> _mockExecutionContext;

        public DeliveriesControllerTests()
        {
            _mockDeliveryService = new Mock<IDeliveryService>();
            _mockExecutionContext = new Mock<IExecutionContext>();
            var mockLogger = new Mock<ILogger<DeliveriesController>>();

            _deliveriesController = new DeliveriesController(_mockDeliveryService.Object, _mockExecutionContext.Object,
                mockLogger.Object);
        }

        [Fact]
        public async Task GetUnauthorizedExceptionHappenedExpect401()
        {
            _mockDeliveryService.Setup(service => service.GetAll(_mockExecutionContext.Object))
                .Throws<UnauthorizedException>();

            var result = await _deliveriesController.Get();

            Assert.IsType<StatusCodeResult>(result);

            var statusCodeResult = (StatusCodeResult)result;
            Assert.Equal(401, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetExceptionHappenedExpectBadRequestResult()
        {
            _mockDeliveryService.Setup(service => service.GetAll(_mockExecutionContext.Object))
                .Throws<Exception>();

            var result = await _deliveriesController.Get();

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task GetExpetReturnObjectFromAll()
        {
            var deliveries = new List<DeliveryDto>();

            _mockDeliveryService.Setup(service => service.GetAll(_mockExecutionContext.Object))
                .Returns(Task.FromResult((IEnumerable<DeliveryDto>) deliveries));

            var result = await _deliveriesController.Get();

            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = (OkObjectResult)result;
            Assert.Equal(deliveries, okObjectResult.Value);
        }

        [Fact]
        public async Task GetByIdUnauthorizedExceptionHappenedExpect401()
        {
            const string expectedId = "expectedId";
            _mockDeliveryService.Setup(service => service.Get(_mockExecutionContext.Object, expectedId))
                .Throws<UnauthorizedException>();

            var result = await _deliveriesController.Get(expectedId);

            Assert.IsType<StatusCodeResult>(result);

            var statusCodeResult = (StatusCodeResult)result;
            Assert.Equal(401, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetByIdExceptionHappenedExpectBadRequestResult()
        {
            const string expectedId = "expectedId";
            _mockDeliveryService.Setup(service => service.Get(_mockExecutionContext.Object, expectedId))
                .Throws<Exception>();

            var result = await _deliveriesController.Get(expectedId);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task GetByIdExpectReturnObjectFromGet()
        {
            const string expectedId = "expectedId";
            var delivery = new DeliveryDto();

            _mockDeliveryService.Setup(service => service.Get(_mockExecutionContext.Object, expectedId))
                .Returns(Task.FromResult(delivery));

            var result = await _deliveriesController.Get(expectedId);

            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = (OkObjectResult)result;
            Assert.Equal(delivery, okObjectResult.Value);
        }

        [Fact]
        public async Task DeleteByIdUnauthorizedExceptionHappenedExpect401()
        {
            const string expectedId = "expectedId";
            _mockDeliveryService.Setup(service => service.Delete(_mockExecutionContext.Object, expectedId))
                .Throws<UnauthorizedException>();

            var result = await _deliveriesController.Delete(expectedId);

            Assert.IsType<StatusCodeResult>(result);

            var statusCodeResult = (StatusCodeResult)result;
            Assert.Equal(401, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task DeleteByIdExceptionHappenedExpectBadRequestResult()
        {
            const string expectedId = "expectedId";
            _mockDeliveryService.Setup(service => service.Delete(_mockExecutionContext.Object, expectedId))
                .Throws<Exception>();

            var result = await _deliveriesController.Delete(expectedId);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteByIdNoDeletedExpectBadRequest()
        {
            const string expectedId = "expectedId";

            _mockDeliveryService.Setup(service => service.Delete(_mockExecutionContext.Object, expectedId))
                .Returns(Task.FromResult(false));

            var result = await _deliveriesController.Delete(expectedId);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteByIdDeletedExpectNoContentResult()
        {
            const string expectedId = "expectedId";

            _mockDeliveryService.Setup(service => service.Delete(_mockExecutionContext.Object, expectedId))
                .Returns(Task.FromResult(true));

            var result = await _deliveriesController.Delete(expectedId);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task CreateUnauthorizedExceptionHappenedExpect401()
        {
            var deliveryDto = new DeliveryDto();
            _mockDeliveryService.Setup(service => service.Create(_mockExecutionContext.Object, deliveryDto))
                .Throws<UnauthorizedException>();

            var result = await _deliveriesController.Create(deliveryDto);

            Assert.IsType<StatusCodeResult>(result);

            var statusCodeResult = (StatusCodeResult)result;
            Assert.Equal(401, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task CreateExceptionHappenedExpectBadRequestResult()
        {
            var deliveryDto = new DeliveryDto();
            _mockDeliveryService.Setup(service => service.Create(_mockExecutionContext.Object, deliveryDto))
                .Throws<Exception>();

            var result = await _deliveriesController.Create(deliveryDto);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task CreateNullDtoExpectBadRequestResult()
        {
            var result = await _deliveriesController.Create(null);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task CreateExpectReturnObjectFromCreate()
        {
            var deliveryDto = new DeliveryDto();
            var deliveryDtoResponse = new DeliveryDto();

            _mockDeliveryService.Setup(service => service.Create(_mockExecutionContext.Object, deliveryDto))
                .Returns(Task.FromResult(deliveryDtoResponse));

            var result = await _deliveriesController.Create(deliveryDto);

            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = (OkObjectResult)result;
            Assert.Equal(deliveryDtoResponse, okObjectResult.Value);
        }

        [Fact]
        public async Task UpdateUnauthorizedExceptionHappenedExpect401()
        {
            var deliveryDto = new DeliveryDto();
            _mockDeliveryService.Setup(service => service.Update(_mockExecutionContext.Object, deliveryDto))
                .Throws<UnauthorizedException>();

            var result = await _deliveriesController.Update("expectedId", deliveryDto);

            Assert.IsType<StatusCodeResult>(result);

            var statusCodeResult = (StatusCodeResult)result;
            Assert.Equal(401, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task UpdateExceptionHappenedExpectBadRequestResult()
        {
            var deliveryDto = new DeliveryDto();
            _mockDeliveryService.Setup(service => service.Update(_mockExecutionContext.Object, deliveryDto))
                .Throws<Exception>();

            var result = await _deliveriesController.Update("expectedId", deliveryDto);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateNullDtoExpectBadRequestResult()
        {
            var result = await _deliveriesController.Update("expectedId", null);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateExpectReturnObjectFromUpdate()
        {
            const string expectedId = "expectedId";
            var deliveryDto = new DeliveryDto();
            var deliveryDtoResponse = new DeliveryDto();

            _mockDeliveryService.Setup(service => service.Update(_mockExecutionContext.Object, deliveryDto))
                .Returns(Task.FromResult(deliveryDtoResponse));

            var result = await _deliveriesController.Update(expectedId, deliveryDto);

            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = (OkObjectResult)result;
            Assert.Equal(deliveryDtoResponse, okObjectResult.Value);
        }
    }
}
