using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeliveryPlatform.Core.Interfaces;
using DeliveryPlatform.HostedService;
using DeliveryPlatform.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace DeliveryPlatform.Api.Tests.HostedServices
{
    public class DeliveryExpirationBackgroundServiceTests
    {
        private readonly DeliveryExpirationBackgroundService _deliveryExpirationBackgrondService;
        private const int ExpectedTestTimeoutMilliseconds = 1000;
        private readonly Mock<IExpirationService> _mockExpirationService;


        public DeliveryExpirationBackgroundServiceTests()
        {
            _mockExpirationService = new Mock<IExpirationService>();

            var mockOptions = new Mock<IOptions<DeliveryExpirationSettings>>();
            mockOptions.SetupGet(options => options.Value).Returns(new DeliveryExpirationSettings
            {
                CheckExpirationTime = ExpectedTestTimeoutMilliseconds
            });

            var mockLogger = new Mock<ILogger<DeliveryExpirationBackgroundService>>();

            _deliveryExpirationBackgrondService = new DeliveryExpirationBackgroundService(mockOptions.Object,
                mockLogger.Object, _mockExpirationService.Object);
        }

        [Fact]
        public async Task ExecuteAsyncExpectRunFewTimes()
        {
            await _deliveryExpirationBackgrondService.StartAsync(new CancellationToken());
            await Task.Delay(ExpectedTestTimeoutMilliseconds * 5);
            await _deliveryExpirationBackgrondService.StopAsync(new CancellationToken());

            _mockExpirationService.Verify(service => service.UpdateExpirations(), Times.AtLeast(2));
        }
    }
}
